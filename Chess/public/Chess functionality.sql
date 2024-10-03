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


-- test with do statement
do $$
  declare results boolean;
  begin 
    call create_user('Kongo1', 'hashed', 'kongomail1@email.com', results);
    raise notice 'user created: %', results;
  end;
$$;

-- check results
select * from users;


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
  
-- create a new user and check that it has customizations
call create_user('Kongo', 'hashed', 'kongomail@email.com', null);

select * from users natural join user_customization natural join customization;
select * from users;


/*
- login -> check if username and password matches
*/

-- procedure to log in and create the session
drop procedure if exists login;
create procedure login(in p_username varchar, in p_password varchar, out results boolean)
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
    
    insert into sessions (created_at, last_seen) 
    values (now, now)
    returning session_id into sess_id;
    
    insert into user_session (u_id, session_id) 
    values (user_id, sess_id);
    
    results := true;
    raise notice 'Sign in successful %, %', user_id, sess_id;

   else
    raise notice 'Does not match, cannot sign in';
    results := false;
  end if;
end;
$$;



-- tests

call login('Kongo', 'hashed', null);

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

select * from get_session_history('Kongo');

/*
- update_last_seen -> updates current session
*/
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




/*
logout -> logout and expire the session


create procedure logout(in username)
LANGUAGE plpgsql
as $$
begin 
  
-- logic for session logout

end;
$$;
*/



/*
- play chess -> create chess table with the users
when game is won we must update win and on-going
- get chess history -> all chess games played in order

- play move -> insert move into tables with relation to the chess game. check if on-going
- get move history from specific game -> get move history from
*/





/*
- calculate_rating -> on update of win in chess
- get leaderboard -> get all userss and their rating in descending order
*/





/*
- friends?
- searching for usernames
*/












