import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { Title } from '../Components/Title';

function Frontpage() {
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
                    <Button variant='primary' onClick={() => navigate("/chess_game")}>
                        Play Chess
                    </Button>
                </Col>
            </Row>

            <Row>
                <Col className='text-center'>
                    <Button variant='primary' onClick={() => navigate("/sign_up")}>
                        sign up
                    </Button>
                </Col>
            </Row>

        </Container>)
}
  
export default Frontpage;