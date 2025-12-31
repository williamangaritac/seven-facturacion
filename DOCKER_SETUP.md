# 🐳 Guía de Configuración Docker - Sistema de Facturación Seven

Esta guía te ayudará a levantar el proyecto completo usando Docker en cualquier máquina.

## 📋 Requisitos Previos

- **Docker Desktop** instalado y corriendo
  - Windows: [Descargar Docker Desktop](https://www.docker.com/products/docker-desktop/)
  - Mac: [Descargar Docker Desktop](https://www.docker.com/products/docker-desktop/)
  - Linux: [Instalar Docker Engine](https://docs.docker.com/engine/install/)

- **Docker Compose** (incluido en Docker Desktop)

## 🚀 Inicio Rápido

### Opción 1: Script Automático (Recomendado)

#### Windows (PowerShell)
```powershell
.\start.ps1
```

#### Linux/Mac (Bash)
```bash
chmod +x start.sh
./start.sh
```

### Opción 2: Comandos Manuales

```bash
# 1. Clonar el repositorio
git clone <url-del-repositorio>
cd digitalware

# 2. Construir las imágenes
docker-compose build

# 3. Levantar los servicios
docker-compose up -d

# 4. Verificar que todo esté corriendo
docker-compose ps
```

## 🌐 Acceso a los Servicios

Una vez levantados los contenedores:

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **Frontend** | http://localhost:4200 | Aplicación Angular |
| **Backend API** | http://localhost:5000 | API REST .NET 10 |
| **PostgreSQL** | localhost:5432 | Base de datos |

## 🔐 Credenciales

### Aplicación Web
- **Usuario:** `admin`
- **Contraseña:** `admin123`

### Base de Datos PostgreSQL
- **Host:** `localhost` (o `postgres` dentro de Docker)
- **Puerto:** `5432`
- **Database:** `seven_facturacion_dev`
- **Usuario:** `postgres`
- **Contraseña:** `postgres`

## 📊 Comandos Útiles

### Ver logs de todos los servicios
```bash
docker-compose logs -f
```

### Ver logs de un servicio específico
```bash
docker-compose logs -f api        # Backend
docker-compose logs -f frontend   # Frontend
docker-compose logs -f postgres   # Base de datos
```

### Reiniciar un servicio
```bash
docker-compose restart api
docker-compose restart frontend
docker-compose restart postgres
```

### Detener todos los servicios
```bash
docker-compose down
```

### Detener y eliminar volúmenes (resetear base de datos)
```bash
docker-compose down -v
```

### Reconstruir imágenes
```bash
docker-compose build --no-cache
```

### Reconstruir y levantar
```bash
docker-compose up -d --build
```

### Acceder a la consola de PostgreSQL
```bash
docker-compose exec postgres psql -U postgres -d seven_facturacion_dev
```

### Ejecutar comandos en el contenedor del backend
```bash
docker-compose exec api bash
```

### Ver estado de los contenedores
```bash
docker-compose ps
```

### Ver uso de recursos
```bash
docker stats
```

## 🏗️ Arquitectura de Contenedores

```
┌─────────────────────────────────────────────────────────┐
│                    Docker Network                        │
│                   (seven_network)                        │
│                                                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Frontend   │  │   Backend    │  │  PostgreSQL  │  │
│  │   Angular    │  │   .NET 10    │  │      16      │  │
│  │              │  │              │  │              │  │
│  │  Port: 4200  │  │  Port: 5000  │  │  Port: 5432  │  │
│  │              │  │              │  │              │  │
│  │   Nginx      │──┼──> API      │──┼──> Database  │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

## 🔧 Solución de Problemas

### Error: "Cannot connect to the Docker daemon"
**Solución:** Asegúrate de que Docker Desktop esté corriendo.

### Error: "Port already in use"
**Solución:** Algún servicio está usando los puertos 4200, 5000 o 5432.

```bash
# Windows
netstat -ano | findstr :4200
netstat -ano | findstr :5000
netstat -ano | findstr :5432

# Linux/Mac
lsof -i :4200
lsof -i :5000
lsof -i :5432
```

Detén el proceso que esté usando el puerto o cambia el puerto en `docker-compose.yml`.

### El frontend no carga
1. Verifica que el contenedor esté corriendo: `docker-compose ps`
2. Revisa los logs: `docker-compose logs frontend`
3. Reconstruye la imagen: `docker-compose build frontend`

### El backend no responde
1. Verifica que PostgreSQL esté saludable: `docker-compose ps`
2. Revisa los logs: `docker-compose logs api`
3. Verifica la conexión a la base de datos: `docker-compose logs postgres`

### La base de datos no tiene datos
Los scripts SQL se ejecutan automáticamente al crear el contenedor por primera vez.

Si necesitas resetear la base de datos:
```bash
docker-compose down -v
docker-compose up -d
```

### Credenciales inválidas
Si el login no funciona, verifica que el script de usuarios se haya ejecutado:

```bash
docker-compose exec postgres psql -U postgres -d seven_facturacion_dev -c "SELECT username, activo FROM facturacion.usuarios;"
```

Deberías ver el usuario `admin` activo.

## 🔄 Actualizar el Proyecto

Si hay cambios en el código:

```bash
# 1. Detener servicios
docker-compose down

# 2. Obtener últimos cambios
git pull

# 3. Reconstruir imágenes
docker-compose build

# 4. Levantar servicios
docker-compose up -d
```

## 📦 Volúmenes

El proyecto usa volúmenes de Docker para persistir datos:

- `postgres_data`: Datos de PostgreSQL

Para ver los volúmenes:
```bash
docker volume ls
```

Para eliminar volúmenes huérfanos:
```bash
docker volume prune
```

## 🌍 Variables de Entorno

Las variables de entorno se configuran en `docker-compose.yml`.

Para personalizarlas, crea un archivo `.env` en la raíz del proyecto:

```env
POSTGRES_PASSWORD=tu_password_seguro
API_PORT=5000
FRONTEND_PORT=4200
```

## 📝 Notas Importantes

1. **Primera ejecución:** La primera vez que ejecutes `docker-compose up`, Docker descargará las imágenes base y construirá las imágenes del proyecto. Esto puede tardar varios minutos.

2. **Persistencia de datos:** Los datos de PostgreSQL se guardan en un volumen de Docker, por lo que persisten entre reinicios.

3. **Hot reload:** El frontend NO tiene hot reload en Docker. Para desarrollo con hot reload, usa el modo de desarrollo local.

4. **HTTPS:** El backend en Docker usa HTTP (puerto 5000). Para HTTPS, configura un reverse proxy como Traefik o Nginx.

## ✅ Verificación de Instalación

Después de levantar los servicios, verifica que todo funcione:

1. ✅ Frontend accesible en http://localhost:4200
2. ✅ Backend responde en http://localhost:5000/api/clientes
3. ✅ Login funciona con admin/admin123
4. ✅ Puedes crear, editar y eliminar clientes/productos/facturas

---

**¿Problemas?** Revisa los logs con `docker-compose logs -f` o contacta al equipo de desarrollo.

