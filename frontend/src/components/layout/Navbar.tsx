import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';

export default function Navbar() {
  const { user, isAuthenticated, isAdmin, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!isAuthenticated) return null;

  return (
    <nav className="flex items-center justify-between bg-slate-800 px-6 h-15 text-white">
      <Link to="/dashboard" className="text-xl font-bold text-white no-underline">
        AnimalFarm
      </Link>

      <div className="flex gap-4">
        <Link to="/animal-types" className="text-slate-300 hover:text-white no-underline">
          Animal Types
        </Link>
        <Link to="/breeds" className="text-slate-300 hover:text-white no-underline">
          Breeds
        </Link>
        <Link to="/animals" className="text-slate-300 hover:text-white no-underline">
          Animals
        </Link>
        <Link to="/weightings" className="text-slate-300 hover:text-white no-underline">
          Weightings
        </Link>
        {isAdmin && (
          <Link to="/users" className="text-slate-300 hover:text-white no-underline">
            Users
          </Link>
        )}
      </div>

      <div className="flex items-center gap-3">
        <span className="text-slate-300">Hello, {user?.userName}</span>
        <button
          onClick={handleLogout}
          className="border border-slate-300 text-slate-300 px-3 py-1 rounded hover:bg-white/10 cursor-pointer"
        >
          Logout
        </button>
      </div>
    </nav>
  );
}
