import { Card, ListGroup, Row, Col, CardFooter, Button } from 'react-bootstrap';
import { useAuth } from '../Data/AuthProvider';
import ChessBoardDisplay from './ChessBoardDisplay';
import './chessboard.css';

const MatchHistoryCard = ({ matchHistory }) => {
  const { user } = useAuth();

  return (
    <Card className="move-list mb-4 animate-fade-in">
      <div className="card-header">
        Match History ({matchHistory?.totalMatches || 0})
      </div>
      <div className="card-body move-list-content">
        <ListGroup variant="flush">
          {matchHistory?.matches?.length ? (
            matchHistory.matches.map((game) => {
              const isUserBlack = game.blackPlayer === user;
              const userColor = isUserBlack ? 'Black' : 'White';
              const opponent = isUserBlack ? game.whitePlayer : game.blackPlayer;
              const opponentColor = isUserBlack ? 'White' : 'Black';
              const winner = game.winner;

              return (
                <ListGroup.Item key={game.id} className="d-flex align-items-stretch">
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
              );
            })
          ) : (
            <ListGroup.Item style={{color: "var(--chess-gold-light)"}} className="text-center">
              No matches found.
            </ListGroup.Item>
          )}
        </ListGroup>
      </div>
      <CardFooter><Button>123</Button></CardFooter>
    </Card>
  );
};

export default MatchHistoryCard;
