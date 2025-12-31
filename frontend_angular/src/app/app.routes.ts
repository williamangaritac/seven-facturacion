/**
 * @fileoverview Configuración de rutas principales de la aplicación.
 *
 * Define las rutas de nivel superior con lazy loading para
 * cada módulo de features.
 *
 * @architectural_layer App/Routing
 * @pattern Lazy Loading, Feature Modules
 * @author Seven Facturación Team
 * @version 1.0.0
 *
 * @routes
 * - / → Redirect a /facturacion
 * - /facturacion → Módulo de facturación (lazy loaded)
 *
 * @performance
 * Lazy loading reduce el bundle inicial significativamente.
 * Cada feature se carga solo cuando se navega a ella.
 */

import { Routes } from '@angular/router';
import { authGuard } from './core';

/**
 * Rutas principales de la aplicación.
 */
export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login.component').then(
        (m) => m.LoginComponent
      ),
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'facturacion',
  },
  {
    path: 'facturacion',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/facturacion/facturacion.routes').then(
        (m) => m.FACTURACION_ROUTES
      ),
  },
  {
    path: 'clientes',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/clientes/clientes.routes').then(
        (m) => m.CLIENTES_ROUTES
      ),
  },
  {
    path: 'productos',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/productos/productos.routes').then(
        (m) => m.PRODUCTOS_ROUTES
      ),
  },
  {
    path: '**',
    redirectTo: 'facturacion',
  },
];
