import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authApi } from '../api/authApi';
import { RegistrationUserModel } from '../models/auth';

const Register: React.FC = () => {
  const [form, setForm] = useState<RegistrationUserModel>({
    firstName: '',
    lastName: '',
    patronymic: '',
    email: '',
    login: '',
    password: '',
    confirmPassword: '',
    birthday: '',
    isMale: undefined,
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await authApi.register(form);
      navigate('/login');
    } catch (err: any) {
      setError(err.response?.data?.title || 'Registration failed');
    }
  };

  return (
    <div>
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>First Name:</label>
          <input
            type="text"
            name="firstName"
            value={form.firstName}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Last Name:</label>
          <input
            type="text"
            name="lastName"
            value={form.lastName}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Patronymic (optional):</label>
          <input
            type="text"
            name="patronymic"
            value={form.patronymic}
            onChange={handleChange}
          />
        </div>
        <div>
          <label>Email:</label>
          <input
            type="email"
            name="email"
            value={form.email}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Login:</label>
          <input
            type="text"
            name="login"
            value={form.login}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            name="password"
            value={form.password}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Confirm Password:</label>
          <input
            type="password"
            name="confirmPassword"
            value={form.confirmPassword}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Birthday (optional):</label>
          <input
            type="date"
            name="birthday"
            value={form.birthday}
            onChange={handleChange}
            min="1925-01-01"
            max="2025-01-01"
          />
        </div>
        <div>
          <label>Gender (optional):</label>
          <select
            name="isMale"
            value={form.isMale === undefined ? '' : form.isMale.toString()}
            onChange={handleChange}
          >
            <option value="">Select</option>
            <option value="true">Male</option>
            <option value="false">Female</option>
          </select>
        </div>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit">Register</button>
      </form>
      <p>
        Already have an account? <a href="/login">Login</a>
      </p>
    </div>
  );
};

export default Register;