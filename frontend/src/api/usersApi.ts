import api from './axios';
import type { UserDto } from '../types/animal';

export const usersApi = {
  getAll: async (): Promise<UserDto[]> => {
    const { data } = await api.get<UserDto[]>('/users');
    return data;
  },

  activate: async (id: string): Promise<{ message: string }> => {
    const { data } = await api.put<{ message: string }>(`/users/${id}/activate`);
    return data;
  },

  deactivate: async (id: string): Promise<{ message: string }> => {
    const { data } = await api.put<{ message: string }>(`/users/${id}/deactivate`);
    return data;
  },
};
