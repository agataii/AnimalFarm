import api from './axios';
import type { Breed, CreateBreedRequest } from '../types/animal';

export const breedsApi = {
  getAll: async (): Promise<Breed[]> => {
    const { data } = await api.get<Breed[]>('/breeds');
    return data;
  },

  getById: async (id: number): Promise<Breed> => {
    const { data } = await api.get<Breed>(`/breeds/${id}`);
    return data;
  },

  getByAnimalType: async (animalTypeId: number): Promise<Breed[]> => {
    const { data } = await api.get<Breed[]>(`/breeds/by-animal-type/${animalTypeId}`);
    return data;
  },

  create: async (payload: CreateBreedRequest): Promise<Breed> => {
    const { data } = await api.post<Breed>('/breeds', payload);
    return data;
  },

  update: async (id: number, payload: CreateBreedRequest): Promise<Breed> => {
    const { data } = await api.put<Breed>(`/breeds/${id}`, payload);
    return data;
  },

  remove: async (id: number): Promise<void> => {
    await api.delete(`/breeds/${id}`);
  },
};
