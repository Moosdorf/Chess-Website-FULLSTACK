import 'bootstrap/dist/css/bootstrap.css';
import './chessboard.css';
import { useContext, useState } from 'react';
import { ChessContext } from '../Pages/Chess';
import { Button } from 'react-bootstrap';

function ChessBoard() {
    const { reversed, chessBoard, movePiece } = useContext(ChessContext);
    const chessBoardDisplay = reversed ? [...chessBoard.board].reverse() : chessBoard.board;
    const [selectedPiece, setSelectedPiece] = useState(null);
    const [promotionInfo, setPromotionInfo] = useState(null);
    const promotionTypes = ["queen", "rook", "bishop", "knight"];
    const lightBrown = "rgb(239, 222, 205)";
    const darkBrown = "rgb(159, 129, 112)";

      

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
    const dragEnd = (e, piece) => {
        e.target.style.opacity = '1';
        if (piece === selectedPiece) return;
        removeSelected();
    };

    const addSelected = (selected) => {
        if (selected.IsWhite === chessBoard.isWhitesTurn && selected.Type !== "empty") setSelectedPiece(selected);
        else setSelectedPiece(null);
    };

    const removeSelected = () => {
        setSelectedPiece(null);
    };

    const attack = (clickedPiece, promotion) => {
        movePiece(selectedPiece, clickedPiece, promotion);
        setPromotionInfo(null);
        removeSelected();
    }

    const onDrop = (piece, promotion) => {
        if (CheckForPromotion(piece)) return;

        if (piece === selectedPiece) { // if the piece is dropped on itself, just reselect it
            addSelected(piece);
            return;
        }
        if (selectedPiece.AvailableMoves.includes(piece.Position) || selectedPiece.AvailableCaptures.includes(piece.Position)) {
            attack(piece, promotion);
        }
        return;
         
    };

    const handlePromotionSelection = (piece, promotionType) => {
        attack(promotionInfo.to, promotionType);
    };



    const handleOnClick = (clickedPiece) => {
        console.log(clickedPiece);
        // If we're in promotion mode, don't process normal clicks
        if (promotionInfo) return;

        if (CheckForPromotion(clickedPiece)) return;

        if (selectedPiece === null) {
            addSelected(clickedPiece);
            return;
        } 

        if (clickedPiece !== selectedPiece) {
            addSelected(clickedPiece);
            if (selectedPiece.AvailableMoves.includes(clickedPiece.Position) || 
                selectedPiece.AvailableCaptures.includes(clickedPiece.Position)) {
                attack(clickedPiece, null);
            } 
            return;
        } 
        
        removeSelected();
    }

    const CheckForPromotion = (clickedPiece) => {
        if (selectedPiece === null || selectedPiece.Type !== "pawn") return false;

        const targetRank = clickedPiece.Position[1];
        const isPromotionRank = (selectedPiece.IsWhite && targetRank === '8') || 
                        (!selectedPiece.IsWhite && targetRank === '1');

        if (isPromotionRank && 
            (selectedPiece.AvailableMoves.includes(clickedPiece.Position) || 
                        selectedPiece.AvailableCaptures.includes(clickedPiece.Position))) {

            const direction = selectedPiece.IsWhite ? -1 : 1;
            const promotionSquares = [];

            for (let i = 0; i < 4; i++) {
                const rank = String.fromCharCode(targetRank.charCodeAt(0) + (i * direction));
                promotionSquares.push({
                    position: clickedPiece.Position[0] + rank,
                    type: promotionTypes[i]
                });
            }
            
            setPromotionInfo({
                from: selectedPiece,
                to: clickedPiece,
                squares: promotionSquares
            });
            return true;
        }
        return false;
    }
    


    

return (
        <div className='wrapper'>
            <div className='chessboard'>
                {chessBoardDisplay.map((row, rowIndex) => (
                    row.map((piece, colIndex) => {
                        const isSelected = piece === selectedPiece;
                        const isMove = selectedPiece && selectedPiece.AvailableMoves.includes(piece.Position);
                        const isTarget = selectedPiece && selectedPiece.AvailableCaptures.includes(piece.Position);
                        const promotionOverlay = promotionInfo?.squares.find(
                            s => s.position === piece.Position
                        );

                        const className = `square ${piece.Type} ${piece.IsWhite ? "white" : "black"} ${isSelected && 'selected'}`;
                        const color = ((rowIndex + colIndex) % 2 === 0 ? darkBrown : lightBrown);
                        const style = { backgroundColor: reversed ? (color === lightBrown ? darkBrown : lightBrown) : color };
                        const image = `/images/${piece.IsWhite ? "white" : "black"}-${piece.Type}.png`;

                        return (
                            <div 
                                onDragOver={dragOver} 
                                onDrop={() => onDrop(piece)} 
                                onClick={() => handleOnClick(piece)} 
                                style={style}  
                                className={className}       
                                key={piece.Position}
                            >
                                {piece.Type !== "empty" && (
                                    <img 
                                        className="piece"
                                        src={image}
                                        alt=""
                                        draggable={piece.IsWhite === chessBoard.isWhitesTurn} 
                                        onDragStart={(e) => {
                                            drag(e, piece);
                                            addSelected(piece);
                                        }}
                                        onDragEnd={(e) => dragEnd(e, piece)}
                                    />
                                )}
                                {isTarget && <div className='target'></div>}
                                {isMove && <div className='move'></div>}
                                <div className='overlay'>{piece.Position}</div>
                                {promotionOverlay && (
                                    <div 
                                        className='promotionOverlay'
                                        onClick={() => handlePromotionSelection(piece, promotionOverlay.type)}
                                    >
                                        <img 
                                            src={`/images/${chessBoard.isWhitesTurn ? "white" : "black"}-${promotionOverlay.type}.png`}
                                            alt={promotionOverlay.type}
                                        />
                                    </div>
                                )}
                            </div>
                        );
                    })
                ))}
            </div>
        </div>
    );
}

export default ChessBoard;