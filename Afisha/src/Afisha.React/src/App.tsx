import React from 'react';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import NavBar from './components/NavBar';
import Register from './components/Register';
import Login from './components/Login';
import Locations from './components/Locations';
import Events from './components/Events';
import Rating from './components/Rating';
import './App.css';

const AppContent: React.FC = () => {
  const location = useLocation();
  // Скрываем NavBar на /login, /register и /
  const hideNavBar = ['/login', '/register', '/'].includes(location.pathname);

  return (
    <div>
      {!hideNavBar && <NavBar />}
      <div className="content">
        <Routes>
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
          <Route path="/locations" element={<Locations />} />
          <Route path="/events" element={<Events />} />
          <Route path="/rating" element={<Rating />} />
          <Route path="/" element={<Login />} />
        </Routes>
      </div>
    </div>
  );
};

const App: React.FC = () => {
  return (
    <Router>
      <AppContent />
    </Router>
  );
};

export default App;