# 🤝 Guía de Contribución - Sistema de Facturación Seven

¡Gracias por tu interés en contribuir al proyecto! Esta guía te ayudará a configurar tu entorno de desarrollo.

## 📋 Requisitos para Desarrollo

### Herramientas Necesarias

- **Git** >= 2.30
- **Docker Desktop** >= 20.10 (para desarrollo con contenedores)
- **.NET SDK 10** (para desarrollo local del backend)
- **Node.js** >= 20.x (para desarrollo local del frontend)
- **PostgreSQL** >= 15 (para desarrollo local)
- **Visual Studio Code** o **Visual Studio 2022** (recomendado)

### Extensiones Recomendadas para VS Code

- C# Dev Kit
- Angular Language Service
- Docker
- PostgreSQL
- GitLens
- ESLint
- Prettier

## 🚀 Configuración del Entorno

### 1. Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd digitalware
```

### 2. Configurar Git

```bash
git config core.autocrlf input  # Linux/Mac
git config core.autocrlf true   # Windows
```

### 3. Opción A: Desarrollo con Docker (Recomendado)

```bash
# Levantar todos los servicios
docker-compose up -d

# Ver logs
docker-compose logs -f
```

### 3. Opción B: Desarrollo Local

#### Backend (.NET 10)

```bash
# Restaurar dependencias
cd src/Seven.Facturacion.Api
dotnet restore

# Configurar base de datos
# Edita appsettings.Development.json con tu cadena de conexión

# Ejecutar migraciones
psql -U postgres -d seven_facturacion_dev -f ../../Scripts/00_crear_esquema.sql
psql -U postgres -d seven_facturacion_dev -f ../../Scripts/01_crear_tablas.sql
psql -U postgres -d seven_facturacion_dev -f ../../Scripts/02_insertar_datos.sql
psql -U postgres -d seven_facturacion_dev -f ../../Scripts/04_crear_tabla_usuarios.sql
psql -U postgres -d seven_facturacion_dev -f ../../Scripts/05_actualizar_password_admin.sql

# Ejecutar
dotnet run
```

#### Frontend (Angular 20)

```bash
# Instalar dependencias
cd frontend_angular
npm install

# Ejecutar en modo desarrollo
npm start
```

## 🏗️ Estructura del Proyecto

```
digitalware/
├── src/                          # Backend .NET
│   ├── Seven.Facturacion.Domain/
│   ├── Seven.Facturacion.Application/
│   ├── Seven.Facturacion.Infrastructure/
│   └── Seven.Facturacion.Api/
├── frontend_angular/             # Frontend Angular
│   └── src/
│       └── app/
│           ├── core/
│           ├── shared/
│           └── features/
├── Scripts/                      # Scripts SQL
├── docker-compose.yml
└── README.md
```

## 📝 Convenciones de Código

### Backend (.NET)

- Seguir las convenciones de C# de Microsoft
- Usar Clean Architecture
- Nombres de clases en PascalCase
- Nombres de métodos en PascalCase
- Nombres de variables en camelCase
- Usar async/await para operaciones asíncronas

### Frontend (Angular)

- Seguir la guía de estilo de Angular
- Usar TypeScript estricto
- Componentes en kebab-case
- Servicios con sufijo `.service.ts`
- Interfaces con prefijo `I` o sin prefijo
- Usar RxJS para programación reactiva

### SQL

- Nombres de tablas en minúsculas con guiones bajos
- Nombres de columnas en minúsculas con guiones bajos
- Usar esquema `facturacion` para todas las tablas

## 🧪 Testing

### Backend

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con cobertura
dotnet test /p:CollectCoverage=true
```

### Frontend

```bash
cd frontend_angular

# Ejecutar tests unitarios
npm test

# Ejecutar tests con cobertura
npm run test:coverage

# Ejecutar linting
npm run lint
```

## 🔄 Flujo de Trabajo Git

### Branches

- `main` - Rama principal (producción)
- `develop` - Rama de desarrollo
- `feature/<nombre>` - Nuevas funcionalidades
- `bugfix/<nombre>` - Corrección de bugs
- `hotfix/<nombre>` - Correcciones urgentes

### Commits

Usar mensajes descriptivos siguiendo Conventional Commits:

```
feat: agregar endpoint de reportes
fix: corregir validación de RUC
docs: actualizar README
style: formatear código
refactor: reorganizar servicios
test: agregar tests para clientes
chore: actualizar dependencias
```

### Pull Requests

1. Crear una rama desde `develop`
2. Hacer commits descriptivos
3. Ejecutar tests localmente
4. Crear Pull Request a `develop`
5. Esperar revisión de código
6. Hacer merge después de aprobación

## 🐛 Reportar Bugs

Al reportar un bug, incluye:

1. Descripción clara del problema
2. Pasos para reproducir
3. Comportamiento esperado vs actual
4. Screenshots si aplica
5. Versión del navegador/SO
6. Logs relevantes

## ✨ Proponer Nuevas Funcionalidades

1. Abre un Issue describiendo la funcionalidad
2. Espera feedback del equipo
3. Si es aprobada, crea una rama `feature/`
4. Implementa la funcionalidad
5. Crea Pull Request

## 📚 Recursos

- [Documentación .NET 10](https://learn.microsoft.com/en-us/dotnet/)
- [Documentación Angular](https://angular.io/docs)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Docker Documentation](https://docs.docker.com/)

## 💬 Contacto

Para preguntas o dudas, contacta al equipo de desarrollo.

---

¡Gracias por contribuir! 🎉

