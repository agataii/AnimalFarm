import api from './axios';
import type { Weighting, CreateWeightingRequest } from '../types/animal';

export const weightingsApi = {
  getAll: async (): Promise<Weighting[]> => {
    const { data } = await api.get<Weighting[]>('/weightings');
    return data;
  },

  getById: async (id: number): Promise<Weighting> => {
    const { data } = await api.get<Weighting>(`/weightings/${id}`);
    return data;
  },

  create: async (payload: CreateWeightingRequest): Promise<Weighting> => {
    const { data } = await api.post<Weighting>('/weightings', payload);
    return data;
  },

  update: async (id: number, payload: CreateWeightingRequest): Promise<Weighting> => {
    const { data } = await api.put<Weighting>(`/weightings/${id}`, payload);
    return data;
  },

  remove: async (id: number): Promise<void> => {
    await api.delete(`/weightings/${id}`);
  },
};
