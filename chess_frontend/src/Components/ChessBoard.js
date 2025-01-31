import 'bootstrap/dist/css/bootstrap.css';
import './chessboard.css';
import { useContext, useState } from 'react';
import { ReversedContext } from '../Pages/Chess';
import Image from 'react-bootstrap/Image';

function ChessBoard({chessState, move, turnColor}) {
    const reversed = useContext(ReversedContext);
    const chessBoard = (reversed) ? [...chessState].reverse() : chessState;
    var lightBrown = "rgb(239, 222, 205)";
    var darkBrown = "rgb(159, 129, 112)";
    const [selectedPiece, setSelectedPiece] = useState(null);
    
    const dragOver = (e, piece) => { 
        e.stopPropagation(); // parent wont get dragover
        e.preventDefault(); // by default we cant drop stuff
    }
    
    const drag = (e, piece) => {

        // create new image from react and put it on cursor
        const dragImage = document.createElement('div');
        dragImage.style.backgroundImage = `url(/images/${piece.color}-${piece.piece}.png)`;
        dragImage.style.backgroundSize = 'cover';
        dragImage.style.width = '50px';
        dragImage.style.height = '50px';
        dragImage.style.position = 'absolute';
        dragImage.style.top = '-9999px'; // Move off-screen

        document.body.appendChild(dragImage);
    
        // Center the drag image on the cursor
        e.dataTransfer.setDragImage(dragImage, 25, 25);
    
        // Clean up the drag image after the drag operation
        setTimeout(() => document.body.removeChild(dragImage), 0);
    

        e.dataTransfer.setData("text", JSON.stringify(piece)); // stringify the data as JSON
    }

    const addSelected = (id) => {
        const squareId = `${id}`;
        setSelectedPiece(squareId);
    };

    return (
        <div className='wrapper'>
            <div className='chessboard'>
                {chessState && chessBoard.map((row, rowIndex) => (
                    row.map((piece, colIndex) => {
                        const isSelected = `${piece.id}` === selectedPiece;
                        let className = `square ${piece.piece} ${piece.color} ${isSelected ? 'selected' : ''}`;

                        let color = ((rowIndex + colIndex) % 2 === 0 ? darkBrown : lightBrown);
                        if (reversed) {color = (color === lightBrown) ? darkBrown : lightBrown;}

                        let style = {backgroundColor: color};
                        let image = `/images/${piece.color}-${piece.piece}.png`;
                        let draggablePiece = piece.color === turnColor;
                        return (
                        <div onDragOver={e => dragOver(e, piece)} 
                             onDrop={e => move(e, piece)} 
                             onClick={() => addSelected(piece.id)} 
                             style={style}  
                             className={className}       
                             key={piece.id}
                        >
                            <img className='piece' 
                                 src={image}
                                 alt=''
                                 draggable={draggablePiece} 
                                 onDragStart={(e) => drag(e, piece)}/>
                        </div>)
                    })
                ))}
            </div>

            
        </div>
    );
}

export default ChessBoard;