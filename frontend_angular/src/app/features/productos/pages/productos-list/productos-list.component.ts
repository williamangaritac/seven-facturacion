/**
 * @fileoverview Componente de listado de productos.
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductosService } from '../../services/productos.service';
import { FacturacionService } from '../../../facturacion/services/facturacion.service';
import { Producto, ProductoBajoStockDto, VentasPorProductoDto } from '../../../../shared/models';

@Component({
  selector: 'app-productos-list',
  standalone: true,
  imports: [CommonModule, RouterModule, CurrencyPipe, FormsModule],
  templateUrl: './productos-list.component.html',
  styleUrl: './productos-list.component.scss',
})
export class ProductosListComponent implements OnInit {
  private readonly productosService = inject(ProductosService);
  private readonly facturacionService = inject(FacturacionService);

  readonly productos = signal<Producto[]>([]);
  readonly productosBajoStock = signal<ProductoBajoStockDto[]>([]);
  readonly ventasPorProducto = signal<VentasPorProductoDto[]>([]);
  readonly isLoading = signal<boolean>(true);
  readonly error = signal<string | null>(null);
  readonly mostrandoStockBajo = signal<boolean>(false);
  readonly mostrandoVentas = signal<boolean>(false);

  // Filtro de año
  anio: number | null = null;

  // Exponer Date para el template
  readonly Date = Date;

  ngOnInit(): void {
    this.cargarProductos();
  }

  cargarProductos(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.mostrandoStockBajo.set(false);
    this.mostrandoVentas.set(false);

    this.productosService.getAll().subscribe({
      next: (data) => {
        this.productos.set(data);
        this.productosBajoStock.set([]);
        this.ventasPorProducto.set([]);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al cargar productos');
        this.isLoading.set(false);
      },
    });
  }

  /**
   * Filtra productos con stock bajo (≤ 5 unidades).
   */
  filtrarStockBajo(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.productosService.getStockBajo(5).subscribe({
      next: (data) => {
        this.productosBajoStock.set(data);
        this.mostrandoStockBajo.set(true);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al cargar productos con stock bajo');
        this.isLoading.set(false);
      },
    });
  }

  /**
   * Busca ventas por producto en un año específico.
   */
  buscarVentasPorAnio(): void {
    if (!this.anio) {
      this.error.set('El año es requerido');
      return;
    }

    if (this.anio < 1900 || this.anio > new Date().getFullYear()) {
      this.error.set('Ingrese un año válido');
      return;
    }

    this.isLoading.set(true);
    this.error.set(null);

    this.facturacionService.getVentasPorProducto(this.anio).subscribe({
      next: (data) => {
        this.ventasPorProducto.set(data);
        this.mostrandoVentas.set(true);
        this.mostrandoStockBajo.set(false);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al buscar ventas por producto');
        this.isLoading.set(false);
      },
    });
  }

  /**
   * Limpia el filtro de ventas y vuelve a la vista normal.
   */
  limpiarFiltroVentas(): void {
    this.anio = null;
    this.cargarProductos();
  }

  eliminarProducto(id: number): void {
    if (confirm('¿Está seguro de eliminar este producto?')) {
      this.productosService.delete(id).subscribe({
        next: () => this.cargarProductos(),
        error: (err) => this.error.set(err.message),
      });
    }
  }

  getStockClass(producto: Producto): string {
    if (producto.estaAgotado) return 'badge badge-danger';
    if (producto.tieneStockBajo) return 'badge badge-warning';
    return 'badge badge-success';
  }
}

