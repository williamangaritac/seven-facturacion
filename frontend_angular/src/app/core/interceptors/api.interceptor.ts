/**
 * @fileoverview HTTP Interceptor para gestión centralizada de peticiones API.
 * 
 * Este interceptor implementa funcionalidades transversales (cross-cutting concerns)
 * para todas las peticiones HTTP de la aplicación, siguiendo el patrón Decorator.
 * 
 * @architectural_layer Core/Interceptors
 * @pattern Decorator Pattern, Chain of Responsibility
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @responsibilities
 * - Añadir headers estándar (Content-Type, Accept)
 * - Gestionar errores HTTP de forma centralizada
 * - Logging de peticiones en desarrollo
 * - Transformación de respuestas de error
 * 
 * @interceptor_chain
 * Este interceptor se ejecuta en la cadena de interceptores de Angular.
 * El orden de ejecución se define en app.config.ts.
 * 
 * @error_handling
 * Los errores se transforman a un formato consistente (ApiError)
 * para facilitar su manejo en componentes y servicios.
 */

import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { CONTENT_TYPES, HTTP_HEADERS } from '../config/api.config';
import { AuthService } from '../services/auth.service';

/**
 * Interface para errores de API normalizados.
 * Proporciona una estructura consistente para manejo de errores en toda la app.
 */
export interface ApiError {
  /** Código de estado HTTP */
  status: number;
  /** Mensaje de error amigable para el usuario */
  message: string;
  /** Detalles técnicos del error (solo en desarrollo) */
  details?: string;
  /** Timestamp del error */
  timestamp: Date;
  /** URL del endpoint que falló */
  url: string;
}

/**
 * HTTP Interceptor funcional para gestión de peticiones API.
 * 
 * @description
 * Implementa el patrón funcional de interceptores introducido en Angular 15+.
 * Se prefiere sobre interceptores basados en clases por su simplicidad y
 * mejor soporte para tree-shaking.
 * 
 * @param req - HttpRequest entrante
 * @param next - Handler para pasar al siguiente interceptor
 * @returns Observable con la respuesta HTTP o error transformado
 * 
 * @example
 * // En app.config.ts
 * provideHttpClient(withInterceptors([apiInterceptor]))
 */
export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Solo interceptar peticiones a nuestra API
  if (!req.url.includes('/api')) {
    return next(req);
  }

  // Preparar headers
  const headers: Record<string, string> = {
    [HTTP_HEADERS.CONTENT_TYPE]: CONTENT_TYPES.JSON,
    [HTTP_HEADERS.ACCEPT]: CONTENT_TYPES.JSON,
  };

  // Agregar token si existe
  const token = authService.getToken();
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  // Clonar request con headers adicionales
  const modifiedReq = req.clone({ setHeaders: headers });

  // Log en desarrollo
  if (typeof window !== 'undefined' && (window as Window & { __DEV__?: boolean }).__DEV__) {
    console.log(`[API] ${req.method} ${req.url}`);
  }

  return next(modifiedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const apiError = transformToApiError(error);
      console.error('[API Error]', apiError);
      return throwError(() => apiError);
    })
  );
};

/**
 * Transforma HttpErrorResponse a ApiError normalizado.
 * 
 * @param error - Error HTTP original
 * @returns ApiError con formato consistente
 * 
 * @internal
 */
function transformToApiError(error: HttpErrorResponse): ApiError {
  let message = 'Ha ocurrido un error inesperado';

  switch (error.status) {
    case 0:
      message = 'No se puede conectar con el servidor';
      break;
    case 400:
      message = error.error?.message || 'Datos de solicitud inválidos';
      break;
    case 401:
      message = 'No autorizado. Por favor inicie sesión';
      break;
    case 404:
      message = 'Recurso no encontrado';
      break;
    case 409:
      message = 'Conflicto: el recurso ya existe';
      break;
    case 500:
      message = 'Error interno del servidor';
      break;
  }

  return {
    status: error.status,
    message,
    details: error.error?.detail || error.message,
    timestamp: new Date(),
    url: error.url || '',
  };
}

