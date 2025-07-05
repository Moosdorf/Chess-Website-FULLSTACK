import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button, Card} from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useEffect, useState } from 'react';
import ChessBoard from '../Components/ChessBoard.js';
import Piece from '../Data/Piece.js';
const ChessContext = createContext(null);

const Stats = ({chessBoard}) => {
    return (
        <Card className="customCard">
        <Card.Body>
            <Card.Header className="customCardHeader">Game Stats (ID: {chessBoard.id})</Card.Header>

            <Card.Text><strong>Turn:</strong> {chessBoard.isWhitesTurn ? 'White' : 'Black'}</Card.Text>

            {chessBoard.checkMate && (
            <Card.Text className="text-danger fw-bold">Checkmate</Card.Text>
            )}

            {chessBoard.check && (
            <>
                <Card.Text className="text-warning">In Check</Card.Text>
                <Card.Text><strong>Block Options:</strong> {chessBoard.checkBlockers.join(', ')}</Card.Text>
            </>
            )}

            <Card.Text><strong>Total Moves:</strong> {chessBoard.moves}</Card.Text>
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
                piece.Pinned,
                piece.Moves,
                piece.Captures,
                piece.AvailableMoves,
                piece.AvailableCaptures,
                piece.Defenders, 
                piece.Attackers,
                piece.IsAlive))
        ));
        setChessBoardHistory([
            ...chessBoardHistory,
            apiBoard.FEN
        ]);
        setChessBoard({board: tempChessBoard, 
            id: apiBoard.Id, 
            isWhitesTurn: apiBoard.IsWhite, 
            moves: apiBoard.Moves, 
            check: apiBoard.Check, 
            checkMate: apiBoard.CheckMate, 
            checkBlockers: apiBoard.BlockCheckPositions});
    }  

    const [chessBoard, setChessBoard] = useState(null);
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 
    const [reversed, setReversed] = useState(false);
    useEffect(() => {
        createBoard();
    }, []);

    

    const movePiece = async (from, to, promotion) => {
        if (!from.AvailableMoves.includes(to.Position) && !from.AvailableCaptures.includes(to.Position) ) return;


        await fetch(`http://localhost:5000/api/chess/${chessBoard.id}/move`, {
            method: "PUT",
            credentials: 'include',
            headers: {
                "Content-Type": "application/json",
                "Accept": "*/*"

            },
            body: JSON.stringify({
                Move: `${from.Position},${to.Position}`,
                Promotion: promotion
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
                    <Col xs={3}>
                        {chessBoard && <Stats chessBoard={chessBoard}/>}

                    </Col>
                    <Col xs={6}>
                        {chessBoard && <ChessBoard/>}
                    </Col>
                    <Col xs={3}>

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