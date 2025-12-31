/**
 * @fileoverview Componente de formulario para crear/editar productos.
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { ProductosService } from '../../services/productos.service';
import { CrearProductoDto } from '../../../../shared/models';

@Component({
  selector: 'app-producto-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './producto-form.component.html',
  styleUrl: './producto-form.component.scss',
})
export class ProductoFormComponent implements OnInit {
  private readonly productosService = inject(ProductosService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly isLoading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly isEditing = signal<boolean>(false);
  
  productoId: number | null = null;
  
  formData: CrearProductoDto = {
    codigo: '',
    nombre: '',
    descripcion: '',
    precio: 0,
    stock: 0,
  };

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'nuevo') {
      this.productoId = +id;
      this.isEditing.set(true);
      this.cargarProducto();
    }
  }

  cargarProducto(): void {
    if (!this.productoId) return;
    
    this.isLoading.set(true);
    this.productosService.getById(this.productoId).subscribe({
      next: (producto) => {
        this.formData = {
          codigo: producto.codigo,
          nombre: producto.nombre,
          descripcion: producto.descripcion || '',
          precio: producto.precio,
          stock: producto.stock,
        };
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.message);
        this.isLoading.set(false);
      },
    });
  }

  guardar(): void {
    this.isLoading.set(true);
    this.error.set(null);

    const observable = this.isEditing()
      ? this.productosService.update(this.productoId!, this.formData)
      : this.productosService.create(this.formData);

    observable.subscribe({
      next: () => {
        this.router.navigate(['/productos']);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al guardar producto');
        this.isLoading.set(false);
      },
    });
  }
}

