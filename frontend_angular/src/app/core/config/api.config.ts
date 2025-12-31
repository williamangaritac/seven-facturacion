/**
 * @fileoverview Configuración centralizada de la API REST para Seven Facturación.
 * 
 * Este archivo define constantes de configuración que permiten gestionar la
 * comunicación con el backend .NET 10 de manera consistente en toda la aplicación.
 * 
 * @architectural_layer Core/Config
 * @pattern Configuration Object Pattern
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @usedBy
 * - HttpInterceptors para inyectar la base URL automáticamente
 * - Servicios HTTP para construir endpoints dinámicos
 * - Componentes que necesiten acceso directo a configuración
 * 
 * @environment_notes
 * - Development: http://localhost:5000/api
 * - Production: Se configura mediante environment.ts
 * 
 * @best_practices
 * - Mantener todas las URLs relativas en servicios
 * - Usar API_CONFIG.endpoints para centralizar rutas
 * - No hardcodear URLs en componentes o servicios
 */

import { InjectionToken } from '@angular/core';

/**
 * Interface que define la estructura de configuración de la API.
 * Utiliza tipado fuerte para prevenir errores en tiempo de compilación.
 */
export interface ApiConfiguration {
  /** URL base del servidor API (sin trailing slash) */
  readonly baseUrl: string;
  /** Timeout en milisegundos para peticiones HTTP */
  readonly timeout: number;
  /** Versión de la API para versionado de endpoints */
  readonly version: string;
  /** Endpoints disponibles en la API */
  readonly endpoints: Readonly<ApiEndpoints>;
}

/**
 * Interface que define todos los endpoints disponibles en la API.
 * Centraliza las rutas para facilitar mantenimiento y refactoring.
 */
export interface ApiEndpoints {
  readonly clientes: string;
  readonly productos: string;
  readonly facturas: string;
}

/**
 * Token de inyección para proveer la configuración de API.
 * Permite inyectar la configuración en servicios y componentes.
 * 
 * @example
 * constructor(@Inject(API_CONFIG_TOKEN) private config: ApiConfiguration) {}
 */
export const API_CONFIG_TOKEN = new InjectionToken<ApiConfiguration>('API_CONFIG');

/**
 * Configuración por defecto de la API.
 * 
 * @readonly
 * @constant
 * 
 * @property {string} baseUrl - URL base del backend .NET 10
 * @property {number} timeout - 30 segundos de timeout por defecto
 * @property {string} version - Versión actual de la API
 * @property {ApiEndpoints} endpoints - Rutas de recursos REST
 */
export const API_CONFIG: ApiConfiguration = {
  baseUrl: 'https://localhost:49497/api',
  timeout: 30000,
  version: 'v1',
  endpoints: {
    clientes: '/Clientes',
    productos: '/Productos',
    facturas: '/Facturas',
  },
} as const;

/**
 * Helper function para construir URLs completas de API.
 * 
 * @param endpoint - Endpoint relativo (ej: '/Clientes')
 * @param id - ID opcional del recurso
 * @returns URL completa del endpoint
 * 
 * @example
 * buildApiUrl('/Clientes') // => 'http://localhost:5000/api/Clientes'
 * buildApiUrl('/Clientes', 5) // => 'http://localhost:5000/api/Clientes/5'
 */
export function buildApiUrl(endpoint: string, id?: number | string): string {
  const base = `${API_CONFIG.baseUrl}${endpoint}`;
  return id !== undefined ? `${base}/${id}` : base;
}

/**
 * Constantes de headers HTTP utilizados en la comunicación con la API.
 * Sigue las convenciones RESTful y JSON:API.
 */
export const HTTP_HEADERS = {
  CONTENT_TYPE: 'Content-Type',
  ACCEPT: 'Accept',
  AUTHORIZATION: 'Authorization',
} as const;

/**
 * Valores de Content-Type soportados por la API.
 */
export const CONTENT_TYPES = {
  JSON: 'application/json',
  FORM_DATA: 'multipart/form-data',
} as const;

