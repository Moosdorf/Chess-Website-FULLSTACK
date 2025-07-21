import { createContext, useContext, useEffect, useRef, useState, useCallback } from "react";
import * as signalR from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Data/AuthProvider";
import { useSignalRConnection } from "./SignalRProvider";
import Piece from "../Data/Piece";

const SignalRGameContext = createContext(null);

export function SignalRGameProvider({ children }) {
  const { user } = useAuth();
  const connection = useSignalRConnection();
  const navigate = useNavigate();
  const handlersRegistered = useRef(false);

  const [messages, setMessages] = useState([{ id: 0, sender: 'System', text: 'Game started!', isOwn: false }]);
  const [queue, setQueue] = useState(null);
  const [chessState, setChessState] = useState(null);

  // --- Event Handlers ---
  const handleSetChessBoard = useCallback((apiBoard) => {
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
      lastMove: apiBoard.lastMove
    });
  }, []);

  // Register handlers
  useEffect(() => {
    if (!connection || handlersRegistered.current) return;

    connection.on("WaitingForOpponent", () => setQueue(true));
    connection.on("QueueStopped", () => setQueue(false));
    connection.on("GameReady", (apiBoard) => {
      console.log(apiBoard);
      handleSetChessBoard(apiBoard);
      setQueue(false);
      navigate("/chess_game");
    });
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
      connection.off("WaitingForOpponent");
      connection.off("QueueStopped");
      connection.off("GameReady");
      connection.off("ReceiveMove");
      connection.off("ReceiveMessage");
      handlersRegistered.current = false;
    };
  }, [connection, user, handleSetChessBoard, navigate]);

  // --- Actions ---

  // join game (start matchmaking, join queue)
  const joinGame = useCallback(() => {
    if (connection?.state === signalR.HubConnectionState.Connected) {
      connection.invoke("JoinGame", user);
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

  // leave game
  const leaveGame = useCallback((sessionId) => {
    console.log("try to leave", connection);
    if (connection && sessionId) {
      console.log("inside")
      setMessages([{ id: 0, sender: 'System', text: 'Game started!', isOwn: false }]);
      connection.invoke("LeaveGame", sessionId);
    }
  }, [connection]);

  return (
    <SignalRGameContext.Provider value={{ chessState, messages, queue, leaveGame, joinGame, stopQueue, sendMessage, sendMove }}>
      {children}
    </SignalRGameContext.Provider>
  );
}

export function useSignalRGame() {
  return useContext(SignalRGameContext);
}
