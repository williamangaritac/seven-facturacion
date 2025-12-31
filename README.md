# 🧾 Sistema de Facturación Seven

Sistema completo de facturación desarrollado con **.NET 10**, **Angular 20** y **PostgreSQL 16**.

[![Docker](https://img.shields.io/badge/Docker-Ready-blue?logo=docker)](https://www.docker.com/)
[![.NET](https://img.shields.io/badge/.NET-10-purple?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-20-red?logo=angular)](https://angular.io/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue?logo=postgresql)](https://www.postgresql.org/)

## 🚀 Inicio Rápido (< 5 minutos)

```bash
# 1. Clonar el repositorio
git clone https://github.com/TU-USUARIO/seven-facturacion-system.git
cd seven-facturacion-system

# 2. Levantar (Windows)
.\start.ps1

# 2. Levantar (Linux/Mac)
chmod +x start.sh && ./start.sh

# 3. Acceder
# http://localhost:4200
# Usuario: admin | Contraseña: admin123
```

> **Nota:** Reemplaza `TU-USUARIO` con tu usuario de GitHub una vez que subas el proyecto.

## 📋 Tabla de Contenidos

- [Requisitos Previos](#requisitos-previos)
- [Inicio Rápido con Docker](#inicio-rápido-con-docker)
- [Desarrollo Local](#desarrollo-local)
- [Arquitectura](#arquitectura)
- [Credenciales de Acceso](#credenciales-de-acceso)
- [Endpoints API](#endpoints-api)
- [Tecnologías](#tecnologías)
- [Documentación](#documentación)

---

## 🚀 Requisitos Previos

### Para ejecutar con Docker (Recomendado)
- [Docker](https://www.docker.com/get-started) >= 20.10
- [Docker Compose](https://docs.docker.com/compose/install/) >= 2.0

**Verificar requisitos:**
```powershell
# Windows
.\check-requirements.ps1

# Linux/Mac
chmod +x check-requirements.sh
./check-requirements.sh
```

### Para desarrollo local
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) >= 20.x
- [PostgreSQL](https://www.postgresql.org/download/) >= 15
- [Angular CLI](https://angular.io/cli) >= 20.x

---

## 🐳 Inicio Rápido con Docker

### Opción 1: Script Automático (Recomendado)

#### Windows (PowerShell)
```powershell
git clone <url-del-repositorio>
cd digitalware
.\start.ps1
```

#### Linux/Mac (Bash)
```bash
git clone <url-del-repositorio>
cd digitalware
chmod +x start.sh
./start.sh
```

### Opción 2: Comandos Manuales

#### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd digitalware
```

#### 2. Levantar todos los servicios

```bash
docker-compose up -d
```

Este comando levantará:
- **PostgreSQL** en el puerto `5432`
- **Backend API** en el puerto `5000`
- **Frontend Angular** en el puerto `4200`

#### 3. Verificar que todo funcione

**Windows:**
```powershell
.\test-docker.ps1
```

**Linux/Mac:**
```bash
chmod +x test-docker.sh
./test-docker.sh
```

#### 4. Acceder a la aplicación

Abre tu navegador en: **http://localhost:4200**

**Credenciales de acceso:**
- **Usuario:** `admin`
- **Contraseña:** `admin123`

#### 5. Detener los servicios

```bash
docker-compose down
```

Para eliminar también los volúmenes (base de datos):

```bash
docker-compose down -v
```

### 📖 Documentación Adicional

Para más detalles sobre Docker, consulta [DOCKER_SETUP.md](DOCKER_SETUP.md)

---

## 💻 Desarrollo Local

### Backend (.NET 10)

1. **Restaurar dependencias:**
   ```bash
   cd src/Seven.Facturacion.Api
   dotnet restore
   ```

2. **Configurar la base de datos:**
   
   Edita `appsettings.json` con tu cadena de conexión:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=seven_facturacion_dev;Username=postgres;Password=postgres"
     }
   }
   ```

3. **Ejecutar scripts SQL:**
   ```bash
   psql -U postgres -d seven_facturacion_dev -f Scripts/00_crear_esquema.sql
   psql -U postgres -d seven_facturacion_dev -f Scripts/01_crear_tablas.sql
   psql -U postgres -d seven_facturacion_dev -f Scripts/02_insertar_datos.sql
   psql -U postgres -d seven_facturacion_dev -f Scripts/04_crear_tabla_usuarios.sql
   psql -U postgres -d seven_facturacion_dev -f Scripts/05_actualizar_password_admin.sql
   ```

4. **Ejecutar el backend:**
   ```bash
   dotnet run
   ```

   El backend estará disponible en: `https://localhost:49497`

### Frontend (Angular 20)

1. **Instalar dependencias:**
   ```bash
   cd frontend_angular
   npm install
   ```

2. **Configurar la URL del API:**
   
   Edita `src/app/core/config/api.config.ts`:
   ```typescript
   export const API_CONFIG = {
     baseUrl: 'https://localhost:49497/api'
   };
   ```

3. **Ejecutar el frontend:**
   ```bash
   npm start
   ```

   El frontend estará disponible en: `http://localhost:4200`

---

## 🏗️ Arquitectura

### Backend - Clean Architecture

```
src/
├── Seven.Facturacion.Domain/          # Entidades y lógica de negocio
├── Seven.Facturacion.Application/     # Casos de uso y DTOs
├── Seven.Facturacion.Infrastructure/  # Persistencia y repositorios
└── Seven.Facturacion.Api/             # Controladores y configuración
```

### Frontend - Arquitectura Modular

```
frontend_angular/src/
├── app/
│   ├── core/           # Servicios singleton y configuración
│   ├── shared/         # Componentes y utilidades compartidas
│   └── features/       # Módulos de funcionalidades
```

---

## 🔐 Credenciales de Acceso

**Usuario por defecto:**
- **Username:** `admin`
- **Password:** `admin123`

---

## 📡 Endpoints API

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/validate` - Validar token

### Clientes
- `GET /api/clientes` - Listar todos los clientes
- `GET /api/clientes/{id}` - Obtener cliente por ID
- `POST /api/clientes` - Crear nuevo cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente

### Productos
- `GET /api/productos` - Listar todos los productos
- `GET /api/productos/{id}` - Obtener producto por ID
- `POST /api/productos` - Crear nuevo producto
- `PUT /api/productos/{id}` - Actualizar producto
- `DELETE /api/productos/{id}` - Eliminar producto
- `GET /api/productos/lista-precios` - Obtener lista de precios
- `GET /api/productos/bajo-stock` - Productos con stock bajo

### Facturas
- `GET /api/facturas` - Listar todas las facturas
- `GET /api/facturas/{id}` - Obtener factura por ID
- `POST /api/facturas` - Crear nueva factura
- `PUT /api/facturas/{id}` - Actualizar factura
- `DELETE /api/facturas/{id}` - Eliminar factura

---

## 🛠️ Tecnologías

### Backend
- **.NET 10** - Framework principal
- **Entity Framework Core 10** - ORM
- **PostgreSQL 16** - Base de datos
- **BCrypt.Net** - Hash de contraseñas
- **Clean Architecture** - Patrón arquitectónico

### Frontend
- **Angular 20** - Framework frontend
- **TypeScript** - Lenguaje
- **RxJS** - Programación reactiva
- **Angular Material** - Componentes UI

### DevOps
- **Docker** - Contenedores
- **Docker Compose** - Orquestación
- **Nginx** - Servidor web para frontend

---

## 📝 Scripts Disponibles

### Docker

```bash
# Levantar todos los servicios
docker-compose up -d

# Ver logs
docker-compose logs -f

# Ver logs de un servicio específico
docker-compose logs -f api
docker-compose logs -f frontend
docker-compose logs -f postgres

# Reconstruir imágenes
docker-compose build

# Reconstruir y levantar
docker-compose up -d --build

# Detener servicios
docker-compose down

# Detener y eliminar volúmenes
docker-compose down -v
```

### Backend

```bash
cd src/Seven.Facturacion.Api

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run

# Ejecutar tests
dotnet test
```

### Frontend

```bash
cd frontend_angular

# Instalar dependencias
npm install

# Desarrollo
npm start

# Compilar para producción
npm run build

# Ejecutar tests
npm test

# Linting
npm run lint
```

---

## 🔧 Solución de Problemas

### El backend no se conecta a la base de datos

Verifica que PostgreSQL esté corriendo:
```bash
docker-compose ps
```

Verifica los logs:
```bash
docker-compose logs postgres
```

### El frontend no se conecta al backend

Verifica que la URL del API en `src/app/core/config/api.config.ts` sea correcta.

En Docker, el frontend usa el proxy de Nginx, por lo que debe apuntar a `/api`.

### Error de credenciales inválidas

Asegúrate de que el script `05_actualizar_password_admin.sql` se haya ejecutado correctamente.

Puedes verificar el hash en la base de datos:
```bash
docker-compose exec postgres psql -U postgres -d seven_facturacion_dev -c "SELECT username, password_hash FROM facturacion.usuarios;"
```

---

## 📄 Licencia

Este proyecto es parte de una prueba técnica para DigitalWare.

---

## 👥 Autor

Desarrollado como prueba técnica para el proceso de selección de DigitalWare.

---

## 📚 Documentación

Este proyecto incluye documentación completa:

| Documento | Descripción | Para Quién |
|-----------|-------------|------------|
| **[QUICK_START.md](QUICK_START.md)** | Inicio en < 5 minutos | Todos |
| **[DOCKER_SETUP.md](DOCKER_SETUP.md)** | Guía detallada de Docker | Usuarios Docker |
| **[CONTRIBUTING.md](CONTRIBUTING.md)** | Guía de contribución | Desarrolladores |
| **[ARCHITECTURE_BACKEND.md](ARCHITECTURE_BACKEND.md)** | Arquitectura del backend | Desarrolladores |
| **[DOCKER_FILES_SUMMARY.md](DOCKER_FILES_SUMMARY.md)** | Resumen de archivos Docker | DevOps |
| **[DEPLOYMENT_SUMMARY.md](DEPLOYMENT_SUMMARY.md)** | Resumen de despliegue | DevOps/Líderes |
| **[DOCUMENTATION_INDEX.md](DOCUMENTATION_INDEX.md)** | Índice completo | Todos |

### 📖 Guías Rápidas

- **¿Primera vez?** → [QUICK_START.md](QUICK_START.md)
- **¿Problemas con Docker?** → [DOCKER_SETUP.md](DOCKER_SETUP.md)
- **¿Quieres contribuir?** → [CONTRIBUTING.md](CONTRIBUTING.md)
- **¿Necesitas entender la arquitectura?** → [ARCHITECTURE_BACKEND.md](ARCHITECTURE_BACKEND.md)

---

## 📞 Soporte

Para cualquier duda o problema:
1. Consulta la [documentación](DOCUMENTATION_INDEX.md)
2. Revisa los logs: `docker-compose logs -f`
3. Contacta al equipo de desarrollo

