import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useContext, useEffect, useState } from 'react';
import ChessBoard from '../Components/ChessBoard.js';
import Piece from '../Data/Piece.js';
const ChessContext = createContext(null);
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
        console.log("parsed json from create: ", parsedJson)
        handleSetChessBoard(parsedJson);
    }

    const handleSetChessBoard = (apiBoard) => {
        console.log(apiBoard);
        var tempChessBoard = apiBoard.Chessboard;
        tempChessBoard = tempChessBoard.map(row => (
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
        setChessBoard({board: tempChessBoard, id: apiBoard.Id, isWhitesTurn: apiBoard.IsWhite, moves: apiBoard.Moves});
    }  

    const [chessBoard, setChessBoard] = useState(null);
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 
    const [reversed, setReversed] = useState(false);
    useEffect(() => {
        createBoard();
    }, []);

    

    const movePiece = async (from, to) => {
        if (!from.AvailableMoves.includes(to.Position)) return;
        let res = await fetch(`http://localhost:5000/api/chess/${chessBoard.id}/move`, {
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
            console.log("response", res)
            return res.text()
        })
        .then(data => {
            return JSON.parse(data)
        })
        .then(results => {
            console.log("parsed json from move", results);
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
                {chessBoard && <ChessBoard/>}

                <br/>
                {chessBoard && <Button variant='secondary' onClick={() => {
                    setReversed(c => !c);
                }}> 
                    Reverse 
                </Button>}
                
            </Container>
        </ChessContext.Provider>)
}
  
export default Chess;
export { ChessContext }; // export context, can be imported from children