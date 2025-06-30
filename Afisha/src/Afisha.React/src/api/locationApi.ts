import axios from 'axios';
import { OutputLocationFull, PagedLocationsResponse } from '../models/location';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
});

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

export const locationApi = {
  getLocationById: async (id: number): Promise<OutputLocationFull> => {
    const response = await api.get<OutputLocationFull>(`/Location`, { params: { id } });
    return response.data;
  },

  getLocationsPaged: async (page: number, pageSize: number = 20): Promise<PagedLocationsResponse> => {
    const response = await api.get<PagedLocationsResponse>(`/Location/list`, {
      params: { page, pageSize },
    });
    console.log('response.data:', response.data);
    console.log('response.data.locations:', response.data.locations);
    return {
      locations: Array.isArray(response.data.locations) ? response.data.locations : [],
      totalCount: response.data.totalCount || 0,
      totalPages: response.data.totalPages || 1,
    };
  },
};