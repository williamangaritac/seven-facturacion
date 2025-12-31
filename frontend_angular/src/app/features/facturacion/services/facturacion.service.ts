/**
 * @fileoverview Servicio de Facturación para gestión completa de facturas.
 * 
 * Extiende BaseHttpService proporcionando operaciones CRUD estándar
 * más métodos especializados para reportes y análisis de facturación.
 * 
 * @architectural_layer Features/Facturacion/Services
 * @pattern Repository Pattern, Facade Pattern
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @dependencies
 * - HttpClient: Para comunicación HTTP con backend
 * - BaseHttpService: Operaciones CRUD genéricas
 * - Modelos de Factura: Tipado fuerte
 * 
 * @api_endpoints
 * - GET/POST/DELETE /api/Facturas
 * - PATCH /api/Facturas/{id}/estado
 * - GET /api/Facturas/cliente/{clienteId}
 * - GET /api/Facturas/ventas-por-producto?anio=YYYY
 * - GET /api/Facturas/proxima-compra/{clienteId}
 * 
 * @usage
 * @Component({...})
 * export class FacturasListComponent {
 *   private facturacionService = inject(FacturacionService);
 *   facturas$ = this.facturacionService.getAll();
 * }
 */

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { API_CONFIG } from '../../../core/config/api.config';
import {
  Factura,
  CrearFacturaDto,
  ActualizarFacturaDto,
  ActualizarEstadoFacturaDto,
  VentasPorProductoDto,
  ProximaCompraClienteDto,
} from '../../../shared/models';

/**
 * Servicio para gestión de facturación.
 * 
 * @description
 * Proporciona una API completa para:
 * - CRUD de facturas
 * - Cambio de estados
 * - Reportes de ventas
 * - Predicciones de compra
 * 
 * @injectable
 * Registrado en root para singleton global.
 */
@Injectable({
  providedIn: 'root',
})
export class FacturacionService extends BaseHttpService<Factura, CrearFacturaDto, never> {
  /**
   * Constructor con inyección de HttpClient.
   *
   * @param http - Cliente HTTP de Angular
   */
  constructor() {
    super(inject(HttpClient), API_CONFIG.endpoints.facturas);
  }

  /**
   * Actualiza una factura existente (cliente y detalles).
   * Solo se pueden editar facturas en estado PENDIENTE.
   *
   * @param id - ID de la factura a actualizar
   * @param dto - Datos actualizados de la factura
   * @returns Observable con factura actualizada
   *
   * @httpMethod PUT
   * @endpoint /api/Facturas/{id}
   *
   * @businessRules
   * - Solo facturas PENDIENTES son editables
   * - Se recalculan subtotal, impuesto y total
   * - Se ajusta stock de productos automáticamente
   */
  override update(id: number, dto: ActualizarFacturaDto): Observable<Factura> {
    return this.http.put<Factura>(`${this.apiUrl}/${id}`, dto);
  }

  /**
   * Actualiza el estado de una factura.
   *
   * @param id - ID de la factura
   * @param dto - Nuevo estado
   * @returns Observable con factura actualizada
   *
   * @httpMethod PATCH
   * @endpoint /api/Facturas/{id}/estado
   *
   * @businessRules
   * - PENDIENTE → PAGADA: Permitido
   * - PENDIENTE → ANULADA: Permitido
   * - PAGADA → ANULADA: Permitido (con restricciones)
   * - ANULADA → *: No permitido
   */
  actualizarEstado(id: number, dto: ActualizarEstadoFacturaDto): Observable<Factura> {
    return this.http.patch<Factura>(`${this.apiUrl}/${id}/estado`, dto);
  }

  /**
   * Obtiene todas las facturas de un cliente.
   * 
   * @param clienteId - ID del cliente
   * @returns Observable con array de facturas
   * 
   * @httpMethod GET
   * @endpoint /api/Facturas/cliente/{clienteId}
   */
  getByCliente(clienteId: number): Observable<Factura[]> {
    return this.http.get<Factura[]>(`${this.apiUrl}/cliente/${clienteId}`);
  }

  /**
   * Obtiene reporte de ventas por producto para un año.
   * 
   * @param anio - Año fiscal para el reporte
   * @returns Observable con agregación de ventas por producto
   * 
   * @httpMethod GET
   * @endpoint /api/Facturas/ventas-por-producto?anio=YYYY
   * 
   * @sqlQuery
   * Ejecuta agregación con SUM(cantidad), SUM(subtotal),
   * COUNT(DISTINCT factura_id), AVG(precio_unitario)
   */
  getVentasPorProducto(anio: number): Observable<VentasPorProductoDto[]> {
    const params = this.buildParams({ anio });
    return this.http.get<VentasPorProductoDto[]>(
      `${this.apiUrl}/ventas-por-producto`,
      { params }
    );
  }

  /**
   * Obtiene estimación de próxima compra de un cliente.
   * 
   * @param clienteId - ID del cliente
   * @returns Observable con predicción de compra
   * 
   * @httpMethod GET
   * @endpoint /api/Facturas/proxima-compra/{clienteId}
   * 
   * @requirements
   * Cliente debe tener mínimo 2 compras para calcular promedio.
   * Retorna null si no cumple requisitos.
   */
  getProximaCompra(clienteId: number): Observable<ProximaCompraClienteDto | null> {
    return this.http.get<ProximaCompraClienteDto | null>(
      `${this.apiUrl}/proxima-compra/${clienteId}`
    );
  }
}

