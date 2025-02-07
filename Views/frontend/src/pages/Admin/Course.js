import React, { useState, useEffect } from "react";
import { Container, Table, Button, Modal, Form } from "react-bootstrap";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useNavigate } from "react-router-dom";



const Course = () => {
    <ToastContainer />

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
    const [newStudentId, setNewStudentId] = useState("");

    useEffect(() => {
        fetchCourses();
        fetchTeachers();
        fetchStudents();
    }, []);
    const navigate = useNavigate();

    const handleRegisterRedirect = () => {
        navigate("/register");
    };

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
    const [students, setStudents] = useState([]);

    const fetchStudents = async () => {
        try {
            const response = await axios.get("https://localhost:7025/api/User/getAllUsersByRole", {
                params: { role: "Student" },
                headers: { accept: "*/*" },
            });
            setStudents(response.data);
        } catch (error) {
            console.error("Error fetching students:", error);
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
            toast.success("Course Successfully added!");
        } catch (error) {
            toast.error("Course Could not Added. Please check datas and try again!");
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
            toast.success("Course Successfully Edited!");
        } catch (error) {
            toast.error(error.response.data.message);
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
        if (!studentId || !courseForStudents?.id) {
            console.error("Invalid student or course ID");
            return;
        }

        try {
            await axios.post("https://localhost:7025/api/Course/join", {
                studentId: parseInt(studentId, 10),
                courseId: parseInt(courseForStudents.id, 10)
            }, {
                headers: { "Content-Type": "application/json", "accept": "*/*" }
            });

            toast.success("Successfully added!");
            console.log("Successfully added!");

            setShowStudentModal(false);
            setNewStudentId("");

        } catch (error) {
            console.error("Error adding student:", error);
            toast.error(error.response.data.message);
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
                <h2 className="text-center mt-4 mb-4">Course List</h2>

                <div className="d-flex mb-3">
                    <Button variant="primary" onClick={() => setShowAddModal(true)}>
                        Add New Course
                    </Button>
                    <Button variant="primary" onClick={handleRegisterRedirect} className="ms-2">
                        Register New User
                    </Button>
                </div>

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
                                        Add Student
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
                                    <option value="" disabled >Select a teacher</option>
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
                                    <option value="" disabled >Select a teacher</option>
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

                        <Form.Group controlId="formStudentSelect">
                            <Form.Label>Select a Student</Form.Label>
                            <Form.Select
                                value={newStudentId}
                                onChange={(e) => setNewStudentId(e.target.value)}
                            >
                                <option value="" disabled > Select a student</option>
                                {students.map((student) => (
                                    <option key={student.id} value={student.id}>
                                        {student.name}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>

                        <div className="d-flex justify-content-center mt-3">
                            <Button variant="success" size="md" className="w-25" onClick={() => handleAddStudent(newStudentId)}>
                                Add
                            </Button>
                        </div>

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
