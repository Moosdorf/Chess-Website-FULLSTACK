import { Col, Row, Container, Button, Card } from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import ChessBoard from '../Components/ChessBoard.js';
import Piece from '../Data/Piece.js';
import { GetCookies } from '../Functions/HelperMethods.js';
import ActiveChessMoves from '../Components/ActiveChessMoves.js';
const ChessContext = createContext(null);

const PlayerInfo = ({player, classText}) => {
    
    return(
        <Card className={classText}>
            <Card.Header>{player.name}</Card.Header>
        </Card>);
}

const Stats = ({chessBoard}) => {
    return (
            <Card className="game-stats">
            <Card.Header>
                Game ID: {chessBoard.id}
            </Card.Header>
            <Card.Body>
                {chessBoard.checkMate && (
                <Card.Text className="text-danger fw-bold">
                    Checkmate
                </Card.Text>
                )}

                {chessBoard.check && !chessBoard.checkMate && (
                <Card.Text className="text-warning">
                    In Check
                </Card.Text>
                )}

                <Card.Text>
                    Total Moves: {chessBoard.moves}
                </Card.Text>
            </Card.Body>
            </Card>
    )
}

function Chess() {
    var location = useLocation();
    const botGame = location.state?.botGame ?? false;
    const playerWhite = location.state?.playerWhite ?? false;
    var cookies = GetCookies();

    async function createBoard() {

        let body = (!botGame) ? 
                JSON.stringify({
                    "Player1": cookies.user,
                    "Player2": "admin"}) : 
                JSON.stringify({
                    "Player1": cookies.user,
                    "PickedWhite": playerWhite});

        var url = (botGame) ? `http://localhost:5000/api/chess/newbotgame` : `http://localhost:5000/api/chess/new`;

        let res = await fetch(url, {
            method: "POST",
            credentials: 'include',
            headers: {
                "Content-Type": "application/json",
                "Accept": "*/*"

            },
            body: body
        });

        var jsonText = await res.text();
        var parsedJson = await JSON.parse(jsonText);
        handleSetChessBoard(parsedJson);
    }

    const handleSetChessBoard = (apiBoard) => {
        if (chessBoardHistory.length > 0 && chessBoardHistory[chessBoardHistory.length - 1][1] === apiBoard.FEN) return;
        
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
            [apiBoard.LastMove, apiBoard.FEN]
        ]);
        setReversed(cookies.user === apiBoard.Players[0]); // index 0 will always be white
        var fenSplit = apiBoard.FEN.split(" ");
        setChessBoard({board: tempChessBoard, 
            id: apiBoard.Id, 
            isWhitesTurn: apiBoard.IsWhite, 
            moves: parseInt(fenSplit[5]) * 2 + (fenSplit[1] === "b"), 
            check: apiBoard.Check, 
            checkMate: apiBoard.CheckMate, 
            checkBlockers: apiBoard.BlockCheckPositions,
            currentPlayer: apiBoard.CurrentPlayer,
            players: apiBoard.Players,
            botGame: botGame,
            playerWhite: playerWhite,
            lastMove: apiBoard.LastMove});
    }  

    const [chessBoard, setChessBoard] = useState(null);
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 
    const [reversed, setReversed] = useState(false);

    useEffect(() => {
        createBoard();
    }, []);

    useEffect(() => {
        if (botGame && chessBoard?.currentPlayer === "stockfish") {
            movePieceBot();
        }
    }, [chessBoard, botGame]);

    const movePieceBot = async () => {
        await fetch(`http://localhost:5000/api/chess/${chessBoard.id}/moveBot`, {
            method: "PUT",
            credentials: 'include',
            headers: {
                "Content-Type": "application/json",
                "Accept": "*/*"
            }
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
    }

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

    if (!chessBoard) return;
    return ( // give info if board is reveresed or not.
        <ChessContext.Provider value={{reversed, chessBoard, chessBoardHistory, movePiece, movePieceBot}}> 
            <Container className='center'>
                <Row>
                    <Col>
                        <Title message="Chess Game"/>
                    </Col>
                </Row>
                <Row className='g-4'>
                    {/* Column 1 - stats of the game */}
                    <Col md={3}>
                        {chessBoard && <Stats chessBoard={chessBoard}/>}
                    </Col>

                    {/* Column 2 - Chessboard */}
                    <Col md={6}>
                        {chessBoard && <ChessBoard/>}
                    </Col>
                    
                    {/* Column 3 - Player Info */}
                    <Col md={3} className="d-flex flex-column">
                        <div className="flex-grow-1"></div>

                        {/* Player 2 info */}
                        <div className="mb-3">
                            <PlayerInfo
                                classText={chessBoard.currentPlayer !== cookies.user ? "active-card" : ""}
                                player={{ name: chessBoard.players[0] === cookies.user ? chessBoard.players[1] : chessBoard.players[0] }}
                            />
                        </div>

                        <ActiveChessMoves />

                        {/* Player 1 info */}
                        <div className="mt-3">
                            <PlayerInfo
                                classText={chessBoard.currentPlayer === cookies.user ? "active-card" : ""}
                                player={{ name: cookies.user }}
                            />
                        </div>
                        
                        <div className="flex-grow-1"></div>
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