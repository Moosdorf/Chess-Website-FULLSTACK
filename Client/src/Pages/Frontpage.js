import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { Title } from '../Components/Title';
import { useState } from 'react';
import ChessTypeSelection from '../Components/ChessTypeSelection';
import { GetCookies } from '../Functions/HelperMethods';


function Frontpage() {
    var cookies = GetCookies();
    const [showBotOptions, setShowBotOptions] = useState(false);
    const handleShow = () => setShowBotOptions(true);
    const handleClose = () => {
        setShowBotOptions(false);
    }
    const handleSelection = (playerWhite) => {
        if (cookies.user) navigate("/chess_game", { state: { botGame: true, playerWhite: playerWhite} })
    }


    let navigate = useNavigate();
    return (
        <Container>
            <Row className="justify-content-md-center">
                <Col md="auto">
                    <Title message="Chess"/>
                </Col>
            </Row>
            <Row>
                <Col className='text-center'>
                    <Button onClick={handleShow}>
                        Play BOT Chess
                    </Button>
                </Col>
            </Row>
            <ChessTypeSelection show={showBotOptions} handleClose={handleClose} handleSelection={handleSelection} />

            <Row>
                <Col className='text-center'>
                    <Button onClick={() => navigate("/chess_game", { state: { botGame: false } })}>
                        Play Chess
                    </Button>
                </Col>
            </Row>

            <Row>
                <Col className='text-center'>
                    <Button onClick={() => navigate("/sign_up")}>
                        sign up
                    </Button>
                </Col>
            </Row>

        </Container>)
}
  
export default Frontpage;