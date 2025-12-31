/**
 * @fileoverview Configuración de rutas del módulo de Productos.
 */

import { Routes } from '@angular/router';

export const PRODUCTOS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/productos-list/productos-list.component').then(
        (m) => m.ProductosListComponent
      ),
    title: 'Productos | Seven Facturación',
  },
  {
    path: 'nuevo',
    loadComponent: () =>
      import('./pages/producto-form/producto-form.component').then(
        (m) => m.ProductoFormComponent
      ),
    title: 'Nuevo Producto | Seven Facturación',
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./pages/producto-form/producto-form.component').then(
        (m) => m.ProductoFormComponent
      ),
    title: 'Detalle Producto | Seven Facturación',
  },
  {
    path: ':id/editar',
    loadComponent: () =>
      import('./pages/producto-form/producto-form.component').then(
        (m) => m.ProductoFormComponent
      ),
    title: 'Editar Producto | Seven Facturación',
  },
];

