import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";  
import { Button, Form, Container } from "react-bootstrap";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const navigate = useNavigate();  

  const handleLogin = async (e) => {
    e.preventDefault();

    const loginData = { email, password };
    try {
      const response = await axios.post("http://localhost:5000/api/user/login", loginData); 
      console.log(response.data);  
      navigate("/dashboard");
    } catch (error) {
      setErrorMessage("Invalid credentials or something went wrong.");
    }
  };

  return (
    <Container>
      <h2 className="text-center mt-5">Login</h2>
      <Form onSubmit={handleLogin} className="mt-3">
        <Form.Group controlId="formEmail">
          <Form.Label>Email</Form.Label>
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

        {errorMessage && <div className="text-danger mt-2">{errorMessage}</div>}

        <Button variant="primary" type="submit" className="mt-3">
          Login
        </Button>
      </Form>
    </Container>
  );
};

export default Login;
