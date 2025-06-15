import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useContext, useEffect, useState } from 'react';
import ChessBoard from '../Components/ChessBoard.js';
import Piece from '../Data/Piece.js';

async function createBoard() {
    let res = await fetch(`http://localhost:5000/api/chess/new`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "*/*"

        },
        body: JSON.stringify({
            "player1": 0,
            "player2": 1
        })
    });

    var jsonText = await res.text();
    var chessBoard = await JSON.parse(jsonText);

    chessBoard = chessBoard.map(row => (
        row.map(piece => new Piece(
            piece.Type, 
            piece.IsWhite, 
            piece.Position, 
            piece.Moves,
            piece.AvailableMoves,
            piece.Defenders, 
            piece.Attackers,
            piece.IsAlive))
    ));

    console.log(chessBoard);
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
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 
    const [reversed, setReversed] = useState(false);
    
    useEffect(() => {
        createBoard().then(board => {
            setChessBoard(board);
            setChessBoardHistory([[board]]);
        })
    }, []);

    
    var black = chessBoardHistory.length % 2 === 0;
    var turn = (black) ? "black" : "white";
    return ( // give info if board is reveresed or not.
        <ReversedContext.Provider value={reversed}> 
            <Container className='center'>
                <Row>
                    <Col>
                        <Title message="Chess Game"/>
                <p>{(black) ? "black" : "white"}</p>

                    </Col>
                </Row>
                {chessBoard && <ChessBoard key={chessBoard} chessState={chessBoard} turnColor={turn}/>}

                <br/>
                {chessBoard && <Button variant='secondary' onClick={() => {
                    setReversed(c => !c);
                }}> 
                    Reverse 
                </Button>}
                
            </Container>
        </ReversedContext.Provider>)
}
  
export default Chess;
export { ReversedContext }; // export context, can be imported from children