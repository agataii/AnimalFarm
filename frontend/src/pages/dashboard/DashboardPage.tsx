import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';

const cards = [
  { to: '/animal-types', title: 'Animal Types', desc: 'Manage animal type classifications' },
  { to: '/breeds', title: 'Breeds', desc: 'Manage animal breeds' },
  { to: '/animals', title: 'Animals', desc: 'Manage your animals inventory' },
  { to: '/weightings', title: 'Weightings', desc: 'Track animal weight measurements' },
];

export default function DashboardPage() {
  const { user, isAdmin } = useAuth();

  return (
    <div>
      <h1 className="text-2xl font-bold mb-6">Welcome, {user?.userName}!</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {cards.map((card) => (
          <Link
            key={card.to}
            to={card.to}
            className="bg-white p-6 rounded-lg shadow hover:shadow-lg hover:-translate-y-0.5 transition no-underline text-gray-800"
          >
            <h3 className="text-lg font-semibold text-slate-800 mb-2">{card.title}</h3>
            <p className="text-gray-500">{card.desc}</p>
          </Link>
        ))}
        {isAdmin && (
          <Link
            to="/users"
            className="bg-white p-6 rounded-lg shadow hover:shadow-lg hover:-translate-y-0.5 transition no-underline text-gray-800"
          >
            <h3 className="text-lg font-semibold text-slate-800 mb-2">Users</h3>
            <p className="text-gray-500">Manage user accounts</p>
          </Link>
        )}
      </div>
    </div>
  );
}
