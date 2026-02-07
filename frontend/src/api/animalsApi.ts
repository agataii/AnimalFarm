import api from './axios';
import type { Animal, CreateAnimalRequest } from '../types/animal';

export const animalsApi = {
  getAll: async (): Promise<Animal[]> => {
    const { data } = await api.get<Animal[]>('/animals');
    return data;
  },

  getById: async (id: number): Promise<Animal> => {
    const { data } = await api.get<Animal>(`/animals/${id}`);
    return data;
  },

  create: async (payload: CreateAnimalRequest): Promise<Animal> => {
    const { data } = await api.post<Animal>('/animals', payload);
    return data;
  },

  update: async (id: number, payload: CreateAnimalRequest): Promise<Animal> => {
    const { data } = await api.put<Animal>(`/animals/${id}`, payload);
    return data;
  },

  remove: async (id: number): Promise<void> => {
    await api.delete(`/animals/${id}`);
  },
};
