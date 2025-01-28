import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useContext, useEffect, useState } from 'react';
import ChessBoard from '../Components/ChessBoard.js';
import Piece from '../Data/Piece.js';

async function createBoard() {
    let res = await fetch(`http://localhost:5121/api/chess/new`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "*/*"

        },
        body: JSON.stringify({
            "id": 1,
            "player1": 0,
            "player2": 1
        })
    });

    var jsonText = await res.text();
    var chessBoard = await JSON.parse(jsonText);
    chessBoard = chessBoard.map(row => (
        row.map(piece => new Piece(piece.piece, piece.color, piece.id))
    ));
    return chessBoard;
}


const findPosition = (chessState, piece) => {
    for (let row = 0; row < chessState.length; row++) {
        var chessRow = chessState[row];
        for (let col = 0; col < chessRow.length; col++) {
            if (chessState[row][col].id === piece.id) {
                return {row: row, col: col};
            }
        }
    }
}

const convertPosition = (row, col) => {
    let file = row + 1;
    let rank = String.fromCharCode(col + 65);
    return file + rank;
}

const ReversedContext = createContext(null);
function Chess() {
    const [chessBoard, setChessBoard] = useState(null);
    const [reversed, setReversed] = useState(false);
    
    useEffect(() => {
        createBoard().then(board => setChessBoard(board))
    }, []);

    const move = (e, target) => {
        e.preventDefault();
        var data = JSON.parse(e.dataTransfer.getData("text")); // decode JSON

        var attackerPosition = findPosition(chessBoard, data);
        var victimPosition = findPosition(chessBoard, target);

        const request = new Request(`http://localhost:5121/api/chess/${1}/move`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json", // Correct Content-Type for JSON
              },
            body: JSON.stringify({chessState: JSON.stringify(chessBoard), 
                                  move: JSON.stringify({attacker: attackerPosition, victim: victimPosition})})
        });
        
        fetch(request)
        .then(res => res.text())
        .then(data => JSON.parse(data))
        .then(results => {
            let chessBoard = results.map(row => (
                row.map(piece => new Piece(piece.piece, piece.color, piece.id))
            ));
            setChessBoard(chessBoard);
        })
        .catch(e => console.log(e));
    }
    

    return ( // give info if board is reveresed or not.
        <ReversedContext.Provider value={reversed}> 
            <Container className='center'>
                <Row>
                    <Col>
                        <Title message="Chess Game"/>
                    </Col>
                </Row>
                {chessBoard && <ChessBoard chessState={chessBoard} move={move}/>}

                
                <br/>
                {chessBoard && <Button onClick={() => {
                    setReversed(c => !c);
                }}> 
                    Reverse 
                </Button>}
                
            </Container>
        </ReversedContext.Provider>)
}
  
export default Chess;
export { ReversedContext }; // export context, can be imported from children