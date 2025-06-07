import { useForm, SubmitHandler, SubmitErrorHandler } from "react-hook-form";
import { Col, Row, Container, Button, Form } from 'react-bootstrap';

function SignUp() {
    const { register, handleSubmit } = useForm();   



    const onSubmit = async data => {
        console.log("submitted ", data);

        const request = new Request(`http://localhost:5000/api/user/new`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json", // Correct Content-Type for JSON
              },
            body: JSON.stringify({username: data.username, 
                                  password: data.password})
        });

        fetch(request)
        .then(res => res.text())
        .then(data => JSON.parse(data))
        .then(results => {
            console.log(results)
        })
        .catch(e => console.log(e));
    };

    return (
        <Container>
            <Row>
                <Col className='mx-auto' md={6} >
                    <Form onSubmit={handleSubmit(onSubmit)}> 
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Username
                            </Form.Label>
                            <Form.Control 
                            type="text" 
                            placeholder="Enter username"
                            {...register("username")} />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>
                                Password
                            </Form.Label>
                            <Form.Control type="password"
                             placeholder="Enter password"
                             {...register("password")}/>
                        </Form.Group>

                        <Button className='text-center' variant="primary" type="submit">
                            Create Account
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    )
} 
export default SignUp;
