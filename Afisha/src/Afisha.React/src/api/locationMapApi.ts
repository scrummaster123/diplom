import axios from 'axios';
import { OutputLocation } from '../models/event';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
});
const BASE_URL = import.meta.env.VITE_API_URL || '/api';

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
      console.log('Sending token:', token);
    } else {
      console.log('No token found in localStorage');
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export const locationMapApi = {
  getMap: async (): Promise<{ mapUrl: string; mapWidth: number; mapHeight: number }> => {
    const response = await api.get('/Location/map');
    response.data.mapUrl = BASE_URL + response.data.mapUrl;
    return response.data;
  },

  getLocations: async (): Promise<OutputLocation[]> => {
    const response = await api.get('/Location/map-locations');
    // Extract $values if it exists, otherwise return response.data as a fallback
    return response.data.$values ? response.data.$values : Array.isArray(response.data) ? response.data : [];
  },
};