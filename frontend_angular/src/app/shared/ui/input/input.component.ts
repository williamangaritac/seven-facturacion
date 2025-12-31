/**
 * @fileoverview Componente Input wrapper con validación visual.
 * 
 * Proporciona input estilizado con soporte para labels,
 * mensajes de error y estados de validación.
 * 
 * @architectural_layer Shared/UI
 * @pattern Presentational Component, Control Value Accessor
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @example
 * <app-input
 *   label="Correo electrónico"
 *   type="email"
 *   [formControl]="emailControl"
 *   errorMessage="Ingrese un correo válido">
 * </app-input>
 */

import { Component, Input, forwardRef, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';

/**
 * Tipos de input soportados.
 */
export type InputType = 'text' | 'email' | 'password' | 'number' | 'tel' | 'date';

/**
 * Componente de input reutilizable.
 */
@Component({
  selector: 'app-input',
  standalone: true,
  imports: [CommonModule, FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ],
  template: `
    <div class="input-wrapper" [class.has-error]="showError">
      @if (label) {
        <label [for]="inputId" class="input-label">
          {{ label }}
          @if (required) { <span class="required">*</span> }
        </label>
      }
      
      <input
        [id]="inputId"
        [type]="type"
        [placeholder]="placeholder"
        [disabled]="disabled"
        [readonly]="readonly"
        [(ngModel)]="value"
        (ngModelChange)="onValueChange($event)"
        (blur)="onTouched()"
        class="input-field"
        [class.is-invalid]="showError" />
      
      @if (showError && errorMessage) {
        <span class="error-text">{{ errorMessage }}</span>
      }
      
      @if (hint && !showError) {
        <span class="hint-text">{{ hint }}</span>
      }
    </div>
  `,
  styles: [`
    @use '../../../shared/styles/variables' as *;
    @use '../../../shared/styles/mixins' as *;

    .input-wrapper {
      display: flex;
      flex-direction: column;
      gap: $spacing-xs;
    }

    .input-label {
      font-size: $font-size-sm;
      font-weight: $font-weight-medium;
      color: $color-text-secondary;

      .required {
        color: $color-danger;
        margin-left: 2px;
      }
    }

    .input-field {
      @include input-base;

      &.is-invalid {
        border-color: $color-danger;
        &:focus {
          box-shadow: 0 0 0 3px rgba($color-danger, 0.1);
        }
      }
    }

    .error-text {
      font-size: $font-size-xs;
      color: $color-danger;
    }

    .hint-text {
      font-size: $font-size-xs;
      color: $color-text-muted;
    }
  `],
})
export class InputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type: InputType = 'text';
  @Input() placeholder = '';
  @Input() hint = '';
  @Input() errorMessage = '';
  @Input() required = false;
  @Input() readonly = false;
  @Input() showError = false;

  disabled = false;
  value = '';
  inputId = `input-${Math.random().toString(36).slice(2, 9)}`;

  onChange: (value: string) => void = () => {};
  onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onValueChange(value: string): void {
    this.value = value;
    this.onChange(value);
  }
}

