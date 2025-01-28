import 'bootstrap/dist/css/bootstrap.css';
import './chessboard.css';
import { useContext, useState } from 'react';
import { ReversedContext } from '../Pages/Chess';
import { Image } from 'react-bootstrap';


function ChessBoard({chessState, move}) {
    const reversed = useContext(ReversedContext);
    const chessBoard = (reversed) ? [...chessState].reverse() : chessState;
    var lightBrown = "rgb(239, 222, 205)";
    var darkBrown = "rgb(159, 129, 112)";
    const [selectedPiece, setSelectedPiece] = useState(null);
    
    const dragOver = (e) => { 
        e.stopPropagation(); // parent wont get dragover
        e.preventDefault(); // by default we cant drop stuff
    }
    
    const drag = (e, data) => {
        e.dataTransfer.setData("text", JSON.stringify(data)); // stringify the data as JSON
    }

    const addSelected = (id) => {
        const squareId = `${id}`;
        setSelectedPiece(squareId);
        console.log(id);
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
                        return (
                        <div onDragOver={dragOver} onDrop={e => move(e, piece)} onClick={() => addSelected(piece.id)} 
                                        style={style} 
                                        className={className}
                                        key={piece.id}
                        >
                            <img className='piece' src={image} alt=''
                            draggable="true" onDragStart={e => drag(e, piece)}/>
                        </div>)
                    })
                ))}
            </div>

            
        </div>
    );
}

export default ChessBoard;