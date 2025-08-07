import './chessboard.css';
import ChessBoardPieceDisplay from './ChessBoardPieceDisplay';

function ChessBoardDisplay({ FEN, color }) {
    if (!FEN) return <></>
    if (color !== "White") {
        const rows = FEN.split(" ")[0].split("/");
        rows.reverse();
        FEN = rows.join("/") + " " + FEN.split(" ").slice(1).join(" ");
    }
    FEN = FEN.replace(/[1-8]/g, (digit) => "e".repeat(parseInt(digit))); // replaces numbers with e, if 4 then eeee, because of empty spots
    return (
            <div className=''>
                <div className='chessboardDisplay'>
                    {FEN.split(" ")[0].split("/").flatMap((row, rowIndex) => 
                        row.split("").map((piece, colIndex) => (
                            <ChessBoardPieceDisplay key={`${rowIndex}-${colIndex}`} piece={piece} rowCol={[rowIndex, colIndex]} reversed={color !== "White"} />
                        ))
                    )}
                </div>
            </div>
    );
}

export default ChessBoardDisplay;