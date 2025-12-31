/**
 * @fileoverview Componente de detalle de factura.
 * 
 * Muestra información completa de una factura incluyendo
 * detalles de línea y acciones de estado.
 * 
 * @architectural_layer Features/Facturacion/Pages
 * @pattern Smart Component
 * @author Seven Facturación Team
 * @version 1.0.0
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FacturacionService } from '../../services/facturacion.service';
import { Factura, EstadoFactura } from '../../../../shared/models';

/**
 * Componente de detalle de factura.
 */
@Component({
  selector: 'app-factura-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './factura-detail.component.html',
  styleUrl: './factura-detail.component.scss',
})
export class FacturaDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly facturacionService = inject(FacturacionService);

  readonly factura = signal<Factura | null>(null);
  readonly isLoading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.cargarFactura(id);
  }

  /**
   * Carga factura por ID.
   */
  private cargarFactura(id: number): void {
    this.isLoading.set(true);
    
    this.facturacionService.getById(id).subscribe({
      next: (data) => {
        this.factura.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message);
        this.isLoading.set(false);
      },
    });
  }

  /**
   * Cambia estado de la factura.
   */
  cambiarEstado(nuevoEstado: EstadoFactura): void {
    const id = this.factura()?.id;
    if (!id) return;

    this.facturacionService.actualizarEstado(id, { estado: nuevoEstado }).subscribe({
      next: (updated) => this.factura.set(updated),
      error: (err) => this.error.set(err.message),
    });
  }

  /**
   * Retorna clase CSS para badge de estado.
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

