import 'bootstrap/dist/css/bootstrap.css';
import './chessboard.css';
import { useContext, useState } from 'react';
import { ChessContext } from '../Pages/Chess';
import Image from 'react-bootstrap/Image';

function ChessBoard() {
    const {reversed, chessBoard, movePiece} = useContext(ChessContext);
    const chessBoardDisplay = (reversed) ? [...chessBoard.board].reverse() : chessBoard.board;
    const [selectedPiece, setSelectedPiece] = useState(null);
    var lightBrown = "rgb(239, 222, 205)";
    var darkBrown = "rgb(159, 129, 112)";
    

      

    const drag = (e, piece) => {
        // Hide original image temporarily
        e.target.style.opacity = '0.6';
        // create new image from react and put it on cursor
        const dragImage = document.createElement('div');
        dragImage.style.backgroundImage = `url(/images/${piece.IsWhite ? "white" : "black" }-${piece.Type}.png)`;
        dragImage.style.backgroundSize = 'cover';
        dragImage.style.width = '75px';
        dragImage.style.height = '75px';
        dragImage.style.position = 'absolute';
        dragImage.style.top = '-9999px'; // Move off-screen
        dragImage.style.opacity = '1'; 

        document.body.appendChild(dragImage);
    
        // Center the drag image on the cursor
        e.dataTransfer.setDragImage(dragImage, 37, 37);
    
        // Clean up the drag image after the drag operation
        setTimeout(() => document.body.removeChild(dragImage), 0);
    

        e.dataTransfer.setData("text", JSON.stringify(piece)); // stringify the data as JSON
    }
    const dragOver = (e) => { 
        e.stopPropagation(); // parent wont get dragover
        e.preventDefault(); // by default we cant drop stuff
    }
    const addSelected = (selected) => {
        if (selected.IsWhite === chessBoard.isWhitesTurn && selected.Type !== "empty") setSelectedPiece(selected);
        else setSelectedPiece(null);
    };
    const removeSelected = () => {
        setSelectedPiece(null);
    };
    const dragEnd = (e) => {
        e.target.style.opacity = '1';
        removeSelected();
    };
    const handleOnClick = (clickedPiece) => {
        // to do: check om der allerede er en selected, hvis der er, så check hvis den der klikkes på kan være et capture eller move
        if (selectedPiece == null) {
            addSelected(clickedPiece)
            return;
        };
        if (clickedPiece != selectedPiece) {
            if (selectedPiece.AvailableMoves.includes(clickedPiece.Position)) {
                movePiece(selectedPiece, clickedPiece);
                removeSelected();
            } else {
                addSelected(clickedPiece);
            }
            return;
        } 
        removeSelected();
    }



    return (
        <div className='wrapper'>
            <div className='chessboard'>
                {chessBoardDisplay && chessBoardDisplay.map((row, rowIndex) => (
                    row.map((piece, colIndex) => {
                        const isSelected = piece === selectedPiece;
                        const isTarget = selectedPiece && selectedPiece.AvailableMoves.includes(piece.Position);
                        let className = `square ${piece.Type} ${piece.IsWhite ? "white" : "black"} ${isSelected && 'selected'}`;

                        let color = ((rowIndex + colIndex) % 2 === 0 ? darkBrown : lightBrown);
                        if (reversed) {color = (color === lightBrown) ? darkBrown : lightBrown;}

                        let style = {backgroundColor: color};
                        let image = `/images/${(piece.IsWhite) ? "white" : "black"}-${piece.Type}.png`;
                        return (
                        <div onDragOver={e => dragOver(e)} 
                             onDrop={() => movePiece(selectedPiece, piece)} 
                             onClick={() => handleOnClick(piece)} 
                             style={style}  
                             className={className}       
                             key={piece.Position}
                        >
                            {piece.Type != "empty" && <img className={`piece`} 
                                 src={image}
                                 alt=''
                                 draggable={piece.IsWhite === chessBoard.isWhitesTurn} 
                                 onDragStart={(e) => {
                                    drag(e, piece);
                                    addSelected(piece);}
                                }
                                 onDragEnd={(e) => dragEnd(e)}/>}
                            {isTarget && <div className='target'></div>}

                            
                        </div>)
                    })
                ))}
            </div>

            
        </div>
    );
}

export default ChessBoard;