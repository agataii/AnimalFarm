import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import type { Resolver } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useQuery, useMutation } from '@tanstack/react-query';
import { breedsApi } from '../../api/breedsApi';
import { animalTypesApi } from '../../api/animalTypesApi';
import { queryClient } from '../../lib/queryClient';
import { AxiosError } from 'axios';
import type { ApiError } from '../../types/common';

const schema = z.object({
  name: z.string().min(1, 'Name is required').max(100, 'Max 100 characters'),
  animalTypeId: z
    .union([z.string(), z.number()])
    .transform((v) => (v === '' || v === undefined ? NaN : Number(v)))
    .pipe(z.number().positive({ message: 'Animal type is required' })),
});

type FormData = z.infer<typeof schema>;

export default function BreedFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [apiError, setApiError] = useState('');

  const { data: existing } = useQuery({
    queryKey: ['breeds', id],
    queryFn: () => breedsApi.getById(Number(id)),
    enabled: isEdit,
  });

  const { data: animalTypes } = useQuery({
    queryKey: ['animalTypes'],
    queryFn: animalTypesApi.getAll,
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema) as Resolver<FormData>,
  });

  useEffect(() => {
    if (existing) {
      reset({ name: existing.name, animalTypeId: existing.animalTypeId });
    }
  }, [existing, reset]);

  const mutation = useMutation({
    mutationFn: (data: FormData) =>
      isEdit ? breedsApi.update(Number(id), data) : breedsApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['breeds'] });
      navigate('/breeds');
    },
    onError: (err: AxiosError<ApiError>) => {
      const data = err.response?.data;
      if (data?.errors && data.errors.length > 0) {
        setApiError(data.errors.join(', '));
      } else {
        setApiError(data?.message || 'Operation failed');
      }
    },
  });

  const onSubmit = (data: FormData) => {
    setApiError('');
    mutation.mutate(data);
  };

  return (
    <div className="max-w-lg">
      <h1 className="text-2xl font-bold mb-6">
        {isEdit ? 'Edit Breed' : 'Create Breed'}
      </h1>

      {apiError && (
        <div className="bg-red-50 text-red-700 p-3 rounded mb-4">{apiError}</div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} className="bg-white p-6 rounded-lg shadow">
        <div className="mb-4">
          <label htmlFor="name" className="block mb-1 font-medium">
            Name
          </label>
          <input
            id="name"
            type="text"
            {...register('name')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
          {errors.name && (
            <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="animalTypeId" className="block mb-1 font-medium">
            Animal Type
          </label>
          <select
            id="animalTypeId"
            {...register('animalTypeId')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          >
            <option value="">Select animal type</option>
            {animalTypes?.map((at) => (
              <option key={at.id} value={at.id}>
                {at.name}
              </option>
            ))}
          </select>
          {errors.animalTypeId && (
            <p className="text-red-500 text-sm mt-1">{errors.animalTypeId.message}</p>
          )}
        </div>

        <div className="flex gap-3 mt-5">
          <button
            type="submit"
            disabled={mutation.isPending}
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-blue-300 cursor-pointer"
          >
            {isEdit ? 'Update' : 'Create'}
          </button>
          <Link
            to="/breeds"
            className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500 no-underline"
          >
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}
