/**
 * @fileoverview Servicio de Clientes para gestión completa de clientes.
 */

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { API_CONFIG } from '../../../core/config/api.config';
import {
  Cliente,
  CrearClienteDto,
  ActualizarClienteDto,
  ClientePorEdadYCompraDto,
  ClientePorEdadYCompraParams,
} from '../../../shared/models';

@Injectable({
  providedIn: 'root',
})
export class ClientesService extends BaseHttpService<Cliente, CrearClienteDto, ActualizarClienteDto> {
  constructor() {
    super(inject(HttpClient), API_CONFIG.endpoints.clientes);
  }

  /**
   * Obtiene clientes filtrados por edad y rango de compra.
   */
  getClientesPorEdadYCompra(params: ClientePorEdadYCompraParams): Observable<ClientePorEdadYCompraDto[]> {
    const httpParams = this.buildParams({
      edadMaxima: params.edadMaxima,
      fechaDesde: params.fechaDesde,
      fechaHasta: params.fechaHasta,
    });
    return this.http.get<ClientePorEdadYCompraDto[]>(
      `${this.apiUrl}/por-edad-y-compra`,
      { params: httpParams }
    );
  }
}

