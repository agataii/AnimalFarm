import { Link } from 'react-router-dom';
import { useQuery, useMutation } from '@tanstack/react-query';
import { breedsApi } from '../../api/breedsApi';
import { queryClient } from '../../lib/queryClient';

export default function BreedListPage() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ['breeds'],
    queryFn: breedsApi.getAll,
  });

  const deleteMutation = useMutation({
    mutationFn: breedsApi.remove,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['breeds'] }),
  });

  const handleDelete = (id: number) => {
    if (!window.confirm('Are you sure you want to delete this item?')) return;
    deleteMutation.mutate(id);
  };

  if (isLoading) return <p>Loading...</p>;
  if (isError) return <p className="text-red-600">Failed to load data.</p>;

  return (
    <div>
      <div className="flex justify-between items-center mb-5">
        <h1 className="text-2xl font-bold">Breeds</h1>
        <Link
          to="/breeds/new"
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 no-underline"
        >
          Add New
        </Link>
      </div>

      {deleteMutation.isError && (
        <div className="bg-red-50 text-red-700 p-3 rounded mb-4">Delete failed.</div>
      )}

      {data && data.length === 0 ? (
        <p className="text-gray-500">No records found.</p>
      ) : (
        <table className="w-full bg-white shadow rounded-lg overflow-hidden">
          <thead>
            <tr className="bg-slate-800 text-white">
              <th className="text-left p-3">ID</th>
              <th className="text-left p-3">Name</th>
              <th className="text-left p-3">Animal Type</th>
              <th className="text-left p-3">Actions</th>
            </tr>
          </thead>
          <tbody>
            {data?.map((item) => (
              <tr key={item.id} className="border-b hover:bg-gray-50">
                <td className="p-3">{item.id}</td>
                <td className="p-3">{item.name}</td>
                <td className="p-3">{item.animalTypeName}</td>
                <td className="p-3 flex gap-2">
                  <Link
                    to={`/breeds/${item.id}/edit`}
                    className="bg-blue-500 text-white px-3 py-1 rounded text-sm hover:bg-blue-600 no-underline"
                  >
                    Edit
                  </Link>
                  <button
                    onClick={() => handleDelete(item.id)}
                    className="bg-red-500 text-white px-3 py-1 rounded text-sm hover:bg-red-600 cursor-pointer"
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
