import { useForm, SubmitHandler, SubmitErrorHandler } from "react-hook-form";
import { Col, Row, Container, Button, Form } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import { useState } from "react";
import { useAuth } from "../Data/AuthProvider";

function SignUp() {
    const { user, signin } = useAuth();
    const { register, handleSubmit, setError, formState: { errors } } = useForm();
    var navigate = useNavigate();

    const onSubmit = async data => {
        console.log("submitted ", data);

        const request = new Request(`http://localhost:5000/api/user/new`, {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json", // Correct Content-Type for JSON
              },
            body: JSON.stringify({username: data.username, 
                                  password: data.password})
        });

        await fetch(request)
        .then(res => res.text())
        .then(data => JSON.parse(data))
        .then(results => {
            if (results.newUser) {
                document.cookie = `user=${results.newUser.username};max-age=604800;domain=localhost;`;
                signin();
                navigate("/user");
            } else {
                setError("username", {
                    type: "manual",
                    message: "Username is already in use"
                });
            }
        })
        .catch(e => console.log(e));
    };

    return (
        <Container className="text-center">
            <Row >
                <Col>
                    <h1>Sign up</h1>
                </Col>
            </Row>
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
                                {...register("username", { required: "Username is required" })}
                                isInvalid={!!errors.username}/>
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
                             {...register("password", { required: "Password is required" })}
                             isInvalid={!!errors.password}/>
                            <Form.Control.Feedback type="invalid">
                                {errors.password?.message}
                            </Form.Control.Feedback>
                        </Form.Group>
                        <Button className='text-center' type="submit">
                            Create Account
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    )
} 
export default SignUp;
