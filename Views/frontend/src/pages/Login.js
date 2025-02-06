import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; 
import { Form, Button, Container, Row, Col, Alert } from "react-bootstrap";

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");
    const navigate = useNavigate(); 


    const handleSubmit = (e) => {
        e.preventDefault();

        if (email === "test@example.com" && password === "password123") {
            setMessage({ type: "success", text: "Login successful!" });
            setTimeout(() => {
                navigate("/dashboard"); 
              }, 2000);
        } else {
            setMessage({ type: "danger", text: "Invalid email or password." });
        }
    };

    return (
        <Container className="d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
            <Row>
                <Col xs={12} md={12} className="mx-auto">
                    <h2 className="text-center mb-4">Login</h2>
                    {message && <Alert variant={message.type}>{message.text}</Alert>}
                    <Form onSubmit={handleSubmit}>
                        <Form.Group controlId="formEmail">
                            <Form.Label>Email address</Form.Label>
                            <Form.Control
                                type="email"
                                placeholder="Enter your email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </Form.Group>

                        <Form.Group controlId="formPassword" className="mt-3">
                            <Form.Label>Password</Form.Label>
                            <Form.Control
                                type="password"
                                placeholder="Enter your password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </Form.Group>

                        <Button variant="primary" type="submit" className="w-100 mt-3">
                            Login
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
};

export default Login;
