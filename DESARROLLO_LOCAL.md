# 💻 Guía de Desarrollo Local - Sistema de Facturación Seven

Esta guía te ayudará a levantar el proyecto en modo desarrollo local (sin Docker).

## 📋 Requisitos Previos

### Obligatorios
- ✅ **.NET 10 SDK** - [Descargar](https://dotnet.microsoft.com/download)
- ✅ **Node.js 20+** - [Descargar](https://nodejs.org/)
- ✅ **PostgreSQL 16** - [Descargar](https://www.postgresql.org/download/)

### Opcionales
- ✅ **Angular CLI** - `npm install -g @angular/cli`
- ✅ **Visual Studio Code** - [Descargar](https://code.visualstudio.com/)
- ✅ **pgAdmin** - Para administrar PostgreSQL

---

## 🚀 Opción 1: Scripts Automatizados (RECOMENDADO)

### Paso 1: Levantar PostgreSQL

**Opción A: PostgreSQL en Docker (más fácil)**
```bash
docker-compose up -d postgres
```

**Opción B: PostgreSQL local**
- Asegúrate de que PostgreSQL esté corriendo en puerto 5432
- Crea la base de datos y ejecuta los scripts SQL

### Paso 2: Levantar Backend

**Windows:**
```powershell
.\start-backend.ps1
```

**Linux/Mac:**
```bash
chmod +x start-backend.sh
./start-backend.sh
```

### Paso 3: Levantar Frontend (en otra terminal)

**Windows:**
```powershell
.\start-frontend.ps1
```

**Linux/Mac:**
```bash
chmod +x start-frontend.sh
./start-frontend.sh
```

### Paso 4: Acceder

- **Frontend:** http://localhost:4200
- **Backend:** https://localhost:49497/api
- **Swagger:** https://localhost:49497/swagger

**Credenciales:**
- Usuario: `admin`
- Contraseña: `admin123`

---

## 🔧 Opción 2: Comandos Manuales

### 1. Configurar PostgreSQL

```bash
# Crear base de datos
createdb -U postgres seven_facturacion_dev

# Ejecutar scripts SQL (en orden)
psql -U postgres -d seven_facturacion_dev -f Scripts/00_crear_esquema.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/01_crear_tablas.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/02_insertar_datos.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/04_crear_tabla_usuarios.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/05_actualizar_password_admin.sql
```

### 2. Levantar Backend

```bash
# Navegar a la carpeta del backend
cd src/Seven.Facturacion.Api

# Restaurar dependencias
dotnet restore

# Ejecutar
dotnet run
```

El backend estará en: https://localhost:49497

### 3. Levantar Frontend (en otra terminal)

```bash
# Navegar a la carpeta del frontend
cd frontend_angular

# Instalar dependencias (solo la primera vez)
npm install

# Ejecutar
npm start
```

El frontend estará en: http://localhost:4200

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

### Cadena de Conexión (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=seven_facturacion_dev;Username=postgres;Password=postgres"
  }
}
```

### Credenciales de PostgreSQL

- **Host:** localhost
- **Puerto:** 5432
- **Database:** seven_facturacion_dev
- **Usuario:** postgres
- **Contraseña:** postgres

---

## ⚙️ Configuración de Environments

### Frontend - Development (environment.ts)

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:49497/api',
  apiTimeout: 30000,
  enableDebugLogs: true,
};
```

### Frontend - Production (environment.prod.ts)

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

### Base de Datos

```bash
# Conectar a PostgreSQL
psql -U postgres -d seven_facturacion_dev

# Backup
pg_dump -U postgres seven_facturacion_dev > backup.sql

# Restore
psql -U postgres -d seven_facturacion_dev < backup.sql

# Ver tablas
\dt

# Describir tabla
\d nombre_tabla
```

---

## 🔍 Solución de Problemas

### Backend no inicia

**Error:** "Unable to connect to database"

**Solución:**
1. Verifica que PostgreSQL esté corriendo
2. Verifica las credenciales en `appsettings.json`
3. Verifica que la base de datos exista

### Frontend no se conecta al Backend

**Error:** "CORS policy error"

**Solución:**
1. Verifica que el backend esté corriendo
2. Verifica la URL en `environment.ts`
3. El backend ya tiene CORS configurado para localhost:4200

### Puerto ya en uso

**Error:** "Port 4200 is already in use"

**Solución:**
```bash
# Cambiar puerto del frontend
ng serve --port 4201
```

**Error:** "Port 49497 is already in use"

**Solución:**
```bash
# Matar proceso en Windows
netstat -ano | findstr :49497
taskkill /PID <PID> /F

# Matar proceso en Linux/Mac
lsof -i :49497
kill -9 <PID>
```

---

## 📝 Notas Importantes

1. **Hot Reload:**
   - Backend: Usa `dotnet watch run` para hot reload
   - Frontend: `ng serve` ya tiene hot reload por defecto

2. **HTTPS:**
   - El backend usa HTTPS en desarrollo
   - Acepta el certificado autofirmado en el navegador

3. **Logs:**
   - Backend: Los logs aparecen en la consola
   - Frontend: Abre las DevTools del navegador (F12)

4. **Base de Datos:**
   - Los datos se persisten en PostgreSQL
   - Para resetear, elimina y recrea la base de datos

---

## 🎯 Flujo de Trabajo Recomendado

1. **Levantar PostgreSQL** (Docker o local)
2. **Levantar Backend** en una terminal
3. **Levantar Frontend** en otra terminal
4. **Desarrollar** con hot reload
5. **Probar** en http://localhost:4200

---

## 📚 Más Información

- **[README.md](README.md)** - Documentación general
- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Guía de contribución
- **[ARCHITECTURE_BACKEND.md](ARCHITECTURE_BACKEND.md)** - Arquitectura del backend
- **[DOCKER_SETUP.md](DOCKER_SETUP.md)** - Desarrollo con Docker

---

**¡Listo para desarrollar!** 💻

