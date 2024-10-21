-- functions
/*
- create_user -> create user if it is possible
*/

-- procedure to call in order to create the user
drop procedure if exists create_user;
create procedure create_user(in p_username varchar, in p_password varchar, in p_email varchar, out results boolean)
language plpgsql as $$
begin
  if exists ( select 1 
                  from users 
                  where username = p_username)
  then results := false; 
  else 
    insert into users (username, password, email) 
    values (p_username, p_password, p_email);
    results := true;
  end if;
end;
$$;

-- function to get all users and get a single user

drop function if exists get_users;
create function get_users()
returns table (user_id int, username_ int)
language plpgsql as $$
begin 
  select u_id, username
  from users
  order by u_id;
end;
$$;

drop function if exists get_user;
create function get_user(id int)
returns table (user_id int, username_ int)
language plpgsql as $$
begin 
  select u_id, username
  from users
  where u_id = id
  order by u_id;
end;
$$;

-- trigger for creating the customization after the user has been created
drop trigger if exists create_cust on users;
drop function if exists create_customization;

create function create_customization()
returns trigger as $$
declare new_id int;
begin
  insert into customization (board_pref, piece_pref, dark_mode, sound_volume)
  values (default, default, default, default)
  returning cust_id into new_id;

  insert into user_customization values (new.u_id, new_id);
  return null;
end; $$
language plpgsql;

-- must be after insert on users
create trigger create_cust 
after insert on users
for each row execute procedure create_customization();
  


/*
- login -> check if username and password matches
*/

-- procedure to log in and create the session
drop procedure if exists login;
create procedure login(in p_username varchar, in p_password varchar)
LANGUAGE plpgsql
as $$
declare 
  now timestamp := current_timestamp;
  user_id int;
  sess_id int; 


begin 
  if exists ( select 1 
              from users 
              where username = p_username 
              and password = p_password) 
  then
    select u_id 
    into user_id 
    from users where username = p_username;
    
    insert into sessions (created_at, ended_at) 
    values (now, null)
    returning session_id into sess_id;
    
    insert into user_session (u_id, session_id) 
    values (user_id, sess_id);
    
    raise notice 'Sign in successful %, %', user_id, sess_id;

   else
    raise notice 'Does not match, cannot sign in';
  end if;
end;
$$;



-- check if log in matches
drop function if exists check_login_credentials;
create function check_login_credentials(in p_username varchar, in p_password varchar)
returns bool
language plpgsql as $$
declare 
  matches bool;
BEGIN
  select (count(username) > 0) into matches
  from users 
  where username = p_username 
  and password = p_password;
  
  return matches;
end;
$$;



/*
get session_history
*/
drop function if exists get_session_history;
create function get_session_history(p_username varchar)
returns table(sess_id int, user_name varchar, sess_created_at timestamp)
language plpgsql as $$
begin
  return query
    select session_id, username, created_at 
    from users natural join user_session natural join sessions 
    where username = p_username 
    order by created_at desc;
end;
$$;


/*
- update_last_seen -> updates current session

drop procedure if exists update_last_seen;
create procedure update_last_seen(in p_username varchar)
LANGUAGE plpgsql
as $$
declare found_session_id int;
begin 

  select session_id into found_session_id
  from users natural join user_session natural join sessions 
  where username = p_username 
  order by created_at desc limit 1;
  
  update sessions
  set last_seen = current_timestamp
  where session_id = found_session_id;

end;
$$;


-- tests
call update_last_seen('Kongo');

select * 
  from users natural join user_session natural join sessions;
*/



/*
logout -> logout and expire the session
*/
drop procedure if exists logout;
create procedure logout(in _username text, out loggedout int)
LANGUAGE plpgsql
as $$
declare
  user_id int := -1;
begin 

  select u_id into user_id
  from users
  where username = _username;

  if user_id <> -1 then
    update sessions
    set ended_at = current_timestamp
    where session_id = (
          select session_id 
          from sessions 
          natural join user_session
          where u_id = user_id 
          order by sessions desc);
    
  -- logic for session logout
  end if;
end;
$$;




/*
- play chess -> create chess table with the users
when game is won we must update win and on-going
*/

drop procedure if exists create_chess_game;
create procedure create_chess_game(in player_1 int, in player_2 int)
LANGUAGE plpgsql
as $$
declare 
 new_chess_id int;
begin
  -- start ned chess game with both ids
  insert into chess_game (player_id1, player_id2, started)
  values (player_1, player_2, current_timestamp)
  returning chess_id into new_chess_id;
  
  -- start relation 1
  insert into user_chessgame (u_id, chess_id)
  values (player_1, new_chess_id);
  
  -- start relation 2
  insert into user_chessgame (u_id, chess_id)
  values (player_2, new_chess_id);
end;
$$;

/*
- play move -> insert move into tables with relation to the chess game. check if on-going
*/
drop procedure if exists move;
create procedure move(in player_move text, in uid int, in in_chess_id int)
language plpgsql
as $$
declare 
  new_move_number int;
  new_move_id int;
begin
  select (move_number + 1) into new_move_number
  from chess_moves natural join chess_game_moves
  where chess_id = in_chess_id 
  order by move_id desc limit 1;
  
  if (new_move_number is null) then 
    new_move_number := 1;
  end if;
  
  insert into chess_moves (u_id, move, move_number)
  values (uid, player_move, new_move_number)
  returning move_id into new_move_id;
  
  insert into chess_game_moves values (new_move_id, in_chess_id);
  
  insert into play_move values (uid, new_move_id, player_move);
  
end;
$$;



/*
end game
*/
drop procedure if exists end_game;
create procedure end_game(in in_chess_id int, in winner_id int)
language plpgsql
as $$
declare
  winner text;
  not_ended bool;
begin
  
  select on_going into not_ended
  from chess_game 
  where chess_id = in_chess_id;
  
  if not_ended then 
  
    if winner_id > 0 then 
      winner := cast(winner_id as text);
    else 
      winner := 'tied';
    end if;
    
    update chess_game
    set (win, on_going, ended) = (winner, false, current_timestamp)
    where chess_id = in_chess_id;
    
  end if;
  
end;
$$;




/*
- get chess history -> get all chess games a player has played, in order desc
*/

drop function if exists get_user_chess_history;
create function get_user_chess_history(in_u_id int)
returns table (out_chess_id int, out_on_going bool, out_win varchar, out_ended timestamp)
language plpgsql as $$
begin 
  return query
    select chess_id, on_going, win, ended from users 
    natural join user_chessgame
    natural join chess_game
    where u_id = in_u_id
    order by ended desc;

end;
$$;

/*
- get move history from specific game -> get move history from

*/

drop function if exists get_moves_chess_id;
create function get_moves_chess_id(in_chess_id int)
returns table (out_move varchar, out_move_number int, out_chess_id int)
language plpgsql as $$
begin 
  return query
    select move, move_number, chess_id from chess_moves 
    natural join chess_game_moves
    where chess_id = in_chess_id order by move_id desc;
end;
$$;





/*

- calculate_rating -> on update of win in chess
- get leaderboard -> get all userss and their rating in descending order

- friends?
- searching for usernames
*/












