# ============================================================================
# DOCKERFILE - Frontend Angular
# Sistema de Facturación Seven
# Multi-stage build para optimizar tamaño de imagen
# ============================================================================

# ==========================================================================
# STAGE 1: BUILD
# ==========================================================================
FROM node:20-alpine AS build
WORKDIR /app

# Copiar archivos de dependencias
COPY package*.json ./

# Instalar dependencias
RUN npm ci

# Copiar código fuente
COPY . .

# Compilar aplicación para producción
RUN npm run build -- --configuration production

# ==========================================================================
# STAGE 2: SERVE CON NGINX
# ==========================================================================
FROM nginx:alpine AS runtime

# Copiar configuración personalizada de nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copiar archivos compilados
COPY --from=build /app/dist/frontend_angular/browser /usr/share/nginx/html

# Exponer puerto
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget -q --spider http://localhost/ || exit 1

# Comando de inicio (nginx en foreground)
CMD ["nginx", "-g", "daemon off;"]

