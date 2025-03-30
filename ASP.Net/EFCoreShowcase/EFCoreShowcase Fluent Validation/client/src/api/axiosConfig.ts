import axios from 'axios';
import { ValidationError } from './errors';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Add response interceptor for better error handling
axiosInstance.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.data?.errors) {
      // Handle validation errors
      return Promise.reject(new ValidationError(
        'Validation failed',
        error.response.data.errors
      ));
    }

    console.error('API Error:', error.message);
    if (error.response?.status === 404) {
      console.error('API not found. Make sure the backend server is running.');
    }
    return Promise.reject(error);
  }
);

// Add request interceptor for debugging
axiosInstance.interceptors.request.use(
  config => {
    console.log(`Making ${config.method?.toUpperCase()} request to: ${config.url}`);
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
