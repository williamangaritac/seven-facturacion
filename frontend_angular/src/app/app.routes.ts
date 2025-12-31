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

/**
 * Rutas principales de la aplicación.
 */
export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'facturacion',
  },
  {
    path: 'facturacion',
    loadChildren: () =>
      import('./features/facturacion/facturacion.routes').then(
        (m) => m.FACTURACION_ROUTES
      ),
  },
  {
    path: 'clientes',
    loadChildren: () =>
      import('./features/clientes/clientes.routes').then(
        (m) => m.CLIENTES_ROUTES
      ),
  },
  {
    path: 'productos',
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
