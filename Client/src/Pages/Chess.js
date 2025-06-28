import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button, Card} from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useEffect, useState } from 'react';
import ChessBoard from '../Components/ChessBoard.js';
import Piece from '../Data/Piece.js';
const ChessContext = createContext(null);

const Stats = ({chessBoard}) => {
    return (
            <Card className="p-3 shadow-sm" variant="flush">
                <Card.Body>
                    <Card.Title>Game Stats for id: {chessBoard.id}</Card.Title>
                    <Card.Text>Turn: {(chessBoard && chessBoard.isWhitesTurn) ? "white" : "black"}</Card.Text>
                    <Card.Text>Moves: {chessBoard.moves}</Card.Text>
                    
                    {chessBoard.check && <Card.Text>In check</Card.Text>}
                    {chessBoard.check && <Card.Text>Blocks: {chessBoard.checkBlockers}</Card.Text>}
                </Card.Body>
            </Card>
    )
}

function Chess() {

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
        handleSetChessBoard(parsedJson);
    }

    const handleSetChessBoard = (apiBoard) => {
        console.log("update chessboard ", apiBoard);
        var tempChessBoard = apiBoard.Chessboard;
        tempChessBoard = tempChessBoard.map(row => (
            row.map(piece => new Piece(
                piece.Type, 
                piece.IsWhite, 
                piece.Position, 
                piece.Moves,
                piece.Captures,
                piece.AvailableMoves,
                piece.AvailableCaptures,
                piece.Defenders, 
                piece.Attackers,
                piece.IsAlive))
        ));
        setChessBoard({board: tempChessBoard, id: apiBoard.Id, isWhitesTurn: apiBoard.IsWhite, moves: apiBoard.Moves, check: apiBoard.Check, checkBlockers: apiBoard.BlockCheckPositions});
    }  

    const [chessBoard, setChessBoard] = useState(null);
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 
    const [reversed, setReversed] = useState(false);
    useEffect(() => {
        createBoard();
    }, []);

    

    const movePiece = async (from, to) => {
        if (!from.AvailableMoves.includes(to.Position) && !from.AvailableCaptures.includes(to.Position) ) return;

        await fetch(`http://localhost:5000/api/chess/${chessBoard.id}/move`, {
            method: "PUT",
            credentials: 'include',
            headers: {
                "Content-Type": "application/json",
                "Accept": "*/*"

            },
            body: JSON.stringify({
                Move: `${from.Position},${to.Position}`
            })
        }).then(res => {
            return res.text()
        })
        .then(data => {
            return JSON.parse(data)
        })
        .then(results => {
            handleSetChessBoard(results);
        })
        .catch(e => console.log(e));    
    };


    return ( // give info if board is reveresed or not.
        <ChessContext.Provider value={{reversed, chessBoard, movePiece}}> 
            <Container className='center'>
                <Row>
                    <Col>
                        <Title message="Chess Game"/>
                        <p>{(chessBoard && chessBoard.isWhitesTurn) ? "white" : "black"}</p>
                    </Col>
                </Row>
                <Row>
                    <Col md="auto">
                        {chessBoard && <Stats chessBoard={chessBoard}/>}

                    </Col>
                    <Col>
                        {chessBoard && <ChessBoard/>}
                    </Col>

                </Row>
                <Row>
                    <Col>
                        {chessBoard && <Button variant='secondary' onClick={() => {
                            setReversed(c => !c);
                        }}> 
                            Reverse 
                        </Button>}
                    </Col>
                </Row>
                
            </Container>
        </ChessContext.Provider>)
}
  
export default Chess;
export { ChessContext }; // export context, can be imported from children