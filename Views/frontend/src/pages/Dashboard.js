import React from 'react';
import { useNavigate } from 'react-router-dom';

function Dashboard() {
  const navigate = useNavigate();

  // Çıkış işlemi
  const handleLogout = () => {
    navigate('/login'); 
  };

  return (
    <div className="dashboard">
      <h1>Welcome to your Dashboard</h1>
      <p>This is your control panel where you can manage everything.</p>
      
      <div className="dashboard-content">
        <h2>Upcoming Courses</h2>
        <ul>
          <li>Course 1</li>
          <li>Course 2</li>
          <li>Course 3</li>
        </ul>
      </div>

      <button onClick={handleLogout}>Logout</button>
    </div>
  );
}

export default Dashboard;
