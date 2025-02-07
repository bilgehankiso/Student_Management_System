import React, { useEffect, useState } from "react";
import { Container, Table, Button } from "react-bootstrap";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useNavigate, useLocation } from "react-router-dom";

const Grades = () => {
    const [grades, setGrades] = useState([]);
    const navigate = useNavigate();
    const location = useLocation();
    const user = location.state?.user;

    useEffect(() => {
        if (!user) {
            navigate("/login");
        } else {
            fetchGrades(user.id);
        }
    }, []);

    const fetchGrades = async (studentId) => {
        try {
            const response = await axios.get(`https://localhost:7025/api/Grade/student/${studentId}`);
            setGrades(response.data);
        } catch (error) {
            console.error("Error fetching grades:", error);
            toast.error("Failed to fetch grades");
        }
    };

    return (
        <Container className="mt-5">
            <ToastContainer position="top-right" autoClose={3000} />
            <h2 className="text-center mb-4">My Grades</h2>
            <Table striped bordered hover responsive className="text-center">
                <thead>
                    <tr>
                        <th>Course ID</th>
                        <th>Midterm</th>
                        <th>Final</th>
                        <th>Average</th>
                    </tr>
                </thead>
                <tbody>
                    {grades.length > 0 ? (
                        grades.map((grade) => {
                            const average = (grade.midterm + grade.final) / 2;
                            const averageClass = average >= 50 ? 'text-success' : 'text-danger';
                            return (
                                <tr key={grade.id}>
                                    <td>{grade.courseId}</td>
                                    <td>{grade.midterm}</td>
                                    <td>{grade.final}</td>
                                    <td className={averageClass}>{average.toFixed(2)}</td>
                                </tr>
                            );
                        })
                    ) : (
                        <tr>
                            <td colSpan="4">No grades available</td>
                        </tr>
                    )}
                </tbody>
            </Table>
            <Button variant="secondary" onClick={() => navigate(-1)}>
                Back
            </Button>
        </Container>
    );
};

export default Grades;
