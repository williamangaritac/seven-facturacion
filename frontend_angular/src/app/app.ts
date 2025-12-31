/**
 * @fileoverview Componente raíz de Seven Facturación UI.
 *
 * Define el shell principal de la aplicación con header
 * y router-outlet para navegación.
 *
 * @author Seven Facturación Team
 * @version 1.0.0
 */

import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  readonly authService = inject(AuthService);

  logout(): void {
    this.authService.logout();
  }
}
