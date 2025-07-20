import { useCallback, useEffect, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth } from "../Data/AuthProvider";
import Piece from "../Data/Piece";
import { useSignalRConnection } from "./SignalRProvider";



export default function useSignalR() {
  
  // user authentication and connection 
  const { user } = useAuth();
  const connection = useSignalRConnection();

  // messages used in game chat
  const [messages, setMessages] = useState([
      { id: 0, sender: 'System', text: 'Game started!', isOwn: false }
  ]);

  // game state
  const [chessState, setChessState] = useState(null);

  // prevent duplicate handlers
  const handlersRegistered = useRef(false);



  // register handlers
  useEffect(() => {
    console.log("setting chessboard");
    // function to handle the game information sent from the backend
    const handleSetChessBoard = (apiBoard) => {     
        console.log(apiBoard);   
        var tempChessBoard = apiBoard.chessboard;
        tempChessBoard = tempChessBoard.map(row => (
              row.map(piece => new Piece(
                  piece.type, 
                  piece.isWhite, 
                  piece.position, 
                  piece.pinned,
                  piece.moves,
                  piece.captures,
                  piece.availableMoves,
                  piece.availableCaptures,
                  piece.defenders, 
                  piece.attackers,
                  piece.isAlive))
          ));

          // check if any of the players are a bot, and if this player is white
          var botGame = apiBoard.players[0] === "stockfish" || apiBoard.players[1] === "stockfish";
          var playerWhite = apiBoard.players[0];

          // split fen from the api, as we need to set multiple values from it
          var fenSplit = apiBoard.fen.split(" ");
          
          setChessState({board: tempChessBoard, 
              id: apiBoard.id, 
              fen: apiBoard.fen,
              isWhitesTurn: apiBoard.isWhite, 
              moves: parseInt(fenSplit[5]) * 2 + (fenSplit[1] === "b"), 
              check: apiBoard.check, 
              checkMate: apiBoard.checkMate, 
              checkBlockers: apiBoard.blockCheckPositions,
              currentPlayer: apiBoard.currentPlayer,
              players: apiBoard.players,
              botGame: botGame,
              playerWhite: playerWhite,
              sessionId: apiBoard.sessionId,
              lastMove: apiBoard.lastMove});
    }  

    const receiveMessageHandler = (username, message) => {
      setMessages(prev => [...prev, 
        { id: prev.length , sender: username === user ? "You" : username, text: message, isOwn: username === user ? true : false }
      ]);
    }
    const gameReadyHandler = (apiBoard) => {
      console.log("trying to set gameboard")
      handleSetChessBoard(apiBoard);
    };

    const handleWaiting = () => {
      console.log("waiting");
    };

    if (!connection || handlersRegistered.current) return;

    connection.on("ReceiveMove", gameReadyHandler);
    connection.on("ReceiveMessage", receiveMessageHandler); 
    connection.on("GameReady", gameReadyHandler);
    connection.on("WaitingForOpponent", handleWaiting); 

    handlersRegistered.current = true;

    return () => {
      connection.off("ReceiveMove", gameReadyHandler);
      connection.off("ReceiveMessage", receiveMessageHandler);
      connection.off("GameReady", gameReadyHandler);
      connection.off("WaitingForOpponent", handleWaiting); 
      handlersRegistered.current = false;
    };
  }, [connection, user]);

  
  // join game
  const joinGame = useCallback(async () => {
    console.log("trying to invoke in callback" + connection?.state)
    if (connection?.state === signalR.HubConnectionState.Connected) {
      await connection.invoke("JoinGame", user);
    }
  }, [connection, user]);
  
  

  // send message
  const sendMessage = useCallback(async (message, sessionId) => {
    if (!connection) return;

    await connection.invoke("SendMessageToGroup", user, message, sessionId);
  }, [user, connection]);

  // send move
  const sendMove = useCallback(async (gameId, sessionId, move) => {
    if (!connection) return;
    console.log("connection stable")
    await connection.invoke("MakeMove", gameId, sessionId, move);
  }, [user, connection]);


  return { connection, messages, chessState, joinGame,  sendMove, sendMessage };
};

    