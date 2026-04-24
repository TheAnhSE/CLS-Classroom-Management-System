import axios from 'axios';
import { useAuthStore } from '../../features/auth/authStore';

const apiClient = axios.create({
  baseURL: 'http://localhost:5117/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use(
  (config) => {
    const token = useAuthStore.getState().token;
    if (token && config.headers) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

apiClient.interceptors.response.use(
  (response) => {
    // Unwrap ApiResponse<T>
    return response.data;
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      // Handle Unauthorized
      useAuthStore.getState().logout();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default apiClient;
