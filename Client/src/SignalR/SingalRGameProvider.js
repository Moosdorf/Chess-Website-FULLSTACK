import { createContext, useContext, useEffect, useRef, useState, useCallback } from "react";
import * as signalR from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Data/AuthProvider";
import { useSignalRConnection } from "./SignalRProvider";
import Piece from "../Data/Piece";

const SignalRGameContext = createContext(null);

export function SignalRGameProvider({ children }) {
  const { user } = useAuth();
  const { connection } = useSignalRConnection();
  const navigate = useNavigate();
  const handlersRegistered = useRef(false);

  const [messages, setMessages] = useState([{ id: 0, sender: 'System', text: 'Game started!', isOwn: false }]);
  const [queue, setQueue] = useState(null);
  const [chessState, setChessState] = useState(null);

  // --- Event Handlers ---
  const handleSetChessBoard = useCallback((apiBoard, moves=null) => {
    let tempChessBoard = apiBoard.chessboard.map(row =>
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
    );
    const botGame = apiBoard.players.includes("stockfish");
    const playerWhite = apiBoard.players[0];
    const fenSplit = apiBoard.fen.split(" ");

    setChessState({
      board: tempChessBoard,
      id: apiBoard.id,
      fen: apiBoard.fen,
      isWhitesTurn: apiBoard.isWhite,
      moves: parseInt(fenSplit[5]) * 2 + (fenSplit[1] === "b"),
      check: apiBoard.check,
      checkMate: apiBoard.checkMate,
      checkBlockers: apiBoard.blockCheckPositions,
      currentPlayer: apiBoard.currentPlayer,
      players: apiBoard.players,
      botGame,
      playerWhite,
      sessionId: apiBoard.sessionId,
      lastMove: apiBoard.lastMove,
      gameDone: apiBoard.gameDone,
      drawRequest: false,
      previousMoves: moves
    });
  }, []);

  // Register handlers
  useEffect(() => {
    if (!connection || handlersRegistered.current) return;

    connection.on("EndGame", (apiBoard) => {
      console.log("ending game?");
      handleSetChessBoard(apiBoard);
    });

    connection.on("ReceiveGame", (apiBoard, moves) => {
      console.log("set gameboard");
      console.log(apiBoard);
      console.log(moves);
      handleSetChessBoard(apiBoard, moves);
    });

    connection.on("WaitingForOpponent", () => setQueue(true));
    connection.on("QueueStopped", () => setQueue(false));

    connection.on("GameReady", (apiBoard) => {
      handleSetChessBoard(apiBoard);
      setQueue(false);
      navigate(`/chess_game/${apiBoard.id}`);
    });

    connection.on("ReceiveDrawResponse", (message) => console.log("received draw response: " + message));

    connection.on("ReceiveDrawRequest", (message) => {
      setChessState(prevState => ({
        ...prevState,
        drawRequest: message
      }));
    });

    connection.on("BadMove", (message) => console.log(message));

    
    connection.on("ReceiveMove", handleSetChessBoard);
    connection.on("ReceiveMessage", (username, message) => {
      setMessages(prev => [...prev, { 
        id: prev.length, 
        sender: username === user ? "You" : username, 
        text: message, 
        isOwn: username === user 
      }]);
    });

    handlersRegistered.current = true;

    return () => {
      connection.off("ReceiveGame");
      connection.off("BadMove");
      connection.off("ReceiveDrawResponse");
      connection.off("ReceiveDrawRequest");
      connection.off("EndGame");
      connection.off("WaitingForOpponent");
      connection.off("QueueStopped");
      connection.off("GameReady");
      connection.off("ReceiveMove");
      connection.off("ReceiveMessage");
      handlersRegistered.current = false;
    };
  }, [connection, user, handleSetChessBoard, navigate]);

  // --- Actions ---

  // get game
  const getGame = useCallback((id) => {
    if (connection?.state === signalR.HubConnectionState.Connected) {
      console.log("calling connection")
      connection.invoke("GetChessGame", Number(id));
    }
  }, [connection]);

  // join game (start matchmaking, join queue)
  const joinGame = useCallback((user) => {
    if (connection?.state === signalR.HubConnectionState.Connected) {
      connection.invoke("JoinGame", user);
    }
  }, [connection, user]);


  // join bot game (start a botgame)
  const joinBotGame = useCallback((isWhite) => {
    if (connection?.state === signalR.HubConnectionState.Connected) {
      connection.invoke("JoinBotGame", user, isWhite);
    }
  }, [connection, user]);

  // stop queue
  const stopQueue = useCallback(() => {
    if (connection?.state === signalR.HubConnectionState.Connected) {
      connection.invoke("StopQueue");
    }
  }, [connection]);

  // send message in game
  const sendMessage = useCallback((message, sessionId) => {
    if (connection) connection.invoke("SendMessageToGroup", message, sessionId);
  }, [connection, user]);

  // send move
  const sendMove = useCallback((gameId, sessionId, move) => {
    if (connection) connection.invoke("MakeMove", gameId, sessionId, move);
  }, [connection]);

  // send forfeit
  const forfeitGame = useCallback((sessionId) => {
    if (connection) connection.invoke("ForfeitGame", sessionId);
    console.log("go forfeit : ", sessionId)
  }, [connection]);

  // send draw response
  const sendDrawResponse = useCallback((sessionId, response) => {
    if (connection) connection.invoke("SendDrawResponse", sessionId, response);
    console.log("go send draw response: ", response)
  }, [connection]);

  // send draw request
  const sendDrawRequest = useCallback((sessionId) => {
    if (connection) connection.invoke("RequestDraw", sessionId);
    console.log("go send draw : ", sessionId)
  }, [connection]);


  // leave game
  const leaveGame = useCallback((sessionId) => {
    console.log(sessionId);
    if (connection && sessionId) {
      connection.invoke("LeaveGame", sessionId);
      setMessages([]);
      setChessState(null);
    }
  }, [connection]); 


  return (
    <SignalRGameContext.Provider value={{ connection, chessState, messages, queue, getGame, leaveGame, forfeitGame, sendDrawResponse, sendDrawRequest, joinGame, joinBotGame, stopQueue, sendMessage, sendMove }}>
      {children}
    </SignalRGameContext.Provider>
  );
}

export function useSignalRGame() {
  return useContext(SignalRGameContext);
}
