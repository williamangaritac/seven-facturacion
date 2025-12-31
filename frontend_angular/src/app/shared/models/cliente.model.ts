/**
 * @fileoverview Modelos TypeScript para la entidad Cliente.
 * 
 * Define las interfaces que representan la estructura de datos del Cliente
 * según la especificación del backend .NET 10 (ARCHITECTURE_BACKEND.md).
 * 
 * @architectural_layer Shared/Models
 * @pattern Data Transfer Object (DTO)
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @api_alignment
 * Estas interfaces están alineadas con los DTOs del backend:
 * - ClienteDto (Response)
 * - CrearClienteDto (Request)
 * - ActualizarClienteDto (Request)
 * - ClientePorEdadYCompraDto (Response)
 * 
 * @backend_endpoint
 * GET/POST/PUT/DELETE /api/Clientes
 * GET /api/Clientes/por-edad-y-compra
 */

/**
 * Interface que representa un Cliente en respuestas de la API.
 * 
 * @description
 * Mapea directamente al ClienteDto del backend C#.
 * Incluye propiedades calculadas como `nombreCompleto` y `edad`.
 * 
 * @property id - Identificador único del cliente (PK en base de datos)
 * @property nombre - Nombre del cliente (requerido, máx 100 caracteres)
 * @property apellido - Apellido del cliente (requerido, máx 100 caracteres)
 * @property nombreCompleto - Propiedad calculada: nombre + apellido
 * @property correoElectronico - Email único del cliente (validado con regex)
 * @property telefono - Teléfono de contacto (opcional)
 * @property fechaNacimiento - Fecha de nacimiento en formato ISO (YYYY-MM-DD)
 * @property edad - Propiedad calculada: años desde fecha de nacimiento
 * @property direccion - Dirección física del cliente (opcional)
 * @property activo - Estado activo/inactivo del cliente
 * 
 * @example
 * const cliente: Cliente = {
 *   id: 1,
 *   nombre: 'Carlos',
 *   apellido: 'Martínez',
 *   nombreCompleto: 'Carlos Martínez',
 *   correoElectronico: 'carlos@email.com',
 *   telefono: '3001234567',
 *   fechaNacimiento: '1992-05-15',
 *   edad: 33,
 *   direccion: 'Calle 123',
 *   activo: true
 * };
 */
export interface Cliente {
  readonly id: number;
  readonly nombre: string;
  readonly apellido: string;
  readonly nombreCompleto: string;
  readonly correoElectronico: string;
  readonly telefono: string | null;
  readonly fechaNacimiento: string;
  readonly edad: number;
  readonly direccion: string | null;
  readonly activo: boolean;
}

/**
 * DTO para crear un nuevo cliente.
 * 
 * @description
 * Mapea al CrearClienteDto del backend.
 * No incluye id, nombreCompleto ni edad (calculados por el servidor).
 * 
 * @httpMethod POST
 * @endpoint /api/Clientes
 */
export interface CrearClienteDto {
  nombre: string;
  apellido: string;
  correoElectronico: string;
  telefono?: string | null;
  fechaNacimiento: string;
  direccion?: string | null;
}

/**
 * DTO para actualizar un cliente existente.
 * 
 * @description
 * Idéntico a CrearClienteDto. Todas las propiedades son requeridas
 * para actualización completa (PUT).
 * 
 * @httpMethod PUT
 * @endpoint /api/Clientes/{id}
 */
export interface ActualizarClienteDto {
  nombre: string;
  apellido: string;
  correoElectronico: string;
  telefono?: string | null;
  fechaNacimiento: string;
  direccion?: string | null;
}

/**
 * DTO para resultado de consulta clientes por edad y compra.
 * 
 * @description
 * Resultado del endpoint especial que filtra clientes por:
 * - Edad máxima
 * - Rango de fechas de compra
 * 
 * @httpMethod GET
 * @endpoint /api/Clientes/por-edad-y-compra
 * @queryParams edadMaxima, fechaDesde, fechaHasta
 */
export interface ClientePorEdadYCompraDto {
  readonly clienteId: number;
  readonly nombre: string;
  readonly apellido: string;
  readonly nombreCompleto: string;
  readonly correo: string;
  readonly fechaNacimiento: string;
  readonly edad: number;
  readonly totalComprasPeriodo: number;
}

/**
 * Parámetros de consulta para el endpoint por-edad-y-compra.
 * 
 * @example
 * const params: ClientePorEdadYCompraParams = {
 *   edadMaxima: 35,
 *   fechaDesde: '2000-02-01',
 *   fechaHasta: '2000-05-25'
 * };
 */
export interface ClientePorEdadYCompraParams {
  edadMaxima: number;
  fechaDesde: string;
  fechaHasta: string;
}

