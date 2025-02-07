import React, { useEffect, useState } from "react";
import { Container, Table, Button, Modal, Form } from "react-bootstrap";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const GradeOperations = () => {
    const [grades, setGrades] = useState([]);
    const [students, setStudents] = useState([]);
    const [newGrade, setNewGrade] = useState({ studentId: "", course: "", score: "" });
    const [editGrade, setEditGrade] = useState({ id: "", studentId: "", course: "", score: "" });
    const [showAddModal, setShowAddModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);

    useEffect(() => {
        fetchGrades();
        fetchStudents();
    }, []);

    const fetchGrades = async () => {
        try {
            const response = await axios.get("https://localhost:7025/api/Grades/all");
            setGrades(response.data);
        } catch (error) {
            console.error("Error fetching grades:", error);
            toast.error("Failed to fetch grades ");
        }
    };

    const fetchStudents = async () => {
        try {
            const response = await axios.get("https://localhost:7025/api/Students/all");
            setStudents(response.data);
        } catch (error) {
            console.error("Error fetching students:", error);
        }
    };

    const handleAddGrade = async () => {
        try {
            await axios.post("https://localhost:7025/api/Grades/add", newGrade, {
                headers: { "Content-Type": "application/json" },
            });
            setShowAddModal(false);
            setNewGrade({ studentId: "", course: "", score: "" });
            fetchGrades();
            toast.success("Grade added successfully! ");
        } catch (error) {
            console.error("Error adding grade:", error);
            toast.error("Failed to add grade ");
        }
    };

    const handleEditGrade = async () => {
        try {
            await axios.put("https://localhost:7025/api/Grades/update", editGrade, {
                headers: { "Content-Type": "application/json" },
            });
            setShowEditModal(false);
            fetchGrades();
            toast.success("Grade updated successfully! ");
        } catch (error) {
            console.error("Error updating grade:", error);
            toast.error("Failed to update grade ");
        }
    };

    const handleDeleteGrade = async (id) => {
        try {
            await axios.delete(`https://localhost:7025/api/Grades/delete/${id}`);
            fetchGrades();
            toast.success("Grade deleted successfully! ");
        } catch (error) {
            console.error("Error deleting grade:", error);
            toast.error("Failed to delete grade ");
        }
    };

    const openEditModal = (grade) => {
        setEditGrade({ id: grade.id, studentId: grade.studentId, course: grade.course, score: grade.score });
        setShowEditModal(true);
    };

    return (
        <Container className="mt-5">
            <ToastContainer position="top-right" autoClose={3000} />
            <h2 className="text-center mb-4">Grade Management</h2>
            <Button variant="primary" className="mb-3" onClick={() => setShowAddModal(true)}>
                Add New Grade
            </Button>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Student Name</th>
                        <th>Course</th>
                        <th>Score</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {grades.map((grade) => (
                        <tr key={grade.id}>
                            <td>{grade.id}</td>
                            <td>{grade.studentName}</td>
                            <td>{grade.course}</td>
                            <td>{grade.score}</td>
                            <td>
                                <Button variant="warning" size="sm" onClick={() => openEditModal(grade)} className="me-2">
                                    Edit
                                </Button>
                                <Button variant="danger" size="sm" onClick={() => handleDeleteGrade(grade.id)}>
                                    Delete
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
                        <Form.Group>
                            <Form.Label>Student</Form.Label>
                            <Form.Select value={newGrade.studentId} onChange={(e) => setNewGrade({ ...newGrade, studentId: e.target.value })}>
                                <option value="" disabled>Select a student</option>
                                {students.map((student) => (
                                    <option key={student.id} value={student.id}>
                                        {student.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Course</Form.Label>
                            <Form.Control type="text" value={newGrade.course} onChange={(e) => setNewGrade({ ...newGrade, course: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Score</Form.Label>
                            <Form.Control type="number" value={newGrade.score} onChange={(e) => setNewGrade({ ...newGrade, score: e.target.value })} />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowAddModal(false)}>Close</Button>
                    <Button variant="primary" onClick={handleAddGrade}>Add Grade</Button>
                </Modal.Footer>
            </Modal>

            {/* Edit Grade Modal */}
            <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Edit Grade</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group>
                            <Form.Label>Course</Form.Label>
                            <Form.Control type="text" value={editGrade.course} onChange={(e) => setEditGrade({ ...editGrade, course: e.target.value })} />
                        </Form.Group>
                        <Form.Group className="mt-3">
                            <Form.Label>Score</Form.Label>
                            <Form.Control type="number" value={editGrade.score} onChange={(e) => setEditGrade({ ...editGrade, score: e.target.value })} />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowEditModal(false)}>Close</Button>
                    <Button variant="primary" onClick={handleEditGrade}>Update Grade</Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
};

export default GradeOperations;
