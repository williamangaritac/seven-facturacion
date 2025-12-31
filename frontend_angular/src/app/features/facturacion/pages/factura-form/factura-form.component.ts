/**
 * @fileoverview Componente de formulario para crear/editar facturas.
 * 
 * Implementa formulario reactivo con validaciones y selección
 * de productos con cálculo automático de totales.
 * 
 * @architectural_layer Features/Facturacion/Pages
 * @pattern Smart Component, Reactive Forms
 * @author Seven Facturación Team
 * @version 1.0.0
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormArray, Validators } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FacturacionService } from '../../services/facturacion.service';
import { ClientesService } from '../../../clientes/services/clientes.service';
import { ProductosService } from '../../../productos/services/productos.service';
import {
  CrearFacturaDto,
  ActualizarFacturaDto,
  ActualizarDetalleFacturaDto,
  Cliente,
  ListaPrecioDto,
  Factura
} from '../../../../shared/models';

/**
 * Componente para creación de facturas.
 */
@Component({
  selector: 'app-factura-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './factura-form.component.html',
  styleUrl: './factura-form.component.scss',
})
export class FacturaFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly facturacionService = inject(FacturacionService);
  private readonly clientesService = inject(ClientesService);
  private readonly productosService = inject(ProductosService);

  readonly isSubmitting = signal(false);
  readonly isLoading = signal(true);
  readonly error = signal<string | null>(null);
  readonly clientes = signal<Cliente[]>([]);
  readonly productos = signal<ListaPrecioDto[]>([]);

  /** Indica si estamos en modo edición */
  readonly isEditMode = signal(false);
  /** ID de la factura en modo edición */
  readonly facturaId = signal<number | null>(null);
  /** Factura cargada en modo edición */
  readonly facturaActual = signal<Factura | null>(null);
  /** IDs de detalles existentes para enviar al backend */
  private detalleIds: (number | null)[] = [];

  /**
   * Formulario reactivo de factura.
   */
  readonly facturaForm = this.fb.group({
    clienteId: [null as number | null, [Validators.required]],
    detalles: this.fb.array<ReturnType<typeof this.crearDetalleFormGroup>>([]),
  });

  /**
   * Getter para FormArray de detalles.
   */
  get detalles(): FormArray {
    return this.facturaForm.get('detalles') as FormArray;
  }

  ngOnInit(): void {
    // Detectar modo edición por parámetro de ruta
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode.set(true);
      this.facturaId.set(Number(idParam));
    }
    this.cargarDatos();
  }

  /**
   * Carga clientes y productos para los dropdowns.
   * En modo edición, también carga la factura existente.
   */
  private cargarDatos(): void {
    this.isLoading.set(true);

    // Cargar clientes y productos en paralelo
    Promise.all([
      this.clientesService.getAll().toPromise(),
      this.productosService.getListaPrecios().toPromise()
    ]).then(([clientes, productos]) => {
      this.clientes.set(clientes || []);
      this.productos.set(productos || []);

      if (this.isEditMode() && this.facturaId()) {
        // Modo edición: cargar factura existente
        this.cargarFactura(this.facturaId()!);
      } else {
        // Modo creación: agregar primera línea vacía
        this.isLoading.set(false);
        this.agregarDetalle();
      }
    }).catch(err => {
      this.error.set('Error al cargar datos: ' + err.message);
      this.isLoading.set(false);
    });
  }

  /**
   * Carga una factura existente para edición.
   */
  private cargarFactura(id: number): void {
    this.facturacionService.getById(id).subscribe({
      next: (factura) => {
        // Verificar que la factura sea editable
        if (factura.estado !== 'PENDIENTE') {
          this.error.set('Solo se pueden editar facturas en estado PENDIENTE');
          this.isLoading.set(false);
          return;
        }

        this.facturaActual.set(factura);

        // Poblar el formulario con los datos de la factura
        this.facturaForm.patchValue({
          clienteId: factura.clienteId
        });

        // Limpiar detalles y agregar los existentes
        this.detalles.clear();
        this.detalleIds = [];

        factura.detalles.forEach(detalle => {
          const detalleGroup = this.crearDetalleFormGroup();
          detalleGroup.patchValue({
            productoId: detalle.productoId,
            cantidad: detalle.cantidad
          });
          this.detalles.push(detalleGroup);
          this.detalleIds.push(detalle.id);
        });

        // Si no hay detalles, agregar uno vacío
        if (this.detalles.length === 0) {
          this.agregarDetalle();
        }

        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set('Error al cargar la factura: ' + (err.error?.message || err.message));
        this.isLoading.set(false);
      }
    });
  }

  /**
   * Crea FormGroup para línea de detalle.
   */
  private crearDetalleFormGroup() {
    return this.fb.group({
      productoId: ['' as string | number, [Validators.required]],
      cantidad: [1, [Validators.required, Validators.min(1)]],
    });
  }

  /**
   * Agrega nueva línea de detalle.
   */
  agregarDetalle(): void {
    this.detalles.push(this.crearDetalleFormGroup());
    this.detalleIds.push(null); // Nuevo detalle sin ID
  }

  /**
   * Elimina línea de detalle por índice.
   */
  eliminarDetalle(index: number): void {
    if (this.detalles.length > 1) {
      this.detalles.removeAt(index);
      this.detalleIds.splice(index, 1);
    }
  }

  /**
   * Maneja el cambio de producto en un detalle.
   */
  onProductoChange(event: Event, index: number): void {
    const select = event.target as HTMLSelectElement;
    const value = select.value ? Number(select.value) : null;
    this.detalles.at(index).get('productoId')?.setValue(value);
    console.log(`Producto cambiado en índice ${index}: ${value}`);
  }

  /**
   * Maneja el cambio de cantidad en un detalle.
   */
  onCantidadChange(event: Event, index: number): void {
    const input = event.target as HTMLInputElement;
    const value = input.value ? Number(input.value) : 0;
    this.detalles.at(index).get('cantidad')?.setValue(value);
    console.log(`Cantidad cambiada en índice ${index}: ${value}`);
  }

  /**
   * Envía el formulario al backend.
   */
  onSubmit(): void {
    // Limpiar errores previos
    this.error.set(null);

    // Obtener valores actuales del formulario
    const clienteId = this.facturaForm.get('clienteId')?.value;

    // Validación manual más explícita
    const errores: string[] = [];

    if (!clienteId) {
      errores.push('Debe seleccionar un cliente');
    }

    // Construir detalles válidos - leer directamente del FormArray
    const detalles: ActualizarDetalleFacturaDto[] = [];
    for (let i = 0; i < this.detalles.length; i++) {
      const control = this.detalles.at(i);
      const productoIdRaw = control.get('productoId')?.value;
      const cantidadRaw = control.get('cantidad')?.value;

      // Convertir a números, manejando strings vacíos
      const productoId = productoIdRaw !== null && productoIdRaw !== undefined && productoIdRaw !== ''
        ? Number(productoIdRaw)
        : null;
      const cantidad = cantidadRaw ? Number(cantidadRaw) : 0;

      console.log(`Detalle ${i}: productoIdRaw=${productoIdRaw}, productoId=${productoId}, cantidad=${cantidad}`);

      if (productoId !== null && !isNaN(productoId) && productoId > 0 && cantidad > 0) {
        detalles.push({
          id: this.detalleIds[i] ?? null, // ID existente o null para nuevos
          productoId: productoId,
          cantidad: cantidad
        });
      }
    }

    console.log('Detalles válidos:', detalles);

    if (detalles.length === 0) {
      errores.push('Debe agregar al menos un producto válido');
    }

    if (errores.length > 0) {
      this.facturaForm.markAllAsTouched();
      this.error.set(errores.join('. '));
      return;
    }

    this.isSubmitting.set(true);

    if (this.isEditMode() && this.facturaId()) {
      // Modo edición: usar update
      const dto: ActualizarFacturaDto = {
        clienteId: Number(clienteId),
        detalles: detalles,
      };

      console.log('Actualizando factura:', dto);

      this.facturacionService.update(this.facturaId()!, dto).subscribe({
        next: (factura) => {
          this.router.navigate(['/facturacion', factura.id]);
        },
        error: (err) => {
          const errorMessage = err.error?.message || err.error?.title || err.message || 'Error al actualizar la factura';
          this.error.set(errorMessage);
          this.isSubmitting.set(false);
        },
      });
    } else {
      // Modo creación: usar create
      const dto: CrearFacturaDto = {
        clienteId: Number(clienteId),
        detalles: detalles.map(d => ({ productoId: d.productoId, cantidad: d.cantidad })),
      };

      console.log('Creando factura:', dto);

      this.facturacionService.create(dto).subscribe({
        next: (factura) => {
          this.router.navigate(['/facturacion', factura.id]);
        },
        error: (err) => {
          const errorMessage = err.error?.message || err.error?.title || err.message || 'Error al crear la factura';
          this.error.set(errorMessage);
          this.isSubmitting.set(false);
        },
      });
    }
  }

  /**
   * Cancela y vuelve al listado.
   */
  cancelar(): void {
    this.router.navigate(['/facturacion']);
  }
}

