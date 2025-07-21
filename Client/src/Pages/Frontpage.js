import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { Title } from '../Components/Title';


function Frontpage() {

    return (
        <Container>
            <Row className="justify-content-md-center">
                <Col md="auto">
                    <Title message="Chess"/>
                </Col>
            </Row>
        </Container>)
}
  
export default Frontpage;