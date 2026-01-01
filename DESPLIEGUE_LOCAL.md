# 🚀 Guía Completa de Despliegue Local - Sistema de Facturación Seven

Esta guía te ayudará a desplegar el proyecto completo en tu máquina local (Frontend + Backend + Base de Datos).

---

## 📋 Requisitos Previos

### Obligatorios
- ✅ **.NET 10 SDK** - [Descargar](https://dotnet.microsoft.com/download)
- ✅ **Node.js 20+** - [Descargar](https://nodejs.org/)
- ✅ **PostgreSQL 16** - [Descargar](https://www.postgresql.org/download/)
- ✅ **Git** - [Descargar](https://git-scm.com/)

### Opcionales
- ✅ **Angular CLI** - `npm install -g @angular/cli`
- ✅ **Visual Studio Code** - [Descargar](https://code.visualstudio.com/)
- ✅ **pgAdmin** - Para administrar PostgreSQL
- ✅ **Postman** - Para probar API

---

## 🎯 Opción 1: Despliegue Rápido con Scripts (RECOMENDADO)

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/williamangaritac/seven-facturacion.git
cd seven-facturacion
```

### Paso 2: Verificar Requisitos

```bash
# Verificar .NET
dotnet --version

# Verificar Node.js
node --version
npm --version

# Verificar PostgreSQL
psql --version
```

### Paso 3: Configurar Base de Datos

**Opción A: PostgreSQL Local (Recomendado)**

```bash
# Crear base de datos
createdb -U postgres seven_facturacion_dev

# Ejecutar scripts SQL (en orden)
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/00_crear_esquema.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/01_crear_tablas.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/02_insertar_datos.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/04_crear_tabla_usuarios.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/05_actualizar_password_admin.sql
```

**Opción B: PostgreSQL en Docker**

```bash
# Levantar solo PostgreSQL
docker-compose up -d postgres

# Esperar 10 segundos a que inicie
# Luego ejecutar los scripts SQL
```

### Paso 4: Levantar Backend

**Windows (PowerShell):**
```powershell
.\start-backend.ps1
```

**Linux/Mac (Bash):**
```bash
chmod +x start-backend.sh
./start-backend.sh
```

**Manual:**
```bash
cd app_backend/src/Seven.Facturacion.Api
dotnet restore
dotnet run
```

✅ Backend estará en: `https://localhost:49497/api`

### Paso 5: Levantar Frontend (en otra terminal)

**Windows (PowerShell):**
```powershell
.\start-frontend.ps1
```

**Linux/Mac (Bash):**
```bash
chmod +x start-frontend.sh
./start-frontend.sh
```

**Manual:**
```bash
cd frontend_angular
npm install
npm start
```

✅ Frontend estará en: `http://localhost:4200`

### Paso 6: Acceder a la Aplicación

Abre tu navegador y ve a:
- **Frontend:** http://localhost:4200
- **Backend API:** https://localhost:49497/api
- **Swagger:** https://localhost:49497/swagger

**Credenciales:**
- Usuario: `admin`
- Contraseña: `admin123`

---

## 🐳 Opción 2: Despliegue Completo con Docker

### Requisitos
- ✅ Docker instalado
- ✅ Docker Compose instalado

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/williamangaritac/seven-facturacion.git
cd seven-facturacion
```

### Paso 2: Levantar Todo

```bash
# Levantar todos los servicios
docker-compose up -d

# Ver logs
docker-compose logs -f

# Verificar que todo esté corriendo
docker-compose ps
```

### Paso 3: Acceder

```
Frontend:  http://localhost:4200
Backend:   http://localhost:5000/api
Swagger:   http://localhost:5000/swagger
PostgreSQL: localhost:5432
```

**Credenciales:**
- Usuario: `admin`
- Contraseña: `admin123`

### Comandos Útiles

```bash
# Detener todo
docker-compose down

# Detener y eliminar volúmenes (resetear BD)
docker-compose down -v

# Ver logs de un servicio
docker-compose logs -f api
docker-compose logs -f frontend
docker-compose logs -f postgres

# Reconstruir imágenes
docker-compose build --no-cache

# Reiniciar un servicio
docker-compose restart api
```

---

## 🔧 Opción 3: Despliegue Manual Paso a Paso

### Paso 1: Configurar PostgreSQL

```bash
# Conectar a PostgreSQL
psql -U postgres

# En la consola de PostgreSQL:
CREATE DATABASE seven_facturacion_dev;
\c seven_facturacion_dev

# Salir
\q
```

### Paso 2: Ejecutar Scripts SQL

```bash
# Desde la raíz del proyecto
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/00_crear_esquema.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/01_crear_tablas.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/02_insertar_datos.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/04_crear_tabla_usuarios.sql
psql -U postgres -d seven_facturacion_dev -f app_backend/Scripts/05_actualizar_password_admin.sql
```

### Paso 3: Configurar Backend

```bash
cd app_backend/src/Seven.Facturacion.Api

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

Backend estará en: `https://localhost:49497`

### Paso 4: Configurar Frontend

```bash
cd frontend_angular

# Instalar dependencias
npm install

# Ejecutar servidor de desarrollo
npm start
```

Frontend estará en: `http://localhost:4200`

---

## 🌐 URLs de Desarrollo

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **Frontend** | http://localhost:4200 | Aplicación Angular |
| **Backend API** | https://localhost:49497/api | API REST |
| **Swagger** | https://localhost:49497/swagger | Documentación API |
| **PostgreSQL** | localhost:5432 | Base de datos |

---

## 🔐 Configuración de Base de Datos

### Credenciales PostgreSQL

```
Host: localhost
Puerto: 5432
Database: seven_facturacion_dev
Usuario: postgres
Contraseña: postgres
```

### Connection String (.NET)

```
Host=localhost;Port=5432;Database=seven_facturacion_dev;Username=postgres;Password=postgres
```

### Archivo: `app_backend/src/Seven.Facturacion.Api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=seven_facturacion_dev;Username=postgres;Password=postgres"
  }
}
```

---

## ⚙️ Configuración de Environments

### Frontend - Development (`frontend_angular/src/environments/environment.ts`)

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:49497/api',
  apiTimeout: 30000,
  enableDebugLogs: true,
};
```

### Frontend - Production (`frontend_angular/src/environments/environment.prod.ts`)

```typescript
export const environment = {
  production: true,
  apiUrl: '/api',
  apiTimeout: 30000,
  enableDebugLogs: false,
};
```

---

## 🛠️ Comandos Útiles

### Backend (.NET)

```bash
cd app_backend/src/Seven.Facturacion.Api

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run

# Ejecutar con hot reload
dotnet watch run

# Ejecutar tests
dotnet test
```

### Frontend (Angular)

```bash
cd frontend_angular

# Instalar dependencias
npm install

# Ejecutar en desarrollo
npm start
# o
ng serve

# Compilar para producción
npm run build
# o
ng build --configuration production

# Ejecutar tests
npm test
# o
ng test

# Linting
npm run lint
# o
ng lint
```

### Base de Datos (PostgreSQL)

```bash
# Conectar a PostgreSQL
psql -U postgres -d seven_facturacion_dev

# Ver tablas
\dt

# Describir tabla
\d clientes

# Ejecutar query
SELECT * FROM clientes;

# Salir
\q

# Backup
pg_dump -U postgres seven_facturacion_dev > backup.sql

# Restore
psql -U postgres -d seven_facturacion_dev < backup.sql
```

---

## 🔍 Solución de Problemas

### Backend no inicia

**Error:** "Unable to connect to database"

**Solución:**
1. Verifica que PostgreSQL esté corriendo
2. Verifica las credenciales en `appsettings.json`
3. Verifica que la base de datos exista

```bash
# Verificar PostgreSQL
psql -U postgres -c "SELECT version();"

# Verificar base de datos
psql -U postgres -l | grep seven_facturacion_dev
```

### Frontend no se conecta al Backend

**Error:** "CORS policy error" o "Failed to fetch"

**Solución:**
1. Verifica que el backend esté corriendo
2. Verifica la URL en `environment.ts`
3. Verifica que el certificado HTTPS sea aceptado

```bash
# Probar conexión al backend
curl -k https://localhost:49497/api/clientes
```

### Puerto ya en uso

**Error:** "Port 4200 is already in use"

**Solución:**
```bash
# Cambiar puerto del frontend
ng serve --port 4201

# O matar el proceso
# Windows
netstat -ano | findstr :4200
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :4200
kill -9 <PID>
```

### Base de datos no se crea

**Error:** "database does not exist"

**Solución:**
```bash
# Crear base de datos manualmente
createdb -U postgres seven_facturacion_dev

# Verificar que se creó
psql -U postgres -l | grep seven_facturacion_dev
```

---

## 📝 Flujo de Trabajo Recomendado

### Desarrollo Diario

1. **Iniciar PostgreSQL** (si no está en Docker)
   ```bash
   # Windows: PostgreSQL se inicia automáticamente
   # Linux/Mac: brew services start postgresql
   ```

2. **Terminal 1 - Backend**
   ```bash
   cd app_backend/src/Seven.Facturacion.Api
   dotnet watch run
   ```

3. **Terminal 2 - Frontend**
   ```bash
   cd frontend_angular
   npm start
   ```

4. **Navegador**
   ```
   http://localhost:4200
   ```

5. **Desarrollar** con hot reload automático

### Testing

```bash
# Backend
cd app_backend
dotnet test

# Frontend
cd frontend_angular
npm test
```

### Build para Producción

```bash
# Backend
cd app_backend
dotnet publish -c Release

# Frontend
cd frontend_angular
npm run build
```

---

## ✅ Checklist de Despliegue

- [ ] .NET 10 SDK instalado
- [ ] Node.js 20+ instalado
- [ ] PostgreSQL 16 instalado
- [ ] Repositorio clonado
- [ ] Base de datos creada
- [ ] Scripts SQL ejecutados
- [ ] Backend levantado y funcionando
- [ ] Frontend levantado y funcionando
- [ ] Login funciona (admin/admin123)
- [ ] CRUD de clientes funciona
- [ ] CRUD de productos funciona
- [ ] Creación de facturas funciona

---

## 🎯 Próximos Pasos

1. **Leer documentación:**
   - `README.md` - Información general
   - `ARCHITECTURE_FRONTEND.md` - Arquitectura del frontend
   - `ARCHITECTURE_BASE_DATOS.md` - Arquitectura de BD
   - `DOCKER_SETUP.md` - Despliegue con Docker

2. **Desarrollar:**
   - Crear nuevas features
   - Modificar componentes
   - Agregar nuevas tablas

3. **Desplegar:**
   - A staging
   - A producción

---

## 📞 Soporte

Si tienes problemas:

1. Revisa la sección "Solución de Problemas"
2. Verifica los logs del backend y frontend
3. Consulta la documentación de arquitectura
4. Abre un issue en GitHub

---

**¡Listo para desarrollar!** 💻

