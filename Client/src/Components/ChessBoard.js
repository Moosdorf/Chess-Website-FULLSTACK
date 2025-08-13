import './chessboard.css';
import { createContext, useContext, useState } from 'react';
import { ChessContext } from '../Pages/Chess';
import { GetCookies } from '../Functions/HelperMethods';
import ChessBoardPiece from './ChessBoardPiece';
import ChessBoardPieceDisplay from './ChessBoardPieceDisplay';

const ChessBoardContext = createContext(null);

function ChessBoard() {
    const [selectedPiece, setSelectedPiece] = useState(null);

    const cookies = GetCookies();
    const { reversed, chessState, chessBoardHistory, activeMoveIndex } = useContext(ChessContext);
    const ourTurn = cookies.user === chessState.currentPlayer;

    // --- Determine what to display based on activeMoveIndex ---
    let displayBoard;
    let fenMode = false;

    if (activeMoveIndex !== "current" && typeof chessBoardHistory[activeMoveIndex]?.fen === "string") {
        // History view in FEN format
        displayBoard = chessBoardHistory[activeMoveIndex].fen;
        fenMode = true;
    } else {
        // Current board or structured board history
        displayBoard = reversed ? [...chessState.board].reverse() : chessState.board;
    }

    const reverse = (FEN, color) => {
        if (color === true) return FEN;

        const rows = FEN.split(" ")[0].split("/");
        rows.reverse();
        FEN = rows.join("/") + " " + FEN.split(" ").slice(1).join(" ");
        return FEN;
    }

    // --- Render ---
    return (
        <ChessBoardContext.Provider value={{ selectedPiece, setSelectedPiece, ourTurn }}>
            <div className="wrapper">
                <div className="chessboard">
                    {fenMode
                        ? reverse(displayBoard, reversed).replace(/[1-8]/g, (digit) => "e".repeat(parseInt(digit)))
                              .split(" ")[0]
                              .split("/")
                              .flatMap((row, rowIndex) =>
                                  row.split("").map((piece, colIndex) => (
                                      <ChessBoardPieceDisplay
                                          key={`${rowIndex}-${colIndex}`}
                                          piece={piece}
                                          rowCol={[rowIndex, colIndex]}
                                          reversed={reversed}
                                      />
                                  ))
                              )
                        : displayBoard.map((row, rowIndex) =>
                              row.map((piece, colIndex) => {
                                  const rowCol = [rowIndex, colIndex];
                                  return (
                                      <ChessBoardPiece
                                          key={`${rowIndex}-${colIndex}`}
                                          piece={piece}
                                          rowCol={rowCol}
                                      />
                                  );
                              })
                          )}
                </div>
            </div>
        </ChessBoardContext.Provider>
    );
}

export { ChessBoardContext };
export default ChessBoard;
