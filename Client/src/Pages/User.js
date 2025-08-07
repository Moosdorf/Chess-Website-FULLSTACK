import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button, CardHeader, Card, CardBody } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { useAuth } from '../Data/AuthProvider.js';
import MatchHistoryCard from '../Components/MatchHistoryCard.js';

function Userpage() {
    const { user } = useAuth();
    const [matchHistory, setMatchHistory] = useState(null);

    useEffect(() => {
        if (!user) return; 
        const fetchData = async () => {
            try {
                const request = new Request(`http://localhost:5000/api/user/match_history/${user}`, {
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
    }, [user]);
    
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
                    <MatchHistoryCard matchHistory={matchHistory} />
                </Col>
                <Col>
                    <Card></Card>
                </Col>
            </Row>

            

        </Container>)
}
  
export default Userpage;