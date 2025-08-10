import { Button, ButtonGroup, Card, CardBody, ListGroup } from "react-bootstrap";
import { ChessContext } from '../Pages/Chess';
import { useContext, useEffect, useRef, useState } from "react";
import { useSignalRGame } from "../SignalR/SingalRGameProvider";
import { useAuth } from "../Data/AuthProvider";


function ActiveChessMoves() {
    const { forfeitGame, sendDrawResponse, sendDrawRequest } = useSignalRGame();
    const { user } = useAuth();
    const { chessState, chessBoardHistory } = useContext(ChessContext);
    const [showConfirm, setShowConfirm] = useState({
                                                warning: false
                                            });
    const listEndRef = useRef(null);

    useEffect(() => {
        if (chessState.drawRequest && chessState.drawRequest !== user) {
            setShowConfirm({
                            warning: true,
                            message: "Accept draw?",
                            type: "request"                                                                                   
                            })
        }
    }, [chessState])

    useEffect(() => {
        if (listEndRef.current) {
            listEndRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [chessBoardHistory]);

    return (
            <Card className="move-list mb-1">
                <CardBody style={{ padding: 10 }}>
                        <div className="move-list-content">
                            <ListGroup variant="flush">
                                {chessBoardHistory.map((move, i) =>
                                    move.move && (
                                        <ListGroup.Item
                                            className={chessState.moves === i ? 'active' : ''}
                                            key={move.move}
                                        >
                                            {i}. {move.move}
                                        </ListGroup.Item>
                                    )
                                )}
                                <div ref={listEndRef} />
                            </ListGroup>
                        {showConfirm.warning && <div className="text-center">{showConfirm.message}</div>}
                        </div>
                    
                    </CardBody>
                <ButtonGroup>
                    <Button className='move-nav-btn'>{"<"}</Button>
                    <Button className='move-nav-btn'>{"!!"}</Button>
                    <Button className='move-nav-btn'>{">"}</Button>
                </ButtonGroup>
                {!showConfirm.warning && <ButtonGroup>
                            
                    <Button disabled={chessState.gameDone} className='move-nav-btn' 
                        onClick={() => setShowConfirm({
                            warning: true,
                            message: "Abort????"                                                                                   
                    })}>
                        Abort
                    </Button>
                    <Button disabled={chessState.gameDone} className='move-nav-btn' 
                        onClick={() => setShowConfirm({
                            warning: true,
                            message: "Confirm request of sending draw",
                            type: "draw"                                                                         
                    })}>
                        Draw
                    </Button>
                    <Button disabled={chessState.gameDone} className="move-nav-btn"
                        onClick={() => setShowConfirm({
                            warning: true,
                            message: "Do you want to forfeit?",
                            type: "forfeit"                                                                                 
                    })}>
                        Forfeit
                    </Button>

                </ButtonGroup>}
                {showConfirm.warning && (
                    <ButtonGroup>
                            <Button
                            size="sm"
                            onClick={() => {
                                if (showConfirm.type === "request") {sendDrawResponse(chessState.sessionId, true)}
                                else showConfirm.type === "forfeit" ? forfeitGame(chessState.sessionId) : sendDrawRequest(chessState.sessionId);
                                setShowConfirm(false);
                            }}
                            >
                            Yes
                            </Button>
                            <Button
                                size="sm"
                                className="ms-2"
                                onClick={() => {
                                    if (showConfirm.type === "request") {sendDrawResponse(chessState.sessionId, false)}
                                    setShowConfirm(false)
                                }}
                            >
                            No
                            </Button>
                    </ButtonGroup>
                )}
            </Card>)

}

export default ActiveChessMoves;