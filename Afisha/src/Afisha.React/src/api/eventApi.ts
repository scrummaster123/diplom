import axios from 'axios';
import { OutputEvent, PagedEventsResponse } from '../models/event';

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

export const eventApi = {
  getEventById: async (id: number): Promise<OutputEvent> => {
    const response = await api.get<OutputEvent>('/Event', { params: { id } });
    return response.data;
  },

  getEventsPaged: async (page: number, pageSize: number = 20): Promise<PagedEventsResponse> => {
    const response = await api.get<PagedEventsResponse>('/Event/list', {
      params: { page, pageSize },
    });
    return {
      events: Array.isArray(response.data.events) ? response.data.events : [],
      totalCount: response.data.totalCount || 0,
      totalPages: response.data.totalPages || 1,
    };
  },
};