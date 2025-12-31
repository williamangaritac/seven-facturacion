/**
 * @fileoverview Componente Button wrapper estilo Odoo.
 * 
 * Encapsula estilos y comportamientos de botón para mantener
 * consistencia visual en toda la aplicación.
 * 
 * @architectural_layer Shared/UI
 * @pattern Presentational Component, Wrapper Pattern
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @description
 * Este componente envuelve la funcionalidad de botón proporcionando:
 * - Variantes de estilo (primary, secondary, danger, success)
 * - Tamaños (sm, md, lg)
 * - Estado de loading con spinner
 * - Soporte para iconos
 * 
 * @accessibility
 * - Incluye roles ARIA apropiados
 * - Estados focus visibles
 * - Soporte para disabled
 * 
 * @example
 * <app-button variant="primary" (clicked)="onSave()">
 *   Guardar
 * </app-button>
 * 
 * <app-button variant="danger" [loading]="isDeleting">
 *   Eliminar
 * </app-button>
 */

import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Variantes visuales del botón.
 */
export type ButtonVariant = 'primary' | 'secondary' | 'danger' | 'success' | 'ghost';

/**
 * Tamaños disponibles.
 */
export type ButtonSize = 'sm' | 'md' | 'lg';

/**
 * Tipos HTML de botón.
 */
export type ButtonType = 'button' | 'submit' | 'reset';

/**
 * Componente de botón reutilizable.
 */
@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <button
      [type]="type"
      [disabled]="disabled || loading"
      [class]="buttonClasses"
      (click)="onClick($event)">
      @if (loading) {
        <span class="spinner"></span>
      }
      <ng-content />
    </button>
  `,
  styles: [`
    @use '../../../shared/styles/variables' as *;
    @use '../../../shared/styles/mixins' as *;

    :host {
      display: inline-block;
    }

    button {
      @include button-base;

      &.btn-primary {
        @include button-primary;
      }

      &.btn-secondary {
        @include button-secondary;
      }

      &.btn-danger {
        background-color: $color-danger;
        color: white;
        &:hover:not(:disabled) {
          background-color: $color-primary-dark;
        }
      }

      &.btn-success {
        background-color: $color-success;
        color: white;
        &:hover:not(:disabled) {
          background-color: $color-primary-dark;
        }
      }

      &.btn-ghost {
        background: transparent;
        color: $color-primary;
        &:hover:not(:disabled) {
          background-color: $color-bg-hover;
        }
      }

      &.btn-sm { padding: $spacing-xs $spacing-sm; font-size: $font-size-xs; }
      &.btn-lg { padding: $spacing-md $spacing-lg; font-size: $font-size-base; }
    }

    .spinner {
      width: 16px;
      height: 16px;
      border: 2px solid currentColor;
      border-top-color: transparent;
      border-radius: 50%;
      animation: spin 0.8s linear infinite;
      margin-right: $spacing-xs;
    }
  `],
})
export class ButtonComponent {
  @Input() variant: ButtonVariant = 'primary';
  @Input() size: ButtonSize = 'md';
  @Input() type: ButtonType = 'button';
  @Input() disabled = false;
  @Input() loading = false;

  @Output() clicked = new EventEmitter<MouseEvent>();

  get buttonClasses(): string {
    return `btn-${this.variant} btn-${this.size}`;
  }

  onClick(event: MouseEvent): void {
    if (!this.disabled && !this.loading) {
      this.clicked.emit(event);
    }
  }
}

