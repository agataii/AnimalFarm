import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useQuery, useMutation } from '@tanstack/react-query';
import { animalTypesApi } from '../../api/animalTypesApi';
import { queryClient } from '../../lib/queryClient';
import { AxiosError } from 'axios';
import type { ApiError } from '../../types/common';

const schema = z.object({
  name: z.string().min(1, 'Name is required').max(100, 'Max 100 characters'),
});

type FormData = z.infer<typeof schema>;

export default function AnimalTypeFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [apiError, setApiError] = useState('');

  const { data: existing } = useQuery({
    queryKey: ['animalTypes', id],
    queryFn: () => animalTypesApi.getById(Number(id)),
    enabled: isEdit,
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  useEffect(() => {
    if (existing) {
      reset({ name: existing.name });
    }
  }, [existing, reset]);

  const mutation = useMutation({
    mutationFn: (data: FormData) =>
      isEdit
        ? animalTypesApi.update(Number(id), data)
        : animalTypesApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['animalTypes'] });
      navigate('/animal-types');
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
        {isEdit ? 'Edit Animal Type' : 'Create Animal Type'}
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

        <div className="flex gap-3 mt-5">
          <button
            type="submit"
            disabled={mutation.isPending}
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-blue-300 cursor-pointer"
          >
            {isEdit ? 'Update' : 'Create'}
          </button>
          <Link
            to="/animal-types"
            className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500 no-underline"
          >
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}
