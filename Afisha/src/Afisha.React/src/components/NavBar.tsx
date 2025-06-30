import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import './NavBar.css';

const NavBar: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token'); // Удаляем токен из localStorage
    navigate('/login'); // Перенаправляем на страницу логина
  };

  return (
    <nav className="navbar">
      <ul className="nav-list">
        <li>
          <NavLink to="/locations" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
            Локации
          </NavLink>
        </li>
        <li>
          <NavLink to="/map" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
            Локации на карте
          </NavLink>
        </li>
        <li>
          <NavLink to="/events" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
            События
          </NavLink>
        </li>
        <li>
          <NavLink to="/rating" className={({ isActive }) => (isActive ? 'nav-link active' : 'nav-link')}>
            Рейтинг
          </NavLink>
        </li>
        <li>
          <button className="nav-link logout-button" onClick={handleLogout}>
            Выход
          </button>
        </li>
      </ul>
    </nav>
  );
};

export default NavBar;