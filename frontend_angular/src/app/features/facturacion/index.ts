/**
 * @fileoverview Barrel export del módulo de Facturación.
 * 
 * Centraliza exportaciones del feature de facturación para
 * importaciones limpias desde otros módulos.
 * 
 * @architectural_layer Features/Facturacion
 * @pattern Barrel Export
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @usage
 * import { FacturacionService, FACTURACION_ROUTES } from '@features/facturacion';
 */

// Rutas
export { FACTURACION_ROUTES } from './facturacion.routes';

// Servicios
export { FacturacionService } from './services/facturacion.service';

// Páginas (lazy loaded, pero exportables para tests)
export { FacturasListComponent } from './pages/facturas-list/facturas-list.component';
export { FacturaFormComponent } from './pages/factura-form/factura-form.component';
export { FacturaDetailComponent } from './pages/factura-detail/factura-detail.component';
export { VentasReporteComponent } from './pages/ventas-reporte/ventas-reporte.component';

