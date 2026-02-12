import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import type { Resolver } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useQuery, useMutation } from '@tanstack/react-query';
import { weightingsApi } from '../../api/weightingsApi';
import { animalsApi } from '../../api/animalsApi';
import { queryClient } from '../../lib/queryClient';
import { AxiosError } from 'axios';
import type { ApiError } from '../../types/common';

const schema = z.object({
  animalId: z
    .union([z.string(), z.number()])
    .transform((v) => (v === '' || v === undefined ? NaN : Number(v)))
    .pipe(z.number().positive({ message: 'Animal is required' })),
  date: z.string().min(1, 'Date is required'),
  weightKg: z
    .union([z.string(), z.number()])
    .transform((v) => {
      if (v === '' || v === undefined) return NaN;
      const normalized = typeof v === 'string' ? v.replace(',', '.') : v;
      return Number(normalized);
    })
    .pipe(z.number().positive({ message: 'Weight must be greater than 0' })),
});

type FormData = z.infer<typeof schema>;

export default function WeightingFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [apiError, setApiError] = useState('');

  const { data: existing } = useQuery({
    queryKey: ['weightings', id],
    queryFn: () => weightingsApi.getById(Number(id)),
    enabled: isEdit,
  });

  const { data: animals } = useQuery({
    queryKey: ['animals'],
    queryFn: animalsApi.getAll,
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
      reset({
        animalId: existing.animalId,
        date: existing.date.substring(0, 10),
        weightKg: existing.weightKg,
      });
    }
  }, [existing, reset]);

  const mutation = useMutation({
    mutationFn: (data: FormData) => {
      const payload = {
        ...data,
        date: new Date(data.date).toISOString(),
      };
      return isEdit
        ? weightingsApi.update(Number(id), payload)
        : weightingsApi.create(payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['weightings'] });
      navigate('/weightings');
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
        {isEdit ? 'Edit Weighting' : 'Create Weighting'}
      </h1>

      {apiError && (
        <div className="bg-red-50 text-red-700 p-3 rounded mb-4">{apiError}</div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} className="bg-white p-6 rounded-lg shadow">
        <div className="mb-4">
          <label htmlFor="animalId" className="block mb-1 font-medium">
            Animal
          </label>
          <select
            id="animalId"
            {...register('animalId')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          >
            <option value="">Select animal</option>
            {animals?.map((a) => (
              <option key={a.id} value={a.id}>
                {a.name} ({a.inventoryNumber})
              </option>
            ))}
          </select>
          {errors.animalId && (
            <p className="text-red-500 text-sm mt-1">{errors.animalId.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="date" className="block mb-1 font-medium">
            Date
          </label>
          <input
            id="date"
            type="date"
            {...register('date')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
          {errors.date && (
            <p className="text-red-500 text-sm mt-1">{errors.date.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="weightKg" className="block mb-1 font-medium">
            Weight (kg)
          </label>
          <input
            id="weightKg"
            type="number"
            step="0.01"
            min={0}
            {...register('weightKg')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
          {errors.weightKg && (
            <p className="text-red-500 text-sm mt-1">{errors.weightKg.message}</p>
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
            to="/weightings"
            className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500 no-underline"
          >
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}
