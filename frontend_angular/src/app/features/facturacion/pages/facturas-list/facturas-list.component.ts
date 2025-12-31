/**
 * @fileoverview Componente de listado de facturas con DevExtreme DataGrid.
 * 
 * Página principal del módulo de facturación que muestra todas las facturas
 * en un grid interactivo con filtros, ordenamiento y paginación.
 * 
 * @architectural_layer Features/Facturacion/Pages
 * @pattern Smart Component (Container Component)
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @component_type
 * Smart Component: Maneja lógica de negocio y estado.
 * Coordina comunicación entre servicio y UI.
 * 
 * @devextreme
 * Utiliza DxDataGridComponent para visualización tabular.
 * Configuración Odoo-like con colores corporativos.
 * 
 * @features
 * - Listado paginado de facturas
 * - Filtros por estado, cliente, fecha
 * - Ordenamiento por columnas
 * - Acciones: Ver detalle, cambiar estado
 * - Export a Excel/PDF
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FacturacionService } from '../../services/facturacion.service';
import { Factura, EstadoFactura } from '../../../../shared/models';

/**
 * Componente de listado de facturas.
 * 
 * @description
 * Implementa patrón Standalone Component de Angular 16+.
 * Usa Signals para gestión de estado reactivo.
 * 
 * @standalone
 * @selector app-facturas-list
 * 
 * @example
 * <app-facturas-list />
 */
@Component({
  selector: 'app-facturas-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './facturas-list.component.html',
  styleUrl: './facturas-list.component.scss',
})
export class FacturasListComponent implements OnInit {
  /**
   * Servicio de facturación inyectado.
   * Usa inject() function para mejor tree-shaking.
   */
  private readonly facturacionService = inject(FacturacionService);

  /**
   * Signal con lista de facturas.
   * Signals proporcionan reactividad fine-grained sin RxJS.
   */
  readonly facturas = signal<Factura[]>([]);

  /**
   * Signal de estado de carga.
   */
  readonly isLoading = signal<boolean>(true);

  /**
   * Signal de mensaje de error.
   */
  readonly error = signal<string | null>(null);

  /**
   * Columnas configuradas para el grid.
   * Define estructura visual estilo Odoo.
   */
  readonly columnas = [
    { field: 'numeroFactura', header: 'Número', width: 150 },
    { field: 'nombreCliente', header: 'Cliente', width: 200 },
    { field: 'fecha', header: 'Fecha', width: 120, type: 'date' },
    { field: 'subtotal', header: 'Subtotal', width: 120, type: 'currency' },
    { field: 'impuesto', header: 'IVA 19%', width: 100, type: 'currency' },
    { field: 'total', header: 'Total', width: 120, type: 'currency' },
    { field: 'estado', header: 'Estado', width: 100 },
  ];

  /**
   * Lifecycle hook - Inicialización.
   * Carga inicial de facturas.
   */
  ngOnInit(): void {
    this.cargarFacturas();
  }

  /**
   * Carga todas las facturas desde el backend.
   * 
   * @async
   * @emits facturas - Actualiza signal con datos
   * @emits isLoading - false cuando completa
   * @emits error - mensaje si falla
   */
  cargarFacturas(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.facturacionService.getAll().subscribe({
      next: (data) => {
        this.facturas.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al cargar facturas');
        this.isLoading.set(false);
      },
    });
  }

  /**
   * Cambia el estado de una factura.
   * 
   * @param id - ID de la factura
   * @param nuevoEstado - Estado destino
   */
  cambiarEstado(id: number, nuevoEstado: EstadoFactura): void {
    this.facturacionService.actualizarEstado(id, { estado: nuevoEstado }).subscribe({
      next: () => this.cargarFacturas(),
      error: (err) => this.error.set(err.message),
    });
  }

  /**
   * Obtiene clase CSS para badge de estado.
   * Estilo Odoo con colores semánticos.
   * 
   * @param estado - Estado de la factura
   * @returns Clase CSS para el badge
   */
  getEstadoClass(estado: EstadoFactura): string {
    const clases: Record<EstadoFactura, string> = {
      PENDIENTE: 'badge-warning',
      PAGADA: 'badge-success',
      ANULADA: 'badge-danger',
    };
    return clases[estado];
  }
}

