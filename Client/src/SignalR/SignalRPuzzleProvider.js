import { createContext, useContext, useEffect, useRef, useState, useCallback } from "react";
import * as signalR from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Data/AuthProvider";
import { useSignalRConnection } from "./SignalRProvider";
import Piece from "../Data/Piece";

const SignalRPuzzleContext = createContext(null);

export function SignalRPuzzleProvider({ children }) {
  const { user } = useAuth();
  const { connection } = useSignalRConnection();
  const navigate = useNavigate();
  const handlersRegistered = useRef(false);

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
      puzzleId: apiBoard.id,
      fen: apiBoard.fen,
      isWhitesTurn: apiBoard.isWhite,
      moves: parseInt(fenSplit[5]) * 2 + (fenSplit[1] === "b"),
      check: apiBoard.check,
      checkBlockers: apiBoard.blockCheckPositions,
      botGame,
      playerWhite,
      sessionId: apiBoard.sessionId,
      lastMove: apiBoard.lastMove,
      gameDone: apiBoard.gameDone,
      previousMoves: moves
    });
  }, []);

  // Register handlers
  useEffect(() => {
    if (!connection || handlersRegistered.current) return;


    handlersRegistered.current = true;

    return () => {

      handlersRegistered.current = false;
    };
  }, [connection]);

  // --- Actions ---

  // send move
  const sendMove = useCallback((gameId, sessionId, move) => {
    if (connection) connection.invoke("MakeMove", gameId, sessionId, move);
  }, [connection]);


  return (
    <SignalRPuzzleContext.Provider value={{ connection }}>
      {children}
    </SignalRPuzzleContext.Provider>
  );
}

export function useSignalRPuzzle() {
  return useContext(SignalRPuzzleContext);
}
