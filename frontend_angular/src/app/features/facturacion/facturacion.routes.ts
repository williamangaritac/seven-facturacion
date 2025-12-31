/**
 * @fileoverview Configuración de rutas del módulo de Facturación.
 * 
 * Define las rutas lazy-loaded para todas las páginas del módulo.
 * Implementa patrón de routing modular de Angular 16+.
 * 
 * @architectural_layer Features/Facturacion
 * @pattern Lazy Loading, Feature Modules
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @routes
 * - /facturacion → Lista de facturas
 * - /facturacion/nueva → Crear factura
 * - /facturacion/:id → Detalle de factura
 * - /facturacion/reportes/ventas → Reporte de ventas
 * 
 * @lazy_loading
 * Cada componente se carga bajo demanda para optimizar
 * el bundle inicial de la aplicación.
 */

import { Routes } from '@angular/router';

/**
 * Rutas del módulo de facturación.
 * 
 * @description
 * Configuración de rutas usando dynamic imports para lazy loading.
 * Cada ruta carga su componente solo cuando se navega a ella.
 * 
 * @example
 * // En app.routes.ts
 * {
 *   path: 'facturacion',
 *   loadChildren: () => import('./features/facturacion/facturacion.routes')
 *     .then(m => m.FACTURACION_ROUTES)
 * }
 */
export const FACTURACION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/facturas-list/facturas-list.component').then(
        (m) => m.FacturasListComponent
      ),
    title: 'Facturas | Seven Facturación',
  },
  {
    path: 'nueva',
    loadComponent: () =>
      import('./pages/factura-form/factura-form.component').then(
        (m) => m.FacturaFormComponent
      ),
    title: 'Nueva Factura | Seven Facturación',
  },
  {
    path: 'reportes/ventas',
    loadComponent: () =>
      import('./pages/ventas-reporte/ventas-reporte.component').then(
        (m) => m.VentasReporteComponent
      ),
    title: 'Reporte de Ventas | Seven Facturación',
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./pages/factura-detail/factura-detail.component').then(
        (m) => m.FacturaDetailComponent
      ),
    title: 'Detalle Factura | Seven Facturación',
  },
  {
    path: ':id/editar',
    loadComponent: () =>
      import('./pages/factura-form/factura-form.component').then(
        (m) => m.FacturaFormComponent
      ),
    title: 'Editar Factura | Seven Facturación',
  },
];

