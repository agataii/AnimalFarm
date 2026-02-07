import { createBrowserRouter, Navigate } from 'react-router-dom';
import Layout from './components/layout/Layout';
import ProtectedRoute from './components/common/ProtectedRoute';
import AdminRoute from './components/common/AdminRoute';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import ActivatePage from './pages/auth/ActivatePage';
import DashboardPage from './pages/dashboard/DashboardPage';
import AnimalTypeListPage from './pages/animalTypes/AnimalTypeListPage';
import AnimalTypeFormPage from './pages/animalTypes/AnimalTypeFormPage';
import BreedListPage from './pages/breeds/BreedListPage';
import BreedFormPage from './pages/breeds/BreedFormPage';
import AnimalListPage from './pages/animals/AnimalListPage';
import AnimalFormPage from './pages/animals/AnimalFormPage';
import WeightingListPage from './pages/weightings/WeightingListPage';
import WeightingFormPage from './pages/weightings/WeightingFormPage';
import UserListPage from './pages/users/UserListPage';

export const router = createBrowserRouter([
  // Public routes
  { path: '/login', element: <LoginPage /> },
  { path: '/register', element: <RegisterPage /> },
  { path: '/activate', element: <ActivatePage /> },

  // Protected routes
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <Layout />,
        children: [
          { path: '/dashboard', element: <DashboardPage /> },

          { path: '/animal-types', element: <AnimalTypeListPage /> },
          { path: '/animal-types/new', element: <AnimalTypeFormPage /> },
          { path: '/animal-types/:id/edit', element: <AnimalTypeFormPage /> },

          { path: '/breeds', element: <BreedListPage /> },
          { path: '/breeds/new', element: <BreedFormPage /> },
          { path: '/breeds/:id/edit', element: <BreedFormPage /> },

          { path: '/animals', element: <AnimalListPage /> },
          { path: '/animals/new', element: <AnimalFormPage /> },
          { path: '/animals/:id/edit', element: <AnimalFormPage /> },

          { path: '/weightings', element: <WeightingListPage /> },
          { path: '/weightings/new', element: <WeightingFormPage /> },
          { path: '/weightings/:id/edit', element: <WeightingFormPage /> },

          // Admin routes
          {
            element: <AdminRoute />,
            children: [{ path: '/users', element: <UserListPage /> }],
          },
        ],
      },
    ],
  },

  // Catch-all
  { path: '*', element: <Navigate to="/login" replace /> },
]);
