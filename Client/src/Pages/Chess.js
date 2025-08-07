import { Col, Row, Container, Button, Card } from 'react-bootstrap';
import { Title } from '../Components/Title';
import { createContext, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import ChessBoard from '../Components/ChessBoard.js';
import ActiveChessMoves from '../Components/ActiveChessMoves.js';
import Chat from '../Components/Chat.js';
import { useAuth } from '../Data/AuthProvider.js';
import { useSignalRGame } from '../SignalR/SingalRGameProvider.js';



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
    const { user } = useAuth();
    const { leaveGame, chessState, sendMove } = useSignalRGame();
    
    var location = useLocation();

    const botGame = location.state?.botGame ?? false;

    const [reversed, setReversed] = useState(false);
    const [chessBoardHistory, setChessBoardHistory] = useState([]); 


    useEffect(() => {
        return () => {
            console.log("leave the game");
            leaveGame(chessState.sessionId); 
        };
    }, []);

    useEffect(() => {
        if (chessState == null) return;
        console.log("updating history" , chessBoardHistory);
        console.log(chessState);
        setReversed(chessState.playerWhite === user);
        setChessBoardHistory(prevHistory => [
            ...prevHistory, 
            { move: chessState.lastMove, fen: chessState.fen }
        ]);
    }, [user, chessState]);



    useEffect(() => {
        if (botGame && chessState?.currentPlayer === "stockfish") {
            movePieceBot();
        }
    }, [chessState, botGame]);

    const movePieceBot = async () => {
       
    }

    const movePiece = async (from, to, promotion) => {
        var move = {
            Move: `${from.Position},${to.Position}`,
            Promotion: promotion
        }
        sendMove(chessState.id, chessState.sessionId, move);
    };

    if (!chessState) return (<div>no chess</div>);
    return ( // give info if board is reveresed or not.
        <ChessContext.Provider value={{reversed, chessState, chessBoardHistory, movePiece, movePieceBot}}> 
            <Container className=''>
                <Row>
                    <Col>
                        <Title message="Chess Game"/>
                    </Col>
                </Row>
                <Row className='g-4'>
                    {/* Column 1 - stats of the game */}
                    <Col md={3}>
                        {chessState && <Stats chessBoard={chessState}/>}
                        {chessState && <Chat sessionId={chessState.sessionId}/>}
                        
                    </Col>

                    {/* Column 2 - Chessboard */}
                    <Col md={6}>
                        {chessState && <ChessBoard/>}
                    </Col>
                    
                    {/* Column 3 - Player Info */}
                    <Col md={3} className="d-flex flex-column">
                        <div className="flex-grow-1"></div>

                        {/* Player 2 info */}
                        <div className="mb-3">
                            <PlayerInfo
                                classText={chessState.currentPlayer !== user ? "active-card" : ""}
                                player={{ name: chessState.players[0] === user ? chessState.players[1] : chessState.players[0] }}
                            />
                        </div>

                        <ActiveChessMoves />

                        {/* Player 1 info */}
                        <div className="mt-3">
                            <PlayerInfo
                                classText={chessState.currentPlayer === user ? "active-card" : ""}
                                player={{ name: user }}
                            />
                        </div>
                        
                        <div className="flex-grow-1"></div>
                    </Col>

                </Row>
                <Row>
                    <Col>
                        {chessState && <Button variant='secondary' onClick={() => {
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