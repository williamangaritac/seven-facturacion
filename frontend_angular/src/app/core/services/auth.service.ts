/**
 * @fileoverview Servicio de autenticación.
 * @author Seven Facturación Team
 */

import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { API_CONFIG } from '../config/api.config';
import { LoginRequest, LoginResponse } from '../../shared/models';

const TOKEN_KEY = 'auth_token';
const USERNAME_KEY = 'auth_username';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  
  private readonly apiUrl = `${API_CONFIG.baseUrl}/auth`;
  
  readonly isAuthenticated = signal<boolean>(this.hasToken());
  readonly currentUsername = signal<string | null>(this.getStoredUsername());

  /**
   * Realiza el login del usuario.
   */
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          localStorage.setItem(TOKEN_KEY, response.token);
          localStorage.setItem(USERNAME_KEY, response.username);
          this.isAuthenticated.set(true);
          this.currentUsername.set(response.username);
        })
      );
  }

  /**
   * Cierra la sesión del usuario.
   */
  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USERNAME_KEY);
    this.isAuthenticated.set(false);
    this.currentUsername.set(null);
    this.router.navigate(['/login']);
  }

  /**
   * Obtiene el token almacenado.
   */
  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  /**
   * Verifica si existe un token.
   */
  private hasToken(): boolean {
    return !!this.getToken();
  }

  /**
   * Obtiene el username almacenado.
   */
  private getStoredUsername(): string | null {
    return localStorage.getItem(USERNAME_KEY);
  }
}

