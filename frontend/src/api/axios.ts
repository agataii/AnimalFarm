import axios from 'axios';
import type { AuthUser } from '../types/auth';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

api.interceptors.request.use((config) => {
  const stored = localStorage.getItem('auth_user');
  if (stored) {
    try {
      const user: AuthUser = JSON.parse(stored);
      config.headers.Authorization = `Bearer ${user.token}`;
    } catch {
      // Invalid JSON in localStorage — ignore
    }
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('auth_user');
    }
    return Promise.reject(error);
  }
);

export default api;
