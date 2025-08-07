import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { Title } from '../Components/Title';
import { useSignalRGame } from '../SignalR/SingalRGameProvider';
import { useAuth } from '../Data/AuthProvider';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ChessTypeSelection from '../Components/ChessTypeSelection';


function Frontpage() {
  const { joinGame, joinBotGame } = useSignalRGame(); 
  const [showBotOptions, setShowBotOptions] = useState(false);

  const handleShow = () => setShowBotOptions(true);
  
  const handleClose = () => {
      setShowBotOptions(false);
  }
  
  const handleSelection = (playerWhite) => {
    joinBotGame(playerWhite);
    handleClose(true);
  }

    return (
        <Container>
            <Row className="justify-content-md-center">
                <Col md="auto">
                    <Title message="Chess"/>
                </Col>
            </Row>
            <Row className="justify-content-md-center">
                <Col md="auto">
                    <Button onClick={() => joinGame()}>Queue Multiplayer</Button>
                </Col>
                <Col md="auto">
                    <Button onClick={() => handleShow()}>Queue Stockfish</Button>
                </Col>
            </Row>
            <ChessTypeSelection show={showBotOptions} handleClose={handleClose} handleSelection={handleSelection} />

        </Container>)
}
  
export default Frontpage;