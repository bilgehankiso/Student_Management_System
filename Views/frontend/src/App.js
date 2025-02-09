import React, { useEffect } from "react";
import { BrowserRouter as Router, Routes, Route, useNavigate } from "react-router-dom";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Register from "./pages/Admin/Register";
import Course from "./pages/Admin/Course";
import Grades from "./pages/Student/Grades";
import GradeOperations from "./pages/Teacher/GradeOperations";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const HomeRedirect = () => {
  const navigate = useNavigate();

  useEffect(() => {
    navigate("/login");
  }, []);
  return null;
};

function App() {
  return (
    <Router>
      <ToastContainer />
      <Routes>
        <Route path="/" element={<HomeRedirect />} />
        <Route path="/login" element={<Login />} />
        <Route path="/teacher/GradeOperations" element={<GradeOperations />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/register" element={<Register />} />
        <Route path="/admin/Course" element={<Course />} />
        <Route path="/student/Grades" element={<Grades />} />
      </Routes>
    </Router>
  );
}

export default App;
