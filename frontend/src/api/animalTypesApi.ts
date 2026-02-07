import api from './axios';
import type { AnimalType, CreateAnimalTypeRequest } from '../types/animal';

export const animalTypesApi = {
  getAll: async (): Promise<AnimalType[]> => {
    const { data } = await api.get<AnimalType[]>('/animaltypes');
    return data;
  },

  getById: async (id: number): Promise<AnimalType> => {
    const { data } = await api.get<AnimalType>(`/animaltypes/${id}`);
    return data;
  },

  create: async (payload: CreateAnimalTypeRequest): Promise<AnimalType> => {
    const { data } = await api.post<AnimalType>('/animaltypes', payload);
    return data;
  },

  update: async (id: number, payload: CreateAnimalTypeRequest): Promise<AnimalType> => {
    const { data } = await api.put<AnimalType>(`/animaltypes/${id}`, payload);
    return data;
  },

  remove: async (id: number): Promise<void> => {
    await api.delete(`/animaltypes/${id}`);
  },
};
