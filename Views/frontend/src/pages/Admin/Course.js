import React, { useState, useEffect } from "react";
import { Container, Table, Button } from "react-bootstrap";
import axios from "axios";

const Course = () => {
    const [courses, setCourses] = useState([]);

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await axios.get("https://localhost:7025/api/Course/all", {
                    headers: {
                        accept: "*/*",
                    },
                });
                setCourses(response.data); 
            } catch (error) {
                console.error("Error fetching courses:", error);
            }
        };

        fetchCourses(); 
    }, []);

    return (

        <Container className="mt-5 d-flex justify-content-center">
        <div className="w-75">
            <h2 className="text-center mb-4">Course List</h2>
            <Table striped bordered hover responsive className="text-center">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Course Name</th>
                        <th>Teacher Name</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {courses.map((course) => (
                        <tr key={course.id}>
                            <td>{course.id}</td>
                            <td>{course.name}</td>
                            <td>{course.teacherName}</td>
                            <td>
                                <Button variant="warning" size="sm">
                                    Edit
                                </Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    </Container>
    );
};

export default Course;
