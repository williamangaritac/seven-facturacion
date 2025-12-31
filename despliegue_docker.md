# 🚀 Guía de Despliegue con Docker

## Inicio Rápido (3 pasos)

### Prerrequisitos
- **Docker Desktop** instalado y ejecutándose
  - [Descargar Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Pasos para ejecutar el proyecto

```bash
# 1. Clonar el repositorio
git clone https://github.com/tu-usuario/digitalware.git

# 2. Entrar al directorio
cd digitalware

# 3. Levantar todos los servicios
docker-compose up -d
```

### ✅ ¡Listo! Accede a:

| Servicio | URL |
|----------|-----|
| 🌐 **Frontend** | http://localhost:4200 |
| 🔌 **Backend API** | http://localhost:5000 |
| 📚 **Swagger/Docs** | http://localhost:5000/swagger |
| 🗄️ **PostgreSQL** | localhost:5432 |

---

## Comandos Útiles

```bash
# Ver estado de los servicios
docker-compose ps

# Ver logs en tiempo real
docker-compose logs -f

# Ver logs solo del backend
docker-compose logs -f api

# Detener todos los servicios
docker-compose down

# Reiniciar un servicio específico
docker-compose restart api

# Reconstruir después de cambios en código
docker-compose up -d --build

# Eliminar todo (incluidos datos de BD)
docker-compose down -v
```

---

## Estructura de Archivos Docker

```
digitalware/
├── docker-compose.yml              # Orquestación de 3 servicios
├── Dockerfile.api                  # Build del backend .NET 10
├── .dockerignore                   # Archivos excluidos del build
├── .env.example                    # Variables de entorno (plantilla)
│
├── frontend_angular/
│   ├── Dockerfile                  # Build del frontend Angular
│   ├── nginx.conf                  # Configuración del servidor web
│   └── .dockerignore
│
└── Scripts/
    ├── 00_crear_esquema.sql        # Crea esquema 'facturacion'
    ├── 01_crear_tablas.sql         # Crea tablas con índices
    └── 02_insertar_datos.sql       # Datos de prueba
```

---

## Servicios Docker

### 1. PostgreSQL (Base de Datos)
- **Imagen**: `postgres:16`
- **Puerto**: `5432`
- **Base de datos**: `seven_facturacion_dev`
- **Usuario/Contraseña**: `postgres/postgres`
- **Inicialización automática**: Los scripts SQL se ejecutan al crear el contenedor

### 2. Backend API (.NET 10)
- **Puerto**: `5000` → interno `8080`
- **Swagger**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health
- **Espera a PostgreSQL** antes de iniciar

### 3. Frontend Angular
- **Puerto**: `4200` → interno `80`
- **Servidor**: Nginx optimizado
- **Proxy API**: Las peticiones a `/api/*` se redirigen al backend

---

## Desarrollo Local (Sin Docker)

Si prefieres ejecutar sin Docker:

### Prerrequisitos
- .NET 10 SDK
- Node.js 20+
- PostgreSQL 16
- Angular CLI (`npm install -g @angular/cli`)

### Pasos

```bash
# 1. Crear base de datos PostgreSQL
psql -U postgres -c "CREATE DATABASE seven_facturacion_dev;"

# 2. Ejecutar scripts SQL
psql -U postgres -d seven_facturacion_dev -f Scripts/00_crear_esquema.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/01_crear_tablas.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/02_insertar_datos.sql

# 3. Iniciar Backend (Terminal 1)
dotnet run --project src/Seven.Facturacion.Api

# 4. Iniciar Frontend (Terminal 2)
cd frontend_angular
npm install
ng serve
```

---

## Solución de Problemas

### El puerto 5432 ya está en uso
```bash
# En Windows - Detener PostgreSQL local
net stop postgresql-x64-16

# O cambiar el puerto en docker-compose.yml
ports:
  - "5433:5432"  # Usar puerto 5433 externamente
```

### Los contenedores no inician
```bash
# Ver logs detallados
docker-compose logs

# Reconstruir desde cero
docker-compose down -v
docker-compose up -d --build
```

### Cambios no se reflejan
```bash
# Reconstruir con --build
docker-compose up -d --build
```

### Resetear base de datos
```bash
# Eliminar volumen de datos
docker-compose down -v
docker-compose up -d
```

---

## Variables de Entorno

Copia `.env.example` a `.env` para personalizar:

```bash
cp .env.example .env
```

Variables disponibles:
```env
# Base de datos
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=seven_facturacion_dev

# Backend
ASPNETCORE_ENVIRONMENT=Development

# Frontend
API_URL=http://localhost:5000
```

---

## Ventajas de Docker

| Beneficio | Descripción |
|-----------|-------------|
| ⚡ **Un solo comando** | `docker-compose up -d` levanta todo |
| 🔄 **Reproducible** | Mismo entorno en cualquier máquina |
| 📦 **Aislado** | No contamina tu sistema local |
| 🗃️ **Datos incluidos** | BD con datos de prueba listos |
| 🧹 **Fácil limpiar** | `docker-compose down -v` elimina todo |
| 💻 **Multiplataforma** | Windows, Mac, Linux |

---

## Arquitectura

```
┌─────────────────────────────────────────────────────────────┐
│                      Docker Network                          │
│                                                              │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐  │
│  │   Frontend   │    │   Backend    │    │  PostgreSQL  │  │
│  │   (Nginx)    │───▶│  (.NET 10)   │───▶│    (DB)      │  │
│  │   :4200      │    │   :5000      │    │   :5432      │  │
│  └──────────────┘    └──────────────┘    └──────────────┘  │
│                                                              │
└─────────────────────────────────────────────────────────────┘
         │                    │
         ▼                    ▼
    localhost:4200      localhost:5000
```

