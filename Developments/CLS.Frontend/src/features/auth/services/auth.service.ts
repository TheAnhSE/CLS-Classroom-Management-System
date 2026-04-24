import apiClient from '../../../shared/services/api.client';
import type { ApiResponse } from '../../../shared/types/api.types';
import type { LoginRequest, LoginResponse } from '../types/auth.types';

export const authService = {
  login: async (request: LoginRequest): Promise<ApiResponse<LoginResponse>> => {
    return await apiClient.post<any, ApiResponse<LoginResponse>>('/auth/login', request);
  },
};
