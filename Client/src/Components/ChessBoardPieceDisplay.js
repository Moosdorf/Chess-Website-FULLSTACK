

function ChessBoardPieceDisplay({piece, rowCol, reversed}) {
    let type;

    switch (piece.toLowerCase()) {
        case "p":
            type = "pawn";
            break;
        case "n":
            type = "knight";
            break;
        case "b":
            type = "bishop";
            break;
        case "r":
            type = "rook";
            break;
        case "q":
            type = "queen";
            break;
        case "k":
            type = "king";
            break;
        default:
            type = "empty";
            break;
    }


    const lightBrown = "rgb(239, 222, 205)";
    const darkBrown = "rgb(159, 129, 112)";

    const image = `/images/${piece === piece.toLowerCase() ? "black" : "white"}-${type}.png`;

    const color = ((rowCol[0] + rowCol[1]) % 2 === 0 ? darkBrown : lightBrown);
    const style = { backgroundColor: reversed ? (color === lightBrown ? darkBrown : lightBrown) : color };



    const className = `square ${type} ${piece === piece.toUpperCase() ? "white" : "black"}`;

    return (<div 
            className={className} 
            style={style}
            >
            {type !== "empty" && (
                <img 
                    className="piece"
                    src={image}
                    alt=""
                />
            )}

        </div>)
}
export default ChessBoardPieceDisplay;