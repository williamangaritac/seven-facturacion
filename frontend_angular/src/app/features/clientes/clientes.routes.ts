/**
 * @fileoverview Configuración de rutas del módulo de Clientes.
 */

import { Routes } from '@angular/router';

export const CLIENTES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/clientes-list/clientes-list.component').then(
        (m) => m.ClientesListComponent
      ),
    title: 'Clientes | Seven Facturación',
  },
  {
    path: 'nuevo',
    loadComponent: () =>
      import('./pages/cliente-form/cliente-form.component').then(
        (m) => m.ClienteFormComponent
      ),
    title: 'Nuevo Cliente | Seven Facturación',
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./pages/cliente-form/cliente-form.component').then(
        (m) => m.ClienteFormComponent
      ),
    title: 'Detalle Cliente | Seven Facturación',
  },
  {
    path: ':id/editar',
    loadComponent: () =>
      import('./pages/cliente-form/cliente-form.component').then(
        (m) => m.ClienteFormComponent
      ),
    title: 'Editar Cliente | Seven Facturación',
  },
];

