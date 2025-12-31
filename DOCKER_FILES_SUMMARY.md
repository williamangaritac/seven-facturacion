# 📦 Resumen de Archivos Docker - Sistema de Facturación Seven

Este documento describe todos los archivos relacionados con Docker en el proyecto.

## 📁 Archivos Principales

### 1. `docker-compose.yml`
**Propósito:** Orquestación de todos los servicios del proyecto.

**Servicios definidos:**
- `postgres` - Base de datos PostgreSQL 16
- `api` - Backend .NET 10
- `frontend` - Frontend Angular 20 con Nginx

**Características:**
- ✅ Health checks para todos los servicios
- ✅ Dependencias entre servicios configuradas
- ✅ Scripts SQL ejecutados automáticamente al iniciar
- ✅ Red interna para comunicación entre contenedores
- ✅ Volumen persistente para PostgreSQL

### 2. `Dockerfile.api`
**Propósito:** Construcción de la imagen del backend .NET 10.

**Características:**
- ✅ Multi-stage build (optimización de tamaño)
- ✅ Stage 1: Build con SDK completo
- ✅ Stage 2: Runtime con imagen ligera
- ✅ Health check integrado
- ✅ Curl instalado para health checks

**Tamaño aproximado:** ~200 MB (runtime)

### 3. `frontend_angular/Dockerfile`
**Propósito:** Construcción de la imagen del frontend Angular.

**Características:**
- ✅ Multi-stage build
- ✅ Stage 1: Build con Node.js
- ✅ Stage 2: Serve con Nginx Alpine
- ✅ Configuración de Nginx incluida
- ✅ Health check integrado

**Tamaño aproximado:** ~25 MB

### 4. `frontend_angular/nginx.conf`
**Propósito:** Configuración de Nginx para servir el frontend.

**Características:**
- ✅ Proxy reverso al backend API
- ✅ Soporte para rutas de Angular (SPA)
- ✅ Compresión gzip habilitada
- ✅ Cache para archivos estáticos
- ✅ No-cache para index.html

## 🚀 Scripts de Inicio

### 5. `start.ps1` (Windows)
**Propósito:** Script automatizado para levantar el proyecto en Windows.

**Funcionalidades:**
- ✅ Verifica que Docker esté instalado
- ✅ Verifica que Docker esté corriendo
- ✅ Detiene contenedores existentes
- ✅ Construye imágenes
- ✅ Levanta servicios
- ✅ Muestra información de acceso

### 6. `start.sh` (Linux/Mac)
**Propósito:** Script automatizado para levantar el proyecto en Linux/Mac.

**Funcionalidades:** (Igual que start.ps1)

## 🧪 Scripts de Prueba

### 7. `test-docker.ps1` (Windows)
**Propósito:** Verificar que todos los servicios estén funcionando correctamente.

**Pruebas realizadas:**
- ✅ Docker está corriendo
- ✅ Contenedores están activos
- ✅ Frontend responde (HTTP 200)
- ✅ Backend - Clientes endpoint (HTTP 200)
- ✅ Backend - Productos endpoint (HTTP 200)
- ✅ Backend - Facturas endpoint (HTTP 200)
- ✅ Base de datos tiene datos
- ✅ Login funciona correctamente

### 8. `test-docker.sh` (Linux/Mac)
**Propósito:** (Igual que test-docker.ps1)

## ✅ Scripts de Verificación

### 9. `check-requirements.ps1` (Windows)
**Propósito:** Verificar que todas las herramientas necesarias estén instaladas.

**Verifica:**
- ✅ Docker instalado y corriendo
- ✅ Docker Compose instalado
- ✅ .NET SDK (opcional para desarrollo local)
- ✅ Node.js (opcional para desarrollo local)
- ✅ PostgreSQL (opcional para desarrollo local)
- ✅ Git (opcional)
- ✅ Angular CLI (opcional)

## 📝 Archivos de Configuración

### 10. `.dockerignore`
**Propósito:** Excluir archivos innecesarios del contexto de build.

**Excluye:**
- Directorios de compilación (bin, obj, node_modules, dist)
- Archivos de IDE (.vs, .vscode, .idea)
- Documentación (*.md excepto README)
- Logs y archivos temporales
- Archivos de test

### 11. `.gitattributes`
**Propósito:** Normalizar line endings para diferentes sistemas operativos.

