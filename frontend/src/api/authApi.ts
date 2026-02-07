import api from './axios';
import type { LoginRequest, RegisterRequest, AuthResponse } from '../types/auth';

export const authApi = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const res = await api.post<AuthResponse>('/auth/login', data);
    return res.data;
  },

  register: async (data: RegisterRequest): Promise<{ message: string }> => {
    const res = await api.post<{ message: string }>('/auth/register', data);
    return res.data;
  },

  activate: async (userId: string, token: string): Promise<{ message: string }> => {
    const res = await api.get<{ message: string }>('/auth/activate', {
      params: { userId, token },
    });
    return res.data;
  },
};
