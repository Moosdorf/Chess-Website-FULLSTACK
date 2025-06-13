import { useForm, SubmitHandler, SubmitErrorHandler } from "react-hook-form";
import { Col, Row, Container, Button, Form } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { Title } from "../Components/Title";
import { useAuth } from "../Data/AuthProvider";

function SignIn() {
    const { user, signin } = useAuth();
    const { register, handleSubmit, setError, formState: { errors } } = useForm();
    var navigate = useNavigate();
    const onSubmit = async data => {

        const request = new Request(`http://localhost:5000/api/user/sign_in`, {
            method: "PUT",
            credentials: "include",
            headers: {
                "Content-Type": "application/json", 
              },
            body: JSON.stringify({username: data.username, 
                                  password: data.password})
        });

        await fetch(request)
        .then(res => res.text())
        .then(data => JSON.parse(data))
        .then(results => {
            if (results.userSignedIn) {
                document.cookie = `user=${results.userSignedIn.username};max-age=604800;domain=localhost;`;
                signin(); 
                navigate("/user");
            } else {
                setError("root", {
                    type: "manual",
                    message: "Username or password is incorrect"
                });
            }
        })
        .catch(e => console.log(e));
    };

    return (
        <Container className="text-center">
            <Row >
                <Col>
                    <h1>Sign in</h1>
                </Col>
            </Row>
            <Row >
                <Col className='mx-auto' md={6} >
                    <Form onSubmit={handleSubmit(onSubmit)}> 
                        {errors.root && (
                            <div className="text-danger mb-3">{errors.root.message}</div>
                        )}
                        <Form.Group className="mb-3">
                            <Form.Label>
                                Username
                            </Form.Label>
                            <Form.Control 
                            type="text" 
                            placeholder="Enter username"
                             {...register("username", { required: "Username is required"})}
                            isInvalid={!!errors.username} />
                            <Form.Control.Feedback type="invalid">
                                {errors.username?.message}
                            </Form.Control.Feedback>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>
                                Password
                            </Form.Label>
                            <Form.Control type="password"
                             placeholder="Enter password"
                             {...register("password", { required: "Password is required"})}
                              isInvalid={!!errors.password}
                             />
                            <Form.Control.Feedback type="invalid">
                                {errors.password?.message}
                            </Form.Control.Feedback>
                        </Form.Group>

                        <Button className='text-center' variant="secondary" type="submit">
                            Sign In
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    )
} 
export default SignIn;