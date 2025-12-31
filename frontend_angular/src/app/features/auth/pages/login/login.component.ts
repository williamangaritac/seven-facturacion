/**
 * @fileoverview Componente de login.
 * @author Seven Facturación Team
 */

import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  username = '';
  password = '';
  
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  onSubmit(): void {
    if (!this.username || !this.password) {
      this.error.set('Por favor ingrese usuario y contraseña');
      return;
    }

    this.isLoading.set(true);
    this.error.set(null);

    this.authService.login({ username: this.username, password: this.password })
      .subscribe({
        next: () => {
          this.isLoading.set(false);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.isLoading.set(false);
          this.error.set(err.message || 'Credenciales inválidas');
        }
      });
  }
}

