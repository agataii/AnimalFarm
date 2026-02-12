import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import type { Resolver } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useQuery, useMutation } from '@tanstack/react-query';
import { animalsApi } from '../../api/animalsApi';
import { breedsApi } from '../../api/breedsApi';
import { queryClient } from '../../lib/queryClient';
import { AxiosError } from 'axios';
import type { ApiError } from '../../types/common';

const schema = z.object({
  inventoryNumber: z.string().min(1, 'Inventory number is required').max(50),
  gender: z.string().min(1, 'Gender is required'),
  name: z.string().min(1, 'Name is required').max(200),
  arrivalDate: z.string().min(1, 'Arrival date is required'),
  arrivalAgeMonths: z
    .union([z.string(), z.number()])
    .transform((v) => (v === '' || v === undefined ? NaN : Number(v)))
    .pipe(z.number().min(0, 'Must be 0 or greater')),
  breedId: z
    .union([z.string(), z.number()])
    .transform((v) => (v === '' || v === undefined ? NaN : Number(v)))
    .pipe(z.number().positive({ message: 'Breed is required' })),
  parentAnimalId: z.string().optional(),
});

type FormData = z.infer<typeof schema>;

export default function AnimalFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [apiError, setApiError] = useState('');

  const { data: existing } = useQuery({
    queryKey: ['animals', id],
    queryFn: () => animalsApi.getById(Number(id)),
    enabled: isEdit,
  });

  const { data: breeds } = useQuery({
    queryKey: ['breeds'],
    queryFn: breedsApi.getAll,
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
        inventoryNumber: existing.inventoryNumber,
        gender: existing.gender,
        name: existing.name,
        arrivalDate: existing.arrivalDate.substring(0, 10),
        arrivalAgeMonths: existing.arrivalAgeMonths,
        breedId: existing.breedId,
        parentAnimalId: existing.parentAnimalId?.toString() ?? '',
      });
    }
  }, [existing, reset]);

  const mutation = useMutation({
    mutationFn: (data: FormData) => {
      const payload = {
        inventoryNumber: data.inventoryNumber,
        gender: data.gender,
        name: data.name,
        arrivalDate: new Date(data.arrivalDate).toISOString(),
        arrivalAgeMonths: data.arrivalAgeMonths,
        breedId: data.breedId,
        parentAnimalId: data.parentAnimalId ? Number(data.parentAnimalId) : null,
      };
      return isEdit
        ? animalsApi.update(Number(id), payload)
        : animalsApi.create(payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['animals'] });
      navigate('/animals');
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

  // Filter out current animal from parent options
  const parentOptions = animals?.filter((a) => a.id !== Number(id)) ?? [];

  return (
    <div className="max-w-lg">
      <h1 className="text-2xl font-bold mb-6">
        {isEdit ? 'Edit Animal' : 'Create Animal'}
      </h1>

      {apiError && (
        <div className="bg-red-50 text-red-700 p-3 rounded mb-4">{apiError}</div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} className="bg-white p-6 rounded-lg shadow">
        <div className="mb-4">
          <label htmlFor="inventoryNumber" className="block mb-1 font-medium">
            Inventory Number
          </label>
          <input
            id="inventoryNumber"
            type="text"
            {...register('inventoryNumber')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
          {errors.inventoryNumber && (
            <p className="text-red-500 text-sm mt-1">{errors.inventoryNumber.message}</p>
          )}
        </div>

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
          <label htmlFor="gender" className="block mb-1 font-medium">
            Gender
          </label>
          <select
            id="gender"
            {...register('gender')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          >
            <option value="">Select gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
          {errors.gender && (
            <p className="text-red-500 text-sm mt-1">{errors.gender.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="breedId" className="block mb-1 font-medium">
            Breed
          </label>
          <select
            id="breedId"
            {...register('breedId')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          >
            <option value="">Select breed</option>
            {breeds?.map((b) => (
              <option key={b.id} value={b.id}>
                {b.name}
              </option>
            ))}
          </select>
          {errors.breedId && (
            <p className="text-red-500 text-sm mt-1">{errors.breedId.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="arrivalDate" className="block mb-1 font-medium">
            Arrival Date
          </label>
          <input
            id="arrivalDate"
            type="date"
            {...register('arrivalDate')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
          {errors.arrivalDate && (
            <p className="text-red-500 text-sm mt-1">{errors.arrivalDate.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="arrivalAgeMonths" className="block mb-1 font-medium">
            Arrival Age (months)
          </label>
          <input
            id="arrivalAgeMonths"
            type="number"
            min={0}
            {...register('arrivalAgeMonths')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
          {errors.arrivalAgeMonths && (
            <p className="text-red-500 text-sm mt-1">{errors.arrivalAgeMonths.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="parentAnimalId" className="block mb-1 font-medium">
            Parent Animal (optional)
          </label>
          <select
            id="parentAnimalId"
            {...register('parentAnimalId')}
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          >
            <option value="">None</option>
            {parentOptions.map((a) => (
              <option key={a.id} value={a.id}>
                {a.name} ({a.inventoryNumber})
              </option>
            ))}
          </select>
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
            to="/animals"
            className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500 no-underline"
          >
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}
