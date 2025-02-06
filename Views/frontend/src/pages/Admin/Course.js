import React, { useState, useEffect } from "react";
import { Container, Table, Button, Modal, Form } from "react-bootstrap";
import axios from "axios";

const Course = () => {
    const [courses, setCourses] = useState([]);
    const [teachers, setTeachers] = useState([]);
    const [showAddModal, setShowAddModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);
    const [showStudentModal, setShowStudentModal] = useState(false);
    const [newCourse, setNewCourse] = useState({ name: "", teacherId: "" });
    const [editCourse, setEditCourse] = useState({ id: "", name: "", teacherId: "" });
    const [courseForStudents, setCourseForStudents] = useState(null);
    const [studentsInCourse, setStudentsInCourse] = useState([]);
    const [allStudents, setAllStudents] = useState([]);

    useEffect(() => {
        fetchCourses();
        fetchTeachers();
    }, []);

    const fetchCourses = async () => {
        try {
            const response = await axios.get("https://localhost:7025/api/Course/all", {
                headers: { accept: "*/*" },
            });
            setCourses(response.data);
        } catch (error) {
            console.error("Error fetching courses:", error);
        }
    };

    const fetchTeachers = async () => {
        try {
            const response = await axios.get("https://localhost:7025/api/User/getAllUsersByRole", {
                params: { role: "Teacher" }, 
                headers: { accept: "*/*" },
            });
            setTeachers(response.data);
        } catch (error) {
            console.error("Error fetching teachers:", error);
        }
    };
    

    const handleAddCourse = async () => {
        try {
            await axios.post("https://localhost:7025/api/Course/addorupdate", newCourse, {
                headers: { "Content-Type": "application/json" },
            });
            setShowAddModal(false);
            setNewCourse({ name: "", teacherId: "" });
            fetchCourses();
        } catch (error) {
            console.error("Error adding course:", error);
        }
    };

    const handleEditCourse = async () => {
        try {
            const updatedCourse = {
                id: editCourse.id,
                name: editCourse.name,
                teacherId: editCourse.teacherId
            };

            await axios.post("https://localhost:7025/api/Course/addorupdate", updatedCourse, {
                headers: { "Content-Type": "application/json" },
            });

            setShowEditModal(false);
            fetchCourses();
        } catch (error) {
            console.error("Error updating course:", error);
        }
    };

    const openEditModal = (course) => {
        setEditCourse({ id: course.id, name: course.name, teacherId: course.teacherId });
        setShowEditModal(true);
    };

    const addDeleteStudent = async (course) => {
        setCourseForStudents(course);
        try {
            const response = await axios.get(`https://localhost:7025/api/Student/course/${course.id}`);
            setStudentsInCourse(response.data);
            const allResponse = await axios.get("https://localhost:7025/api/Student/all");
            setAllStudents(allResponse.data);
        } catch (error) {
            console.error("Error fetching students:", error);
        }
        setShowStudentModal(true);
    };

    const handleAddStudent = async (studentId) => {
        try {
            await axios.post(`https://localhost:7025/api/Student/addToCourse`, {
                studentId: studentId,
                courseId: courseForStudents.id
            });
            const response = await axios.get(`https://localhost:7025/api/Student/course/${courseForStudents.id}`);
            setStudentsInCourse(response.data);
        } catch (error) {
            console.error("Error adding student:", error);
        }
    };

    const handleRemoveStudent = async (studentId) => {
        try {
            await axios.post(`https://localhost:7025/api/Student/removeFromCourse`, {
                studentId: studentId,
                courseId: courseForStudents.id
            });
            const response = await axios.get(`https://localhost:7025/api/Student/course/${courseForStudents.id}`);
            setStudentsInCourse(response.data);
        } catch (error) {
            console.error("Error removing student:", error);
        }
    };

    return (
        <Container className="mt-5 d-flex justify-content-center">
            <div className="w-75">
                <h2 className="text-center mb-4">Course List</h2>
                <Button variant="primary" className="mb-3" onClick={() => setShowAddModal(true)}>
                    Add New Course
                </Button>
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
                                    <Button variant="warning" size="sm" onClick={() => openEditModal(course)} className="me-2">
                                        Edit Course
                                    </Button>
                                    <Button variant="info" size="sm" onClick={() => addDeleteStudent(course)} className="ms-2">
                                        Add/Delete Student
                                    </Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>

                {/* Add Course Modal */}
                <Modal show={showAddModal} onHide={() => setShowAddModal(false)}>
                    <Modal.Header closeButton>
                        <Modal.Title>Add New Course</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form>
                            <Form.Group controlId="formCourseName">
                                <Form.Label>Course Name</Form.Label>
                                <Form.Control
                                    type="text"
                                    placeholder="Enter course name"
                                    value={newCourse.name}
                                    onChange={(e) => setNewCourse({ ...newCourse, name: e.target.value })}
                                />
                            </Form.Group>

                            <Form.Group controlId="formTeacherSelect" className="mt-3">
                                <Form.Label>Teacher</Form.Label>
                                <Form.Select
                                    value={newCourse.teacherId}
                                    onChange={(e) => setNewCourse({ ...newCourse, teacherId: e.target.value })}
                                >
                                    <option value="">Select a teacher</option>
                                    {teachers.map((teacher) => (
                                        <option key={teacher.id} value={teacher.id}>
                                            {teacher.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Form>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={() => setShowAddModal(false)}>
                            Close
                        </Button>
                        <Button variant="primary" onClick={handleAddCourse}>
                            Add Course
                        </Button>
                    </Modal.Footer>
                </Modal>

                {/* Edit Course Modal */}
                <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
                    <Modal.Header closeButton>
                        <Modal.Title>Edit Course</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form>
                            <Form.Group controlId="editCourseName">
                                <Form.Label>Course Name</Form.Label>
                                <Form.Control
                                    type="text"
                                    placeholder="Enter course name"
                                    value={editCourse.name}
                                    onChange={(e) => setEditCourse({ ...editCourse, name: e.target.value })}
                                />
                            </Form.Group>

                            <Form.Group controlId="editTeacherSelect" className="mt-3">
                                <Form.Label>Teacher</Form.Label>
                                <Form.Select
                                    value={editCourse.teacherId}
                                    onChange={(e) => setEditCourse({ ...editCourse, teacherId: e.target.value })}
                                >
                                    <option value="">Select a teacher</option>
                                    {teachers.map((teacher) => (
                                        <option key={teacher.id} value={teacher.id}>
                                            {teacher.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Form>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={() => setShowEditModal(false)}>
                            Close
                        </Button>
                        <Button variant="primary" onClick={handleEditCourse}>
                            Update Course
                        </Button>
                    </Modal.Footer>
                </Modal>

                {/* Add/Delete Student Modal */}
                <Modal show={showStudentModal} onHide={() => setShowStudentModal(false)}>
                    <Modal.Header closeButton>
                        <Modal.Title>Manage Students for {courseForStudents?.name}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <h5>Students Enrolled</h5>
                        <ul>
                            {studentsInCourse.map((student) => (
                                <li key={student.id}>
                                    {student.name}
                                    <Button
                                        variant="danger"
                                        size="sm"
                                        onClick={() => handleRemoveStudent(student.id)}
                                        className="ms-2"
                                    >
                                        Remove
                                    </Button>
                                </li>
                            ))}
                        </ul>

                        <h5 className="mt-4">Add Students</h5>
                        <ul>
                            {allStudents
                                .filter(student => !studentsInCourse.some(s => s.id === student.id))
                                .map((student) => (
                                    <li key={student.id}>
                                        {student.name}
                                        <Button
                                            variant="success"
                                            size="sm"
                                            onClick={() => handleAddStudent(student.id)}
                                            className="ms-2"
                                        >
                                            Add
                                        </Button>
                                    </li>
                                ))}
                        </ul>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={() => setShowStudentModal(false)}>
                            Close
                        </Button>
                    </Modal.Footer>
                </Modal>
            </div>
        </Container>
    );
};

export default Course;
