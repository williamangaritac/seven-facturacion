/**
 * @fileoverview Componente de formulario para crear/editar clientes.
 */

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { ClientesService } from '../../services/clientes.service';
import { CrearClienteDto } from '../../../../shared/models';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './cliente-form.component.html',
  styleUrl: './cliente-form.component.scss',
})
export class ClienteFormComponent implements OnInit {
  private readonly clientesService = inject(ClientesService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly isLoading = signal<boolean>(false);
  readonly error = signal<string | null>(null);
  readonly isEditing = signal<boolean>(false);
  
  clienteId: number | null = null;
  
  formData: CrearClienteDto = {
    nombre: '',
    apellido: '',
    correoElectronico: '',
    telefono: '',
    fechaNacimiento: '',
    direccion: '',
  };

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'nuevo') {
      this.clienteId = +id;
      this.isEditing.set(true);
      this.cargarCliente();
    }
  }

  cargarCliente(): void {
    if (!this.clienteId) return;
    
    this.isLoading.set(true);
    this.clientesService.getById(this.clienteId).subscribe({
      next: (cliente) => {
        this.formData = {
          nombre: cliente.nombre,
          apellido: cliente.apellido,
          correoElectronico: cliente.correoElectronico,
          telefono: cliente.telefono || '',
          fechaNacimiento: cliente.fechaNacimiento.split('T')[0],
          direccion: cliente.direccion || '',
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
      ? this.clientesService.update(this.clienteId!, this.formData)
      : this.clientesService.create(this.formData);

    observable.subscribe({
      next: () => {
        this.router.navigate(['/clientes']);
      },
      error: (err) => {
        this.error.set(err.message || 'Error al guardar cliente');
        this.isLoading.set(false);
      },
    });
  }
}

