/**
 * @fileoverview Componente de listado de clientes.
 */

import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ClientesService } from '../../services/clientes.service';
import { FacturacionService } from '../../../facturacion/services/facturacion.service';
import { Cliente, ClientePorEdadYCompraDto, ProximaCompraClienteDto } from '../../../../shared/models';

@Component({
  selector: 'app-clientes-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './clientes-list.component.html',
  styleUrl: './clientes-list.component.scss',
})
export class ClientesListComponent implements OnInit {
  private readonly clientesService = inject(ClientesService);
  private readonly facturacionService = inject(FacturacionService);

  readonly clientes = signal<Cliente[]>([]);
  readonly clientesFiltrados = signal<ClientePorEdadYCompraDto[]>([]);
  readonly isLoading = signal<boolean>(true);
  readonly error = signal<string | null>(null);
  readonly mostrandoFiltrados = signal<boolean>(false);

  // Próxima compra
  readonly proximaCompra = signal<ProximaCompraClienteDto | null>(null);
  readonly clienteSeleccionado = signal<number | null>(null);
  readonly cargandoPrediccion = signal<boolean>(false);
  readonly errorPrediccion = signal<string | null>(null);

  // Búsqueda por nombre
  terminoBusqueda: string = '';

  // Filtros
  edadMaxima: number | null = null;
  fechaDesde: string = '';
  fechaHasta: string = '';

  /**
   * Lista de clientes filtrada por término de búsqueda.
   */
  readonly clientesMostrados = computed(() => {
    const clientes = this.clientes();
    const termino = this.terminoBusqueda.toLowerCase().trim();

    if (!termino) {
      return clientes;
    }

    return clientes.filter(cliente =>
      cliente.nombreCompleto.toLowerCase().includes(termino)
    );
  });

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.mostrandoFiltrados.set(false);

    this.clientesService.getAll().subscribe({
      next: (data) => {
        this.clientes.set(data);
        this.clientesFiltrados.set([]);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al cargar clientes');
        this.isLoading.set(false);
      },
    });
  }

  buscarPorCriterios(): void {
    if (!this.edadMaxima || !this.fechaDesde || !this.fechaHasta) {
      this.error.set('Todos los campos son requeridos');
      return;
    }

    this.isLoading.set(true);
    this.error.set(null);

    this.clientesService.getClientesPorEdadYCompra({
      edadMaxima: this.edadMaxima,
      fechaDesde: this.fechaDesde,
      fechaHasta: this.fechaHasta,
    }).subscribe({
      next: (data) => {
        this.clientesFiltrados.set(data);
        this.mostrandoFiltrados.set(true);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al buscar clientes');
        this.isLoading.set(false);
      },
    });
  }

  limpiarFiltros(): void {
    this.edadMaxima = null;
    this.fechaDesde = '';
    this.fechaHasta = '';
    this.cargarClientes();
  }

  /**
   * Obtiene la predicción de próxima compra para un cliente.
   */
  verProximaCompra(clienteId: number): void {
    this.cargandoPrediccion.set(true);
    this.errorPrediccion.set(null);
    this.clienteSeleccionado.set(clienteId);

    this.facturacionService.getProximaCompra(clienteId).subscribe({
      next: (data) => {
        if (data) {
          this.proximaCompra.set(data);
        } else {
          this.errorPrediccion.set('El cliente debe tener al menos 2 compras para calcular la predicción');
        }
        this.cargandoPrediccion.set(false);
      },
      error: (err) => {
        this.errorPrediccion.set(err.message || 'Error al obtener predicción');
        this.cargandoPrediccion.set(false);
      },
    });
  }

  /**
   * Cierra el panel de predicción.
   */
  cerrarPrediccion(): void {
    this.proximaCompra.set(null);
    this.clienteSeleccionado.set(null);
    this.errorPrediccion.set(null);
  }

  /**
   * Obtiene la clase CSS según el estado de predicción.
   */
  getEstadoPrediccionClass(estado: string): string {
    switch (estado) {
      case 'VENCIDA':
        return 'badge badge-danger';
      case 'PRÓXIMA':
        return 'badge badge-warning';
      case 'FUTURA':
        return 'badge badge-success';
      default:
        return 'badge';
    }
  }

  eliminarCliente(id: number): void {
    if (confirm('¿Está seguro de eliminar este cliente?')) {
      this.clientesService.delete(id).subscribe({
        next: () => this.cargarClientes(),
        error: (err) => this.error.set(err.message),
      });
    }
  }

  /**
   * Filtra clientes por nombre (dispara reactividad del computed).
   */
  filtrarPorNombre(): void {
    // El computed clientesMostrados se actualiza automáticamente
    // cuando terminoBusqueda cambia, pero necesitamos forzar
    // la detección de cambios si usamos ngModel
    this.clientes.update(c => [...c]);
  }

  /**
   * Limpia la búsqueda por nombre.
   */
  limpiarBusquedaNombre(): void {
    this.terminoBusqueda = '';
  }

  /**
   * Resalta el término de búsqueda en el texto.
   */
  resaltarBusqueda(texto: string): string {
    const termino = this.terminoBusqueda.trim();
    if (!termino) {
      return texto;
    }

    const regex = new RegExp(`(${this.escapeRegex(termino)})`, 'gi');
    return texto.replace(regex, '<mark>$1</mark>');
  }

  /**
   * Escapa caracteres especiales de regex.
   */
  private escapeRegex(str: string): string {
    return str.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }
}

