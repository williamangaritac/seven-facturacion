/**
 * @fileoverview Barrel export para el módulo Core.
 * 
 * Centraliza las exportaciones del módulo core para facilitar
 * importaciones limpias desde otros módulos de la aplicación.
 * 
 * @architectural_layer Core
 * @pattern Barrel Export (Re-export Pattern)
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @usage
 * import { API_CONFIG, apiInterceptor, BaseHttpService } from '@core';
 */

// Configuración
export * from './config/api.config';

// Interceptores
export * from './interceptors/api.interceptor';

// Servicios base
export * from './services/base-http.service';

