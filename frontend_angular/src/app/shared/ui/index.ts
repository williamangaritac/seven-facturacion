/**
 * @fileoverview Barrel export para componentes UI compartidos.
 * 
 * Centraliza las exportaciones de componentes de UI reutilizables
 * para importaciones limpias en toda la aplicación.
 * 
 * @architectural_layer Shared/UI
 * @pattern Barrel Export
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @usage
 * import { ButtonComponent, InputComponent } from '@shared/ui';
 */

export { ButtonComponent } from './button/button.component';
export type { ButtonVariant, ButtonSize, ButtonType } from './button/button.component';
export { InputComponent } from './input/input.component';
export type { InputType } from './input/input.component';

