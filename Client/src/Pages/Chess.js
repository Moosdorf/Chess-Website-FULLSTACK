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
    var parsedJson = await JSON.parse(jsonText);
    console.log(parsedJson);
    var chessBoard = parsedJson.game;
    var chessId = parsedJson.id;
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

    return {chessBoard, chessId};
}


const ChessContext = createContext(null);
function Chess() {
    const [chessBoard, setChessBoard] = useState(null);
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 
    const [reversed, setReversed] = useState(false);
    
    useEffect(() => {
        createBoard().then((board) => {
            console.log(board);
            setChessBoard(board);
            setChessBoardHistory([[board.chessBoard]]);
        })
    }, []);

    
    var black = chessBoardHistory.length % 2 === 0;
    var turn = (black) ? "black" : "white";
    
    return ( // give info if board is reveresed or not.
        <ChessContext value={{reversed, chessBoard}}> 
            <Container className='center'>
                <Row>
                    <Col>
                        <Title message="Chess Game"/>
                <p>{(black) ? "black" : "white"}</p>

                    </Col>
                </Row>
                {chessBoard && <ChessBoard key={chessBoard.id} turnColor={turn}/>}

                <br/>
                {chessBoard && <Button variant='secondary' onClick={() => {
                    setReversed(c => !c);
                }}> 
                    Reverse 
                </Button>}
                
            </Container>
        </ChessContext>)
}
  
export default Chess;
export { ChessContext }; // export context, can be imported from children