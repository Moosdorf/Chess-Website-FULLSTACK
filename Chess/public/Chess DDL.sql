-- drops relations then entities

-- r
drop table if exists chess_game_moves;
drop table if exists player_chessgame;
drop table if exists play_move;
drop table if exists player_session;
drop table if exists player_customization;



-- e
drop table if exists chess_game;
drop table if exists chess_moves;
drop table if exists player;
drop table if exists sessions;
drop table if exists customization;



-- Entities

create table chess_game (
  chess_id serial primary key,
  player_id1 int,
  player_id2 int,
  win varchar default null,
  on_going varchar default 'on-going',
  started timestamp,
  ended timestamp default null
);

create table chess_moves (
  move_id varchar primary key not null,
  player int not null,
  move varchar not null,
  move_number int not null
);

create table player (
  p_id serial primary key,
  username varchar unique not null,
  password varchar not null,
  email varchar unique not null
);

create table sessions(
  session_id serial primary key,
  created_at timestamp,
  last_seen timestamp,
  expiration_token varchar
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
  move_id varchar not null,
  chess_id int not null,
  primary key(move_id, chess_id),
  foreign key (move_id) references chess_moves,
  foreign key (chess_id) references chess_game
);

create table player_chessgame (
  p_id int,
  chess_id int,
  primary key(chess_id),
  foreign key (p_id) references player on delete set null,
  foreign key (chess_id) references chess_game
);

create table play_move (
  p_id int,
  move_id varchar,
  move varchar,
  primary key(p_id, move_id),
  foreign key (p_id) references player on delete set null,
  foreign key (move_id) references chess_moves
);

create table player_session (
  u_id int,
  session_id int,
  primary key (u_id, session_id),
  foreign key (u_id) references player,
  foreign key (session_id) references sessions

);

create table player_customization (
  u_id int,
  cust_id int,
  primary key (u_id, cust_id),
  foreign key (u_id) references player,
  foreign key (cust_id) references customization
);
