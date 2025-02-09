import React, { useEffect, useState } from "react";
import { Container, Table, Button, Modal, Form } from "react-bootstrap";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useNavigate, useLocation } from "react-router-dom";

const GradeOperations = () => {
    const [grades, setGrades] = useState([]);
    const [students, setStudents] = useState([]);
    const [courses, setCourses] = useState([]);
    const [newGrade, setNewGrade] = useState({ studentId: "", courseId: "", midterm: "", final: "" });
    const [editGrade, setEditGrade] = useState({ id: "", studentId: "", courseId: "", midterm: "", final: "" });
    const [showAddModal, setShowAddModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);
    const [selectedCourseId, setSelectedCourseId] = useState("");

    const navigate = useNavigate();
    const location = useLocation();
    const user = location.state?.user;

    useEffect(() => {
        if (!user) {
            navigate("/login");
        }
        fetchGrades();
        fetchStudents();
        fetchCourses();
    }, []);

    const fetchGrades = async () => {
        try {
            const response = await axios.get(`https://localhost:7025/api/Grade/teacher/${user.id}`);
            setGrades(response.data);
        } catch (error) {
            console.error("Error fetching grades:", error);
            toast.error("Failed to fetch grades");
        }
    };

    const fetchStudents = async (courseId) => {
        try {
            if (!courseId) {
                setStudents([]);
                return;
            }

            const response = await axios.get(`https://localhost:7025/api/Course/${courseId}/students`, {
                headers: { accept: "*/*" },
            });

            if (response.data.length === 0) {
                setStudents([]);
            } else {
                setStudents(response.data);
            }
        } catch (error) {
            console.error("Error fetching students:", error);
            toast.error("Failed to fetch students");
            setStudents([]);
        }
    };


    const fetchCourses = async () => {
        try {
            const response = await axios.get(`https://localhost:7025/api/Course/teacher/${user.id}`);
            setCourses(response.data);
        } catch (error) {
            console.error("Error fetching courses:", error);
            toast.error("Failed to fetch courses");
        }
    };

    const handleAddGrade = async () => {
        try {
            await axios.post("https://localhost:7025/api/Grades/add", newGrade, {
                headers: { "Content-Type": "application/json" },
            });
            setShowAddModal(false);
            setNewGrade({ studentId: "", courseId: "", midterm: "", final: "" });
            fetchGrades();
            toast.success("Grade added successfully!");
        } catch (error) {
            console.error("Error adding grade:", error);
            toast.error("Failed to add grade");
        }
    };

    const handleCourseChange = (e) => {
        const courseId = e.target.value;
        setSelectedCourseId(courseId);
        setNewGrade({ ...newGrade, courseId });
        fetchStudents(courseId);
    };

    return (
        <Container className="mt-5">
            <ToastContainer position="top-right" autoClose={3000} />
            <h2 className="text-center mb-4">Grade Management</h2>
            <Button variant="primary" className="mb-3" onClick={() => setShowAddModal(true)}>
                Add New Grade
            </Button>
            <Table striped bordered hover responsive className="text-center">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Student Id</th>
                        <th>Student Name</th>
                        <th>Course Id</th>
                        <th>Course Name</th>
                        <th>Midterm</th>
                        <th>Final</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {grades.map((grade) => (
                        <tr key={grade.id}>
                            <td>{grade.id}</td>
                            <td>{grade.studentId}</td>
                            <td>{grade.studentName}</td>
                            <td>{grade.courseId}</td>
                            <td>{grade.courseName}</td>
                            <td>{grade.midterm}</td>
                            <td>{grade.final}</td>
                            <td>
                                <Button variant="warning" size="sm" className="me-2">
                                    Edit
                                </Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            {/* Add Grade Modal */}
            <Modal show={showAddModal} onHide={() => setShowAddModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Add New Grade</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mt-3">
                            <Form.Label>Course</Form.Label>
                            <Form.Select
                                value={newGrade.courseId}
                                onChange={(e) => handleCourseChange(e)}
                            >
                                <option value="" disabled>Select a course</option>
                                {courses.map((course) => (
                                    <option key={course.id} value={course.id}>
                                        {course.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>

                        <Form.Group>
                            <Form.Label>Student</Form.Label>
                            <Form.Select
                                value={newGrade.studentId}
                                onChange={(e) => setNewGrade({ ...newGrade, studentId: e.target.value })}
                            >
                                <option value="" disabled>Select a student</option>
                                {students.map((student) => (
                                    <option key={student.id} value={student.id}>
                                        {student.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Midterm</Form.Label>
                            <Form.Control
                                type="number"
                                min="0"
                                max="100"
                                value={newGrade.midterm}
                                onChange={(e) => {
                                    const value = Math.max(0, Math.min(100, Number(e.target.value)));
                                    setNewGrade({ ...newGrade, midterm: value });
                                }}
                            />
                        </Form.Group>

                        <Form.Group className="mt-3">
                            <Form.Label>Final</Form.Label>
                            <Form.Control
                                type="number"
                                min="0"
                                max="100"
                                value={newGrade.final}
                                onChange={(e) => {
                                    const value = Math.max(0, Math.min(100, Number(e.target.value)));
                                    setNewGrade({ ...newGrade, final: value });
                                }}
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowAddModal(false)}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleAddGrade}>
                        Add Grade
                    </Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default GradeOperations;
