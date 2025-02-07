import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Register from './pages/Admin/Register'; 
import Course from './pages/Admin/Course'; 
import GradeOperations from './pages/Teacher/GradeOperations';
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <Router>
      <ToastContainer /> 
      <Routes>
        <Route path="/" element={<h1>Welcome to Home Page</h1>} />
        <Route path="/login" element={<Login />} />
        <Route path="/teacher/GradeOperations" element={<GradeOperations />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/register" element={<Register />} />
        <Route path="/admin/Course" element={<Course />} />
      </Routes>
    </Router>
  );
}

export default App;
