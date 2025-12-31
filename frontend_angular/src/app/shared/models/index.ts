/**
 * @fileoverview Barrel export para modelos del dominio.
 * 
 * Centraliza todas las interfaces y tipos del dominio de negocio
 * para importaciones limpias desde cualquier parte de la aplicación.
 * 
 * @architectural_layer Shared/Models
 * @pattern Barrel Export
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @usage
 * import { Cliente, Producto, Factura } from '@shared/models';
 * import type { CrearClienteDto, EstadoFactura } from '@shared/models';
 */

// Cliente models
export type {
  Cliente,
  CrearClienteDto,
  ActualizarClienteDto,
  ClientePorEdadYCompraDto,
  ClientePorEdadYCompraParams,
} from './cliente.model';

// Producto models
export type {
  Producto,
  CrearProductoDto,
  ActualizarProductoDto,
  ListaPrecioDto,
  ProductoBajoStockDto,
  StockBajoParams,
  NivelAlertaStock,
} from './producto.model';

// Factura models
export type {
  Factura,
  DetalleFacturaDto,
  CrearFacturaDto,
  CrearDetalleFacturaDto,
  ActualizarFacturaDto,
  ActualizarDetalleFacturaDto,
  ActualizarEstadoFacturaDto,
  VentasPorProductoDto,
  ProximaCompraClienteDto,
  VentasPorProductoParams,
  EstadoFactura,
  EstadoPrediccion,
} from './factura.model';

// Auth models
export type {
  LoginRequest,
  LoginResponse,
} from './auth.model';

