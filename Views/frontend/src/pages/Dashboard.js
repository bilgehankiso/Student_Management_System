import React from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useNavigate, useLocation } from "react-router-dom";

const Dashboard = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const user = location.state?.user;

    const handleLogout = () => {
        navigate("/login");
    };

    return (
        <Container className="d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
            <Row>
                <Col xs={12} md={12} className="mx-auto text-center">
                    {user ? (
                        <>
                            <h2>Welcome to the Dashboard, {user.name}!</h2>
                            <p><strong>Email:</strong> {user.email}</p>
                            <p><strong>Role:</strong> {user.role}</p>
                            <p><strong>User ID:</strong> {user.id}</p>
                        </>
                    ) : (
                        <h2>Welcome to the Dashboard!</h2>
                    )}
                    <Button variant="danger" className="mt-3" onClick={handleLogout}>
                        Logout
                    </Button>
                </Col>
            </Row>
        </Container>
    );
};

export default Dashboard;
