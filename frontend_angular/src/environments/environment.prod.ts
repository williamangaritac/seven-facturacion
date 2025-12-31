/**
 * @fileoverview Configuración de entorno para producción (Docker)
 * @environment Production
 */

export const environment = {
  production: true,
  apiUrl: '/api', // En Docker, Nginx hace proxy a la API
  apiTimeout: 30000,
  enableDebugLogs: false,
};

