/**
 * @fileoverview Modelos TypeScript para la entidad Factura y DetalleFactura.
 * 
 * Define las interfaces para Facturas, incluyendo detalles de líneas,
 * reportes de ventas y estimaciones de próxima compra.
 * 
 * @architectural_layer Shared/Models
 * @pattern Data Transfer Object (DTO)
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @api_alignment
 * Interfaces alineadas con DTOs del backend:
 * - FacturaDto, DetalleFacturaDto
 * - CrearFacturaDto, CrearDetalleFacturaDto
 * - VentasPorProductoDto
 * - ProximaCompraClienteDto
 * 
 * @backend_endpoint
 * GET/POST/DELETE /api/Facturas
 * PATCH /api/Facturas/{id}/estado
 * GET /api/Facturas/ventas-por-producto
 * GET /api/Facturas/proxima-compra/{clienteId}
 */

/**
 * Estados posibles de una factura.
 * Constraint CHECK en base de datos: ('PENDIENTE', 'PAGADA', 'ANULADA')
 */
export type EstadoFactura = 'PENDIENTE' | 'PAGADA' | 'ANULADA';

/**
 * Interface para detalle de línea de factura (response).
 * 
 * @property id - ID del detalle
 * @property productoId - FK al producto
 * @property nombreProducto - Nombre del producto (desnormalizado)
 * @property cantidad - Cantidad vendida
 * @property precioUnitario - Precio al momento de la venta
 * @property subtotal - Calculado: cantidad * precioUnitario
 */
export interface DetalleFacturaDto {
  readonly id: number;
  readonly productoId: number;
  readonly nombreProducto: string;
  readonly cantidad: number;
  readonly precioUnitario: number;
  readonly subtotal: number;
}

/**
 * Interface que representa una Factura completa (response).
 * 
 * @description
 * Incluye todos los detalles de línea y datos del cliente.
 * Los totales (subtotal, impuesto, total) son calculados por el backend.
 * 
 * @property numeroFactura - Formato: FAC-YYYYMMDDNNN
 * @property impuesto - IVA 19% calculado sobre subtotal
 */
export interface Factura {
  readonly id: number;
  readonly numeroFactura: string;
  readonly clienteId: number;
  readonly nombreCliente: string;
  readonly fecha: string;
  readonly subtotal: number;
  readonly impuesto: number;
  readonly total: number;
  readonly estado: EstadoFactura;
  readonly detalles: DetalleFacturaDto[];
}

/**
 * DTO para crear línea de detalle en nueva factura.
 */
export interface CrearDetalleFacturaDto {
  productoId: number;
  cantidad: number;
}

/**
 * DTO para crear una nueva factura.
 * 
 * @description
 * El backend calcula automáticamente:
 * - numeroFactura (generado)
 * - fecha (timestamp actual)
 * - subtotal, impuesto, total (calculados)
 * - estado inicial ('PENDIENTE')
 * 
 * @httpMethod POST
 * @endpoint /api/Facturas
 */
export interface CrearFacturaDto {
  clienteId: number;
  detalles: CrearDetalleFacturaDto[];
}

/**
 * DTO para actualizar estado de factura.
 *
 * @httpMethod PATCH
 * @endpoint /api/Facturas/{id}/estado
 */
export interface ActualizarEstadoFacturaDto {
  estado: EstadoFactura;
}

/**
 * DTO para actualizar línea de detalle de factura.
 */
export interface ActualizarDetalleFacturaDto {
  id?: number | null;
  productoId: number;
  cantidad: number;
}

/**
 * DTO para actualizar una factura existente.
 * Solo se pueden editar facturas en estado PENDIENTE.
 *
 * @httpMethod PUT
 * @endpoint /api/Facturas/{id}
 */
export interface ActualizarFacturaDto {
  clienteId: number;
  detalles: ActualizarDetalleFacturaDto[];
}

/**
 * DTO para reporte de ventas por producto.
 * 
 * @description
 * Resultado de agregación SQL con SUM, COUNT y AVG.
 * Filtrado por año fiscal.
 * 
 * @httpMethod GET
 * @endpoint /api/Facturas/ventas-por-producto?anio=2000
 */
export interface VentasPorProductoDto {
  readonly productoId: number;
  readonly codigoProducto: string;
  readonly nombreProducto: string;
  readonly cantidadTotalVendida: number;
  readonly montoTotalVendido: number;
  readonly numeroFacturas: number;
}

/**
 * Estados de predicción de próxima compra.
 */
export type EstadoPrediccion = 'VENCIDA' | 'PRÓXIMA' | 'FUTURA';

/**
 * DTO para estimación de próxima compra de cliente.
 * 
 * @description
 * Usa análisis estadístico del historial de compras.
 * Requiere mínimo 2 compras para calcular promedio.
 * 
 * @httpMethod GET
 * @endpoint /api/Facturas/proxima-compra/{clienteId}
 */
export interface ProximaCompraClienteDto {
  readonly clienteId: number;
  readonly nombreCliente: string;
  readonly totalCompras: number;
  readonly ultimaCompra: string;
  readonly promedioDiasEntreCompras: number;
  readonly proximaCompraEstimada: string;
  readonly estadoPrediccion: EstadoPrediccion;
}

/**
 * Parámetros para consulta de ventas por producto.
 */
export interface VentasPorProductoParams {
  anio: number;
}

