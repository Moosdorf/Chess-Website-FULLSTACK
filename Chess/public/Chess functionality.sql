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






/*- logout -> logout and expire the session

- play chess -> create chess table with the userss
- play move -> insert move into tables with relation to the chess game
- calculate_rating -> on update of win in chess

- get chess history -> all chess games played in order
- get leaderboard -> get all userss and their rating in descending order
- get move history from specific game -> get move history from


- friends?

- searching for usernames

*/