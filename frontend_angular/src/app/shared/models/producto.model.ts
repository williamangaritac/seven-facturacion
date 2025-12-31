/**
 * @fileoverview Modelos TypeScript para la entidad Producto.
 * 
 * Define las interfaces que representan la estructura de datos del Producto
 * según la especificación del backend .NET 10 (ARCHITECTURE_BACKEND.md).
 * 
 * @architectural_layer Shared/Models
 * @pattern Data Transfer Object (DTO)
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @api_alignment
 * Estas interfaces están alineadas con los DTOs del backend:
 * - ProductoDto (Response)
 * - CrearProductoDto (Request)
 * - ActualizarProductoDto (Request)
 * - ListaPrecioDto (Response)
 * - ProductoBajoStockDto (Response)
 * 
 * @backend_endpoint
 * GET/POST/PUT/DELETE /api/Productos
 * GET /api/Productos/lista-precios
 * GET /api/Productos/stock-bajo
 */

/**
 * Interface que representa un Producto en respuestas de la API.
 * 
 * @description
 * Mapea directamente al ProductoDto del backend C#.
 * Incluye propiedades calculadas como `tieneStockBajo` y `estaAgotado`.
 * 
 * @property id - Identificador único del producto (PK)
 * @property codigo - Código SKU único del producto
 * @property nombre - Nombre del producto (máx 200 caracteres)
 * @property descripcion - Descripción detallada (opcional)
 * @property precio - Precio unitario (DECIMAL 18,2)
 * @property stock - Cantidad disponible en inventario
 * @property activo - Estado activo/inactivo del producto
 * @property tieneStockBajo - Calculado: stock <= 5
 * @property estaAgotado - Calculado: stock === 0
 * 
 * @example
 * const producto: Producto = {
 *   id: 1,
 *   codigo: 'PROD-001',
 *   nombre: 'Laptop HP ProBook',
 *   descripcion: 'Laptop empresarial 15.6"',
 *   precio: 2500000.00,
 *   stock: 15,
 *   activo: true,
 *   tieneStockBajo: false,
 *   estaAgotado: false
 * };
 */
export interface Producto {
  readonly id: number;
  readonly codigo: string;
  readonly nombre: string;
  readonly descripcion: string | null;
  readonly precio: number;
  readonly stock: number;
  readonly activo: boolean;
  readonly tieneStockBajo: boolean;
  readonly estaAgotado: boolean;
  readonly anioUltimaVenta: number | null;
}

/**
 * DTO para crear un nuevo producto.
 * 
 * @httpMethod POST
 * @endpoint /api/Productos
 */
export interface CrearProductoDto {
  codigo: string;
  nombre: string;
  descripcion?: string | null;
  precio: number;
  stock: number;
}

/**
 * DTO para actualizar un producto existente.
 * 
 * @httpMethod PUT
 * @endpoint /api/Productos/{id}
 */
export interface ActualizarProductoDto {
  codigo: string;
  nombre: string;
  descripcion?: string | null;
  precio: number;
  stock: number;
}

/**
 * DTO para lista de precios de productos activos.
 *
 * @description
 * Versión simplificada de Producto para mostrar precios.
 * Solo incluye información esencial para catálogos.
 *
 * @httpMethod GET
 * @endpoint /api/Productos/lista-precios
 */
export interface ListaPrecioDto {
  readonly id: number;
  readonly codigo: string;
  readonly nombre: string;
  readonly precio: number;
}

/**
 * Niveles de alerta para stock bajo.
 * Mapea a los valores del CASE en el backend.
 */
export type NivelAlertaStock = 'AGOTADO' | 'CRÍTICO' | 'BAJO';

/**
 * DTO para productos con stock bajo.
 * 
 * @description
 * Usado para alertas de inventario y reabastecimiento.
 * Incluye nivel de alerta calculado por el backend.
 * 
 * @httpMethod GET
 * @endpoint /api/Productos/stock-bajo?stockMinimo=5
 */
export interface ProductoBajoStockDto {
  readonly productoId: number;
  readonly codigo: string;
  readonly nombre: string;
  readonly stock: number;
  readonly precio: number;
  readonly nivelAlerta: NivelAlertaStock;
}

/**
 * Parámetros para consulta de stock bajo.
 */
export interface StockBajoParams {
  stockMinimo?: number;
}

