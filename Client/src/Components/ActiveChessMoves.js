import { Button, ButtonGroup, Card, CardBody, ListGroup } from "react-bootstrap";
import { ChessContext } from '../Pages/Chess';
import { useContext, useEffect, useRef } from "react";

function ActiveChessMoves() {
    const { chessState, chessBoardHistory } = useContext(ChessContext);
    
    const listEndRef = useRef(null);

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
                        </div>
                    </CardBody>
                <ButtonGroup>
                    <Button className='move-nav-btn'>{"<"}</Button>
                    <Button className='move-nav-btn'>{"!!"}</Button>
                    <Button className='move-nav-btn'>{">"}</Button>
                </ButtonGroup>
                <ButtonGroup>
                    <Button className='move-nav-btn'>{"Abort"}</Button>
                    <Button className='move-nav-btn'>{"Draw"}</Button>
                    <Button className='move-nav-btn'>{"Forfeit"}</Button>
                </ButtonGroup>
            </Card>)

}

export default ActiveChessMoves;