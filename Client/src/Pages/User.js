import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button, CardHeader, Card, CardBody} from 'react-bootstrap';
import { GetCookies } from '../Functions/HelperMethods'
import { useEffect } from 'react';
import { useAuth } from '../Data/AuthProvider.js';

function Userpage() {
    const { user } = useAuth();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const request = new Request(`http://localhost:5000/api/user/${user}/match_history`, {
                    method: "GET",
                    credentials: 'include',
                    headers: {
                        "Content-Type": "application/json",
                    }
                });

                const res = await fetch(request);
                const data = await res.json(); 
                console.log(data);
            } catch (e) {
                console.log(e);
            }
        };

        fetchData();
    }, [user]);
    
    if (!user) return (<div>no user</div>)



    return (
        <Container>
            <Row>
                <Col>
                    {user && <h1>User: {user}</h1>}
                </Col>
            </Row>
            <Row style={{border: "1px solid green"}}>
                <Col style={{border: "1px solid red"}}>
                <Card>
                    <CardHeader>
                        Match history
                    </CardHeader>
                    <CardBody>
                        123
                    </CardBody>
                </Card>
                </Col>
                <Col style={{border: "1px solid red"}}>
                    <Card>
                        <CardHeader>
                            Puzzle history
                        </CardHeader>
                        <CardBody>
                            123
                        </CardBody>
                    </Card>
                </Col>
                <Col style={{border: "1px solid red"}}>
                    <Card>
                        <CardHeader>
                            Learning history
                        </CardHeader>
                        <CardBody>
                            123
                        </CardBody>
                    </Card>
                </Col>
            </Row>

            <br/>
            
            <Row>
                <Card>
                    <CardHeader>STATS</CardHeader>
                    <CardBody>123</CardBody>
                </Card>
            </Row>

        </Container>)
}
  
export default Userpage;