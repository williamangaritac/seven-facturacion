/**
 * @fileoverview Servicio de Productos para gestión completa de productos.
 */

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { API_CONFIG } from '../../../core/config/api.config';
import {
  Producto,
  CrearProductoDto,
  ActualizarProductoDto,
  ListaPrecioDto,
  ProductoBajoStockDto,
} from '../../../shared/models';

@Injectable({
  providedIn: 'root',
})
export class ProductosService extends BaseHttpService<Producto, CrearProductoDto, ActualizarProductoDto> {
  constructor() {
    super(inject(HttpClient), API_CONFIG.endpoints.productos);
  }

  /**
   * Obtiene lista de precios de productos activos.
   */
  getListaPrecios(): Observable<ListaPrecioDto[]> {
    return this.http.get<ListaPrecioDto[]>(`${this.apiUrl}/lista-precios`);
  }

  /**
   * Obtiene productos con stock bajo.
   */
  getStockBajo(stockMinimo: number = 5): Observable<ProductoBajoStockDto[]> {
    const params = this.buildParams({ stockMinimo });
    return this.http.get<ProductoBajoStockDto[]>(
      `${this.apiUrl}/stock-bajo`,
      { params }
    );
  }
}