**Configuraciones:**
- Scripts shell (.sh) → LF
- Scripts PowerShell (.ps1) → CRLF
- Scripts SQL (.sql) → LF
- Dockerfiles → LF

### 12. `.env.example`
**Propósito:** Plantilla de variables de entorno.

**Variables definidas:**
- PostgreSQL (DB, USER, PASSWORD, PORT)
- Backend (ENVIRONMENT, URLS, PORT)
- Frontend (PORT)
- JWT (opcional)
- SMTP (opcional)

## 🌍 Archivos de Environment (Angular)

### 13. `frontend_angular/src/environments/environment.ts`
**Propósito:** Configuración para desarrollo local.

**Configuración:**
- `apiUrl`: `https://localhost:49497/api`
- `production`: `false`
- `enableDebugLogs`: `true`

### 14. `frontend_angular/src/environments/environment.prod.ts`
**Propósito:** Configuración para producción (Docker).

**Configuración:**
- `apiUrl`: `/api` (proxy de Nginx)
- `production`: `true`
- `enableDebugLogs`: `false`

## 🛠️ Herramientas Adicionales

### 15. `Makefile`
**Propósito:** Comandos simplificados para desarrollo.

**Comandos disponibles:**
- `make help` - Mostrar ayuda
- `make build` - Construir imágenes
- `make up` - Levantar servicios
- `make down` - Detener servicios
- `make logs` - Ver logs
- `make clean` - Limpiar todo
- `make db-shell` - Acceder a PostgreSQL
- Y más...

## 📚 Documentación

### 16. `DOCKER_SETUP.md`
**Propósito:** Guía detallada de configuración Docker.

**Contenido:**
- Requisitos previos
- Inicio rápido
- Acceso a servicios
- Comandos útiles
- Arquitectura de contenedores
- Solución de problemas
- Variables de entorno

### 17. `CONTRIBUTING.md`
**Propósito:** Guía para desarrolladores que quieran contribuir.

**Contenido:**
- Requisitos para desarrollo
- Configuración del entorno
- Estructura del proyecto
- Convenciones de código
- Testing
- Flujo de trabajo Git

## 🔄 Flujo de Trabajo

### Para Usuarios (Solo Ejecutar)

```bash
# 1. Verificar requisitos
.\check-requirements.ps1

# 2. Levantar proyecto
.\start.ps1

# 3. Verificar que funcione
.\test-docker.ps1

# 4. Acceder a http://localhost:4200
```

### Para Desarrolladores

```bash
# 1. Clonar repositorio
git clone <url>
cd digitalware

# 2. Verificar requisitos
.\check-requirements.ps1

# 3. Levantar con Docker
docker-compose up -d

# O desarrollo local
cd src/Seven.Facturacion.Api && dotnet run
cd frontend_angular && npm start
```

## 📊 Puertos Utilizados

| Servicio | Puerto Host | Puerto Contenedor | Protocolo |
|----------|-------------|-------------------|-----------|
| Frontend | 4200 | 80 | HTTP |
| Backend | 5000 | 8080 | HTTP |
| PostgreSQL | 5432 | 5432 | TCP |

## 🔐 Credenciales por Defecto

### Aplicación
- Usuario: `admin`
- Contraseña: `admin123`

### PostgreSQL
- Host: `localhost` (o `postgres` en Docker)
- Puerto: `5432`
- Database: `seven_facturacion_dev`
- Usuario: `postgres`
- Contraseña: `postgres`

## ✨ Características Destacadas

1. **🚀 Inicio con un solo comando** - Scripts automatizados
2. **✅ Verificación automática** - Scripts de prueba incluidos
3. **🔄 Hot reload** - Para desarrollo local
4. **📦 Multi-stage builds** - Imágenes optimizadas
5. **🏥 Health checks** - Monitoreo de servicios
6. **🔒 Persistencia de datos** - Volúmenes de Docker
7. **🌐 Proxy reverso** - Nginx configurado
8. **📝 Documentación completa** - Múltiples guías

## 🎯 Próximos Pasos

Después de levantar el proyecto:

1. ✅ Accede a http://localhost:4200
2. ✅ Inicia sesión con admin/admin123
3. ✅ Explora las funcionalidades
4. ✅ Revisa la documentación en `/docs`
5. ✅ Lee CONTRIBUTING.md si quieres contribuir

---

**¿Problemas?** Consulta DOCKER_SETUP.md o revisa los logs con `docker-compose logs -f`

