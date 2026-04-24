import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/auth.service';
import type { LoginRequest } from '../types/auth.types';
import { useAuthStore } from '../authStore';

export const useLogin = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const setToken = useAuthStore((state) => state.setToken);
  const navigate = useNavigate();

  const login = async (request: LoginRequest) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await authService.login(request);
      if (response.data && response.data.token) {
        setToken(response.data.token);
        navigate('/'); // Redirect to dashboard
      } else {
        setError(response.message || 'Đăng nhập thất bại.');
      }
    } catch (err: any) {
      setError(
        err.response?.data?.message || err.message || 'Có lỗi xảy ra khi kết nối đến máy chủ.'
      );
    } finally {
      setIsLoading(false);
    }
  };

  return { login, isLoading, error };
};
