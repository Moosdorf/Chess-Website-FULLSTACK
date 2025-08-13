import { useContext } from 'react';
import { ChessContext } from '../Pages/Chess';

function ChessBoardPieceDisplay({ piece, rowCol, reversed }) {
    const context = useContext(ChessContext); 

    let type;

    // will use default values when used without a context provider
    const chessBoardHistory = context?.chessBoardHistory ?? [];
    const activeMoveIndex = context?.activeMoveIndex ?? 0;

    switch (piece.toLowerCase()) {
        case "p": type = "pawn"; break;
        case "n": type = "knight"; break;
        case "b": type = "bishop"; break;
        case "r": type = "rook"; break;
        case "q": type = "queen"; break;
        case "k": type = "king"; break;
        default: type = "empty"; break;
    }

    const lightBrown = "rgb(239, 222, 205)";
    const darkBrown = "rgb(159, 129, 112)";
    const image = `/images/${piece === piece.toLowerCase() ? "black" : "white"}-${type}.png`;
    const color = ((rowCol[0] + rowCol[1]) % 2 === 0 ? darkBrown : lightBrown);
    const style = { backgroundColor: reversed ? (color === lightBrown ? darkBrown : lightBrown) : color };

    let className = `square ${type} ${piece === piece.toUpperCase() ? "white" : "black"}`;

    // Only run move highlight logic if context is available
    if (context && chessBoardHistory[activeMoveIndex]?.move) {
        let [from, to] = chessBoardHistory[activeMoveIndex].move.split(",");
        const [row, col] = rowCol;
        const rank = row + 1;
        const file = String.fromCharCode(col + 97);
        const position = `${file}${rank}`;

        if (!from) from = "none";
        if (!to) to = "none";

        if (position === from) className += " movedFrom";
        if (position === to) className += " movedTo";
    }

    return (
        <div className={className} style={style}>
            {type !== "empty" && (
                <img className="piece" src={image} alt="" />
            )}
        </div>
    );
}

export default ChessBoardPieceDisplay;
