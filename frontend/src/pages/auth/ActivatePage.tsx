import { useEffect, useState } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { authApi } from '../../api/authApi';
import { AxiosError } from 'axios';
import type { ApiError } from '../../types/common';

export default function ActivatePage() {
  const [searchParams] = useSearchParams();
  const [message, setMessage] = useState('Activating your account...');
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    const userId = searchParams.get('userId');
    const token = searchParams.get('token');

    if (!userId || !token) {
      setMessage('Invalid activation link.');
      setIsError(true);
      return;
    }

    authApi
      .activate(userId, token)
      .then((res) => {
        setMessage(res.message);
        setIsError(false);
      })
      .catch((err: AxiosError<ApiError>) => {
        setMessage(err.response?.data?.message || 'Activation failed.');
        setIsError(true);
      });
  }, [searchParams]);

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-50">
      <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md text-center">
        <h2 className="text-2xl font-bold mb-6">Account Activation</h2>
        <p className={isError ? 'text-red-600 mb-6' : 'text-green-600 mb-6'}>
          {message}
        </p>
        <Link
          to="/login"
          className="inline-block bg-blue-500 text-white px-6 py-2 rounded hover:bg-blue-600 no-underline"
        >
          Go to Login
        </Link>
      </div>
    </div>
  );
}
