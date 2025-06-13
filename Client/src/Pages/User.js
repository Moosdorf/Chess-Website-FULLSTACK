import 'bootstrap/dist/css/bootstrap.css';
import { Col, Row, Container, Button} from 'react-bootstrap';
import { GetCookies } from '../Functions/HelperMethods'

function Userpage() {

    var cookies = GetCookies();
    console.log(cookies);
    const test = async () => {
        var cookies = GetCookies();
        console.log(cookies);
        const request = new Request(`http://localhost:5000/api/user/test`, {
            method: "PUT",
            credentials: 'include',
            headers: {
                "Content-Type": "application/json", // Correct Content-Type for JSON
              },
        });

        await fetch(request)
        .then(res => {
            console.log(res)
            return res.text()
        })
        .then(data => {
            console.log(data);
            return JSON.parse(data)
        })
        .then(results => {
            console.log(results);
        })
        .catch(e => console.log(e));
    }

    return (
        <Container>
            <Row>
                {cookies.user && <h1>User: {cookies.user}</h1>}
            </Row>
            <Row>
                <Col>
                    gg
                    <Button onClick={test}>
                        test
                    </Button>
                </Col>
            </Row>

        </Container>)
}
  
export default Userpage;