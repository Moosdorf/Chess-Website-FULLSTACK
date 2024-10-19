/*
1. Check if it is possible to create a user. also have customizations already created with trigger
*/

-- test w

call create_user('Kongo', 'hashed', 'kongomail@email.com', null);


-- check results
select * from users natural join user_customization natural join customization;

select * from customization;

/*
2. Check login with session
*/

-- tests

call login('Kongo', 'hashed', null);

select * from get_session_history('Kongo');


/*
3. Check logout with session
*/

call logout('Kongo', null);
select * from sessions;


/*
3. With two users check if the system can create a new chess game with both players
*/

call create_user('1', 'hashed', '1@email.com', null);
call create_user('2', 'hashed', '2@email.com', null);


-- start using ids, they are 2 and 3 respectively
call create_chess_game(2,3);

select * from chess_game;


/*
4. Check if the game can be played with move(). 
*/





/*
5. Add a bunch of random games between the two players and check rating function

6. Check get functions, game_history(user), move_history(game), get leader_board.

7. Check user customization functionality

8. Check logout with session

9. Searching for other usernames

10. Friends?

11. Chat in game?

*/