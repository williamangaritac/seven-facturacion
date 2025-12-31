/**
 * @fileoverview Componente de reporte de ventas por producto.
 *
 * Muestra estadísticas de ventas agrupadas por producto
 * para un año específico, y estimación de próxima compra por cliente.
 *
 * @architectural_layer Features/Facturacion/Pages
 * @pattern Smart Component
 * @author Seven Facturación Team
 * @version 1.0.0
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FacturacionService } from '../../services/facturacion.service';
import { ClientesService } from '../../../clientes/services/clientes.service';
import { VentasPorProductoDto, ProximaCompraClienteDto, Cliente } from '../../../../shared/models';

/**
 * Componente de reporte de ventas.
 */
@Component({
  selector: 'app-ventas-reporte',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './ventas-reporte.component.html',
  styleUrl: './ventas-reporte.component.scss',
})
export class VentasReporteComponent implements OnInit {
  private readonly facturacionService = inject(FacturacionService);
  private readonly clientesService = inject(ClientesService);

  // ========== Reporte de Ventas ==========
  readonly ventas = signal<VentasPorProductoDto[]>([]);
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  anioSeleccionado = new Date().getFullYear();
  readonly aniosDisponibles = [2000, 2023, 2024, 2025];

  // ========== Próxima Compra ==========
  readonly clientes = signal<Cliente[]>([]);
  readonly clientesFiltrados = signal<Cliente[]>([]);
  readonly clienteSeleccionadoId = signal<number | null>(null);
  readonly proximaCompra = signal<ProximaCompraClienteDto | null>(null);
  readonly isLoadingProximaCompra = signal(false);
  readonly errorProximaCompra = signal<string | null>(null);
  readonly proximaCompraConsultada = signal(false);

  /** Término de búsqueda para filtrar clientes */
  busquedaCliente = '';

  ngOnInit(): void {
    this.cargarReporte();
    this.cargarClientes();
  }

  /**
   * Carga la lista de clientes para el selector.
   */
  private cargarClientes(): void {
    this.clientesService.getAll().subscribe({
      next: (clientes) => {
        this.clientes.set(clientes);
        this.clientesFiltrados.set(clientes);
      },
      error: (err) => console.error('Error cargando clientes:', err)
    });
  }

  /**
   * Filtra clientes por nombre o teléfono.
   */
  onBusquedaChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.busquedaCliente = input.value.toLowerCase().trim();

    if (!this.busquedaCliente) {
      this.clientesFiltrados.set(this.clientes());
      return;
    }

    const filtrados = this.clientes().filter(cliente =>
      cliente.nombreCompleto.toLowerCase().includes(this.busquedaCliente) ||
      cliente.telefono?.toLowerCase().includes(this.busquedaCliente)
    );
    this.clientesFiltrados.set(filtrados);
  }

  /**
   * Carga reporte de ventas para el año seleccionado.
   */
  cargarReporte(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.facturacionService.getVentasPorProducto(this.anioSeleccionado).subscribe({
      next: (data) => {
        this.ventas.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message);
        this.isLoading.set(false);
      },
    });
  }

  /**
   * Calcula el total general de ventas.
   */
  getTotalVentas(): number {
    return this.ventas().reduce((sum, v) => sum + v.montoTotalVendido, 0);
  }

  /**
   * Cambia el año y recarga el reporte.
   */
  onAnioChange(): void {
    this.cargarReporte();
  }

  // ========== Métodos Próxima Compra ==========

  /**
   * Maneja el cambio de cliente seleccionado.
   */
  onClienteChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const clienteId = select.value ? Number(select.value) : null;
    this.clienteSeleccionadoId.set(clienteId);
    // Limpiar resultados previos
    this.proximaCompra.set(null);
    this.errorProximaCompra.set(null);
    this.proximaCompraConsultada.set(false);
  }

  /**
   * Consulta la estimación de próxima compra para el cliente seleccionado.
   */
  consultarProximaCompra(): void {
    const clienteId = this.clienteSeleccionadoId();
    if (!clienteId) {
      this.errorProximaCompra.set('Por favor seleccione un cliente');
      return;
    }

    this.isLoadingProximaCompra.set(true);
    this.errorProximaCompra.set(null);
    this.proximaCompra.set(null);
    this.proximaCompraConsultada.set(true);

    this.facturacionService.getProximaCompra(clienteId).subscribe({
      next: (resultado) => {
        this.proximaCompra.set(resultado);
        this.isLoadingProximaCompra.set(false);
      },
      error: (err) => {
        const mensaje = err.error?.message || err.error?.title ||
          'El cliente debe tener al menos 2 compras para estimar la próxima';
        this.errorProximaCompra.set(mensaje);
        this.isLoadingProximaCompra.set(false);
      },
    });
  }

  /**
   * Obtiene la clase CSS para el estado de predicción.
   */
  getEstadoPrediccionClass(estado: string): string {
    switch (estado) {
      case 'VENCIDA': return 'estado-vencida';
      case 'PRÓXIMA': return 'estado-proxima';
      case 'FUTURA': return 'estado-futura';
      default: return '';
    }
  }

  /**
   * Obtiene el ícono para el estado de predicción.
   */
  getEstadoPrediccionIcon(estado: string): string {
    switch (estado) {
      case 'VENCIDA': return '⚠️';
      case 'PRÓXIMA': return '🔔';
      case 'FUTURA': return '📅';
      default: return '📊';
    }
  }
}

