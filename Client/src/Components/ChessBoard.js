import './chessboard.css';
import { createContext, useContext, useState } from 'react';
import { ChessContext } from '../Pages/Chess';
import { GetCookies } from '../Functions/HelperMethods';
import ChessBoardPiece from './ChessBoardPiece';
const ChessBoardContext = createContext(null);

function ChessBoard() {
    const [selectedPiece, setSelectedPiece] = useState(null);

    var cookies = GetCookies();
    const { reversed, chessState } = useContext(ChessContext);
    const chessBoardDisplay = reversed ? [...chessState.board].reverse() : chessState.board;
    var ourTurn = cookies.user === chessState.currentPlayer;
    if (chessState.gameDone) return (<div>gamedone</div>)
    return (
        <ChessBoardContext.Provider value={{selectedPiece, setSelectedPiece, ourTurn}}>
            <div className='wrapper'>
                <div className='chessboard'>
                    {chessBoardDisplay.map((row, rowIndex) => (
                        row.map((piece, colIndex) => {
                            let rowCol = [rowIndex, colIndex];
                            return (
                                <ChessBoardPiece key={rowCol} piece={piece} rowCol={rowCol}/>
                            );
                        })
                    ))}
                </div>
            </div>
        </ChessBoardContext.Provider>
    );
}

export { ChessBoardContext }; // export context, can be imported from children
export default ChessBoard;