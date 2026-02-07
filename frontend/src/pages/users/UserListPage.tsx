import { useQuery, useMutation } from '@tanstack/react-query';
import { usersApi } from '../../api/usersApi';
import { queryClient } from '../../lib/queryClient';
import type { UserDto } from '../../types/animal';

export default function UserListPage() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ['users'],
    queryFn: usersApi.getAll,
  });

  const activateMutation = useMutation({
    mutationFn: (user: UserDto) =>
      user.isActive ? usersApi.deactivate(user.id) : usersApi.activate(user.id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
  });

  if (isLoading) return <p>Loading...</p>;
  if (isError) return <p className="text-red-600">Failed to load data.</p>;

  return (
    <div>
      <h1 className="text-2xl font-bold mb-5">Users</h1>

      {activateMutation.isError && (
        <div className="bg-red-50 text-red-700 p-3 rounded mb-4">Operation failed.</div>
      )}

      {data && data.length === 0 ? (
        <p className="text-gray-500">No records found.</p>
      ) : (
        <table className="w-full bg-white shadow rounded-lg overflow-hidden">
          <thead>
            <tr className="bg-slate-800 text-white">
              <th className="text-left p-3">Username</th>
              <th className="text-left p-3">Email</th>
              <th className="text-left p-3">Active</th>
              <th className="text-left p-3">Roles</th>
              <th className="text-left p-3">Actions</th>
            </tr>
          </thead>
          <tbody>
            {data?.map((user) => (
              <tr key={user.id} className="border-b hover:bg-gray-50">
                <td className="p-3">{user.userName}</td>
                <td className="p-3">{user.email}</td>
                <td className="p-3">
                  <span
                    className={
                      user.isActive
                        ? 'text-green-600 font-semibold'
                        : 'text-red-600 font-semibold'
                    }
                  >
                    {user.isActive ? 'Yes' : 'No'}
                  </span>
                </td>
                <td className="p-3">{user.roles.join(', ')}</td>
                <td className="p-3">
                  <button
                    onClick={() => activateMutation.mutate(user)}
                    disabled={activateMutation.isPending}
                    className={
                      user.isActive
                        ? 'bg-red-500 text-white px-3 py-1 rounded text-sm hover:bg-red-600 cursor-pointer disabled:opacity-50'
                        : 'bg-green-500 text-white px-3 py-1 rounded text-sm hover:bg-green-600 cursor-pointer disabled:opacity-50'
                    }
                  >
                    {user.isActive ? 'Deactivate' : 'Activate'}
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
