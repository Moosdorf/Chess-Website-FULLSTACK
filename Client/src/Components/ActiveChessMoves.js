import { Button, ButtonGroup, Card, CardBody, ListGroup } from "react-bootstrap";
import { ChessContext } from '../Pages/Chess';
import { useContext, useEffect, useRef, useState } from "react";
import { useSignalRGame } from "../SignalR/SingalRGameProvider";
import { useAuth } from "../Data/AuthProvider";


function ActiveChessMoves() {
    const { forfeitGame, sendDrawResponse, sendDrawRequest } = useSignalRGame();
    const { user } = useAuth();
    const { chessState, chessBoardHistory, activeMoveIndex, setActiveMoveIndex } = useContext(ChessContext);

    const [showConfirm, setShowConfirm] = useState({ warning: false });

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
        setActiveMoveIndex("current");
    }, [chessBoardHistory, setActiveMoveIndex]);

    useEffect(() => {
        if (listEndRef.current) {
            listEndRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [activeMoveIndex]);

    const handleSetHistory = (increment) => {
        let currentIndex = (activeMoveIndex === "current") ? chessBoardHistory.length-1 : activeMoveIndex;
        switch(increment) {
            case -1:
                if (currentIndex - 1 >= 0) setActiveMoveIndex(currentIndex - 1);
                break; 
            case 1:
                if (currentIndex + 1 < chessBoardHistory.length) setActiveMoveIndex(currentIndex + 1);
                if (currentIndex + 1 === chessBoardHistory.length-1) setActiveMoveIndex("current");
                break;
            case 0:
                setActiveMoveIndex("current");
                break;
        }
    }
    
    let currentIndex = (activeMoveIndex === "current") ? chessBoardHistory.length-1 : activeMoveIndex;

    const handleSetActiveMoveIndex = (i) => {
        chessBoardHistory.length-1 === i ? setActiveMoveIndex("current") : setActiveMoveIndex(i);
    }

    return (
            <Card className="move-list">
                <CardBody style={{ padding: 15 }}>
                        <div className="move-list-content">
                            <ListGroup variant="flush">
                                {chessBoardHistory.map((move, i) =>
                                        <ListGroup.Item
                                            className={currentIndex === i ? 'active' : ''}
                                            ref={currentIndex === i ? listEndRef : null}
                                            key={i}
                                            onClick={() => handleSetActiveMoveIndex(i)}
                                        >
                                            {i}. {move.move}
                                        </ListGroup.Item>
                                    
                                )}
                            </ListGroup>
                        {showConfirm.warning && <div className="text-center">{showConfirm.message}</div>}
                        </div>
                    </CardBody>
                <ButtonGroup>
                    <Button disabled={0 === currentIndex} onClick={() => handleSetHistory(-1)} className='move-nav-btn'>{"<"}</Button>
                    <Button disabled={chessBoardHistory.length-1 === currentIndex} onClick={() => handleSetHistory(0)} className='move-nav-btn'>{"!!"}</Button>
                    <Button disabled={chessBoardHistory.length-1 === currentIndex} onClick={() => handleSetHistory(1)} className='move-nav-btn'>{">"}</Button>
                </ButtonGroup>
                {!showConfirm.warning && <ButtonGroup>

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