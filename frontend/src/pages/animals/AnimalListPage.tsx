import { Link } from 'react-router-dom';
import { useQuery, useMutation } from '@tanstack/react-query';
import { animalsApi } from '../../api/animalsApi';
import { queryClient } from '../../lib/queryClient';

export default function AnimalListPage() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ['animals'],
    queryFn: animalsApi.getAll,
  });

  const deleteMutation = useMutation({
    mutationFn: animalsApi.remove,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['animals'] }),
  });

  const handleDelete = (id: number) => {
    if (!window.confirm('Are you sure you want to delete this animal?')) return;
    deleteMutation.mutate(id);
  };

  if (isLoading) return <p>Loading...</p>;
  if (isError) return <p className="text-red-600">Failed to load data.</p>;

  return (
    <div>
      <div className="flex justify-between items-center mb-5">
        <h1 className="text-2xl font-bold">Animals</h1>
        <Link
          to="/animals/new"
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
        <div className="overflow-x-auto">
          <table className="w-full bg-white shadow rounded-lg overflow-hidden">
            <thead>
              <tr className="bg-slate-800 text-white">
                <th className="text-left p-3">ID</th>
                <th className="text-left p-3">Inv. Number</th>
                <th className="text-left p-3">Name</th>
                <th className="text-left p-3">Gender</th>
                <th className="text-left p-3">Breed</th>
                <th className="text-left p-3">Arrival Date</th>
                <th className="text-left p-3">Age (months)</th>
                <th className="text-left p-3">Parent</th>
                <th className="text-left p-3">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data?.map((item) => (
                <tr key={item.id} className="border-b hover:bg-gray-50">
                  <td className="p-3">{item.id}</td>
                  <td className="p-3">{item.inventoryNumber}</td>
                  <td className="p-3">{item.name}</td>
                  <td className="p-3">{item.gender}</td>
                  <td className="p-3">{item.breedName}</td>
                  <td className="p-3">
                    {new Date(item.arrivalDate).toLocaleDateString()}
                  </td>
                  <td className="p-3">{item.arrivalAgeMonths}</td>
                  <td className="p-3">{item.parentAnimalName || '—'}</td>
                  <td className="p-3">
                    <div className="flex gap-2">
                      <Link
                        to={`/animals/${item.id}/edit`}
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
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
