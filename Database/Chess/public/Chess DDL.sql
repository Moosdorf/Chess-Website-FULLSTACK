-- drops relations then entities

-- r
drop table if exists chess_game_moves;
drop table if exists user_chessgame;
drop table if exists play_move;
drop table if exists user_session;
drop table if exists user_customization;



-- e
drop table if exists chess_game;
drop table if exists chess_moves;
drop table if exists users;
drop table if exists sessions;
drop table if exists customization;



-- Entities

create table chess_game (
  chess_id serial primary key,
  player_id1 int,
  player_id2 int,
  win varchar default null,
  on_going bool default true,
  started timestamp,
  ended timestamp default null
);

create table chess_moves (
  move_id serial primary key,
  u_id int not null,
  move varchar not null,
  move_number int not null
);

create table users (
  u_id serial primary key,
  username varchar unique not null,
  password varchar not null,
  email varchar unique not null
);

create table sessions (
  session_id serial primary key,
  created_at timestamp,
  ended_at timestamp,
  expiration_token varchar default null
);

create table customization(
  cust_id serial primary key,
  board_pref varchar default 'normal',
  piece_pref varchar default 'normal',
  dark_mode bool default false,
  sound_volume int default 100
);

-- relations

create table chess_game_moves (
  move_id int not null,
  chess_id int not null,
  primary key(move_id, chess_id),
  foreign key (move_id) references chess_moves,
  foreign key (chess_id) references chess_game
);

create table user_chessgame (
  u_id int,
  chess_id int,
  foreign key (u_id) references users on delete set null,
  foreign key (chess_id) references chess_game
);

create table play_move (
  u_id int,
  move_id int,
  move varchar,
  primary key(u_id, move_id),
  foreign key (u_id) references users on delete set null,
  foreign key (move_id) references chess_moves
);

create table user_session (
  u_id int,
  session_id int,
  primary key (u_id, session_id),
  foreign key (u_id) references users,
  foreign key (session_id) references sessions

);

create table user_customization (
  u_id int,
  cust_id int,
  primary key (u_id, cust_id),
  foreign key (u_id) references users,
  foreign key (cust_id) references customization
);
