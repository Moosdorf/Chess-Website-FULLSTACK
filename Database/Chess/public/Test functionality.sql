/*
1. Check if it is possible to create a user. also have customizations already created with trigger
*/

-- test w

call create_user('Kongo', 'hashed', 'kongomail@email.com', null);


-- check results
select * from users;
select * from customization;

/*
2. Check login with session
*/

-- tests

call login('Kongo', 'hashed');

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
-- move, player id, chess id
call move('player 1 moved here', 1, 1);
call move('player 2 moved here', 2, 1);
call move('player 1 now moved here', 1, 1);
call move('player 2 now moved here', 2, 1);


select * from play_move 
natural join chess_moves
natural join chess_game_moves;

/*
5. end game
*/

-- chess_game_id, winner_id (-1 for tie)
call end_game(1, -1);


select * from chess_game;



/*
6. Add a bunch of random games between the two players and check rating function
*/
-- have not made rating yet
-- have not made rating yet

call create_chess_game(2,3);
call create_chess_game(2,3);
call create_chess_game(2,3);
call create_chess_game(2,3);
-- have not made rating yet
-- have not made rating yet

call end_game(2, 3);
call end_game(3, 2);
call end_game(4, 2);
call end_game(5, 2);
-- have not made rating yet

-- have not made rating yet

/*
7. Check get functions, game_history(user), move_history(game), get leader_board.
*/

-- user id
select * from get_user_chess_history(2);

-- chess id
select * from get_moves_chess_id(1);

/*

8. Check user customization functionality

*/

select * from users natural join user_customization natural join customization; 



/* maybe make these in future
9. Searching for other usernames

10. Friends?

11. Chat in game?

*/