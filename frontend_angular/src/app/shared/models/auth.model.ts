/**
 * @fileoverview Modelos de autenticación.
 * @author Seven Facturación Team
 */

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
}

