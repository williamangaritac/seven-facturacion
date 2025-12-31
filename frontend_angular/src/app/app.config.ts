/**
 * @fileoverview Configuración principal de la aplicación Angular.
 *
 * Define los providers globales incluyendo HTTP, routing e interceptors.
 * Implementa standalone application pattern de Angular 16+.
 *
 * @architectural_layer App/Config
 * @pattern Dependency Injection, Provider Pattern
 * @author Seven Facturación Team
 * @version 1.0.0
 *
 * @providers
 * - Router: Navegación SPA
 * - HttpClient: Comunicación con API REST
 * - Interceptors: Gestión transversal de requests
 *
 * @interceptor_chain
 * 1. apiInterceptor: Headers + error handling
 */

import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';
import { apiInterceptor } from './core/interceptors/api.interceptor';

/**
 * Configuración de la aplicación standalone.
 *
 * @description
 * Configura todos los providers necesarios para la aplicación.
 * Usa el nuevo sistema de providers de Angular 16+.
 */
export const appConfig: ApplicationConfig = {
  providers: [
    // Error handling global
    provideBrowserGlobalErrorListeners(),

    // Zone.js optimization
    provideZoneChangeDetection({ eventCoalescing: true }),

    // Router con view transitions (Angular 17+)
    provideRouter(routes, withViewTransitions()),

    // HTTP client con interceptors
    provideHttpClient(
      withInterceptors([apiInterceptor])
    ),
  ],
};
