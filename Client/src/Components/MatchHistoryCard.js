import { Card, ListGroup, Row, Col, CardFooter, Button, ButtonGroup } from 'react-bootstrap';
import { useAuth } from '../Data/AuthProvider';
import ChessBoardDisplay from './ChessBoardDisplay';
import { useEffect, useState } from 'react';
import './chessboard.css';
import { Link } from 'react-router-dom';

const MatchHistoryCard = () => {
  const { user } = useAuth();
  const [matchHistory, setMatchHistory] = useState(null);
  const [pageNum, setPageNum] = useState(1);

  useEffect(() => {
      if (!user) return; 
      const fetchData = async () => {
          try {
              const request = new Request(`http://localhost:5000/api/user/match_history/${user}?page=${pageNum}`, {
                  method: "GET",
                  credentials: 'include',
                  headers: {
                      "Content-Type": "application/json",
                  }
              });
              
              const res = await fetch(request);
              const data = await res.json(); 
              console.log(data);
              setMatchHistory(data);
          } catch (e) {
              console.log(e);
          }
      };

      fetchData();
  }, [pageNum]);

  const handleSetPageNum = (page) => {
    setPageNum(page);
  }

  return (
    <Card className="move-list mb-4 animate-fade-in">
      <div className="card-header">
        Match History ({matchHistory?.amountOfGames || 0})
      </div>
      <div className="card-body move-list-content">
        <ListGroup variant="flush">
          {matchHistory?.items?.length ? (
            matchHistory.items.map((game) => {
              const isUserBlack = game.blackPlayer === user;
              const userColor = isUserBlack ? 'Black' : 'White';
              const opponent = isUserBlack ? game.whitePlayer : game.blackPlayer;
              const opponentColor = isUserBlack ? 'White' : 'Black';
              const winner = game.winner;

              return (
                <Link key={game.id} 
                to={{
                  pathname: `/chess_game/${game.id}`
                }}
                className={"remove-underline"}
                >
                  <ListGroup.Item className="d-flex align-items-stretch">
                    <Row className="w-100">
                      <Col xs={3} className="d-flex justify-content-center align-items-center">
                        ID: {game.id}
                      </Col>

                      <Col xs={6} className="d-flex justify-content-center align-items-center">
                        <ChessBoardDisplay FEN={game.fen} color={userColor} />
                      </Col>

                      <Col xs={3}>
                        <div className="d-flex flex-column justify-content-between h-100">
                          <div className={opponentColor === "White" ? "historyWhite text-end mb-1" : "historyBlack text-end mb-1"}>
                            <span>{opponentColor}</span><br/>
                            <strong>{opponent}</strong><br/>
                            {winner === opponent && <strong>Won</strong>}
                          </div>

                          <hr className="chess-divider" />

                          <div className={userColor === "White" ? "historyWhite text-end" : "historyBlack text-end"}>
                            {winner === user && <strong>Won</strong>} <br/>
                            <span>{userColor}</span> <br/>
                            <strong>{user} (You)</strong>
                          </div>
                        </div>
                      </Col>
                    </Row>
                  </ListGroup.Item>
                </Link>
              );
            })
          ) : (
            <ListGroup.Item style={{color: "var(--chess-gold-light)"}} className="text-center">
              No matches found.
            </ListGroup.Item>
          )}
        </ListGroup>
      </div>
      <CardFooter>
        <ButtonGroup>
          <Button disabled={matchHistory && !matchHistory.hasPrevious} onClick={() => handleSetPageNum(pageNum-1)}>{"<-"}</Button>
          <Button disabled={matchHistory && !matchHistory.hasNext} onClick={() => handleSetPageNum(pageNum+1)}>{"->"}</Button>
        </ButtonGroup>
      </CardFooter>
    </Card>
  );
};

export default MatchHistoryCard;
