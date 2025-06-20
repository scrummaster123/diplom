import axios from 'axios';
import { LoginUserModel, RegistrationUserModel } from '../models/auth';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export const authApi = {
  login: async (data: LoginUserModel) => {
    console.log('Login request:', data);
    const response = await api.post<string>('/Auth/login', data);
    const token = response.data;
    localStorage.setItem('token', token); // Убедитесь, что токен сохраняется
    console.log('Saved token:', token); // Для отладки
    return token;
  },

  register: async (data: RegistrationUserModel) => {
    console.log('Registration request:', data);
    const response = await api.post('/Auth/user-registration', data);
    console.log('Registration response:', response);
    return response;
  },

  getCurrentUser: async (): Promise<{ userId: number }> => {
    //const response = await api.get<{ userId: number }>('/Auth/me');
    //return response.data;
    const id = 3;
    return { userId: id };
  },
};