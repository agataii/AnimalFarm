export interface LoginRequest {
  userName: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  userName: string;
  roles: string[];
  expiration: string;
}

export interface AuthUser {
  token: string;
  userName: string;
  roles: string[];
}
