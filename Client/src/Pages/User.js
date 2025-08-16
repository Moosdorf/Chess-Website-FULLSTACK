import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button, CardHeader, Card, CardBody } from 'react-bootstrap';
import { useAuth } from '../Data/AuthProvider.js';
import MatchHistoryCard from '../Components/MatchHistoryCard.js';

function Userpage() {
    const { user } = useAuth();

    
    if (!user) return (<div>no user</div>)



    return (
        <Container>
            <Row>
                <Col>
                    {user && <h1>User: {user}</h1>}
                </Col>
            </Row>

            <Row>
                <Col>
                    <Card>
                        <CardHeader>STATS</CardHeader>
                        <CardBody>123</CardBody>
                    </Card>
                </Col>
            </Row>
            
            <br/>

            <Row>
                <Col>
                    <MatchHistoryCard/>
                </Col>
                <Col>
                    <Card></Card>
                </Col>
            </Row>

            

        </Container>)
}
  
export default Userpage;