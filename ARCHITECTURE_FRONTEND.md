# 🎨 Arquitectura del Frontend - Sistema de Facturación Seven

## 📋 Información General

| Aspecto | Detalle |
|---------|---------|
| **Framework** | Angular 19 |
| **Lenguaje** | TypeScript 5.x |
| **Estilos** | SCSS |
| **Estado** | Angular Signals |
| **Ubicación** | `frontend_angular/` |

---

## 📂 Estructura de Carpetas

```
frontend_angular/
├── src/
│   ├── app/
│   │   ├── core/                 # Servicios singleton, guards, interceptors
│   │   │   ├── config/           # Configuración de API
│   │   │   ├── guards/           # Guards de rutas (auth)
│   │   │   ├── interceptors/     # Interceptors HTTP
│   │   │   ├── services/         # Servicios globales
│   │   │   └── index.ts          # Barrel export
│   │   │
│   │   ├── features/             # Módulos funcionales (lazy loaded)
│   │   │   ├── auth/             # Autenticación (login)
│   │   │   ├── clientes/         # Gestión de clientes
│   │   │   ├── facturacion/      # Gestión de facturas
│   │   │   └── productos/        # Gestión de productos
│   │   │
│   │   ├── shared/               # Componentes y utilidades compartidas
│   │   │   ├── models/           # Interfaces y tipos
│   │   │   ├── styles/           # Variables y mixins SCSS
│   │   │   └── ui/               # Componentes UI reutilizables
│   │   │
│   │   ├── app.routes.ts         # Configuración de rutas
│   │   ├── app.config.ts         # Configuración de la app
│   │   └── app.ts                # Componente raíz
│   │
│   ├── environments/             # Configuración por entorno
│   │   ├── environment.ts        # Desarrollo
│   │   └── environment.prod.ts   # Producción
│   │
│   ├── styles.scss               # Estilos globales
│   └── main.ts                   # Punto de entrada
│
├── angular.json                  # Configuración Angular CLI
├── package.json                  # Dependencias
├── tsconfig.json                 # Configuración TypeScript
└── Dockerfile                    # Imagen Docker
```

---

## 🏗️ Patrones Arquitectónicos

### 1. Arquitectura por Features

Cada módulo funcional es independiente y auto-contenido:

```
features/clientes/
├── pages/                    # Componentes de página
│   ├── cliente-list/         # Listado de clientes
│   └── cliente-form/         # Formulario CRUD
├── services/                 # Servicios específicos
│   └── cliente.service.ts
└── clientes.routes.ts        # Rutas del módulo
```

### 2. Core Module (Singleton)

Servicios que deben existir una sola vez en la aplicación:

| Archivo | Propósito |
|---------|-----------|
| `auth.service.ts` | Autenticación y gestión de tokens |
| `base-http.service.ts` | Clase base para llamadas HTTP |
| `auth.guard.ts` | Protección de rutas privadas |
| `api.interceptor.ts` | Inyección de token en requests |
| `api.config.ts` | Configuración de URLs de API |

### 3. Shared Module

Recursos reutilizables en toda la aplicación:

| Carpeta | Contenido |
|---------|-----------|
| `models/` | Interfaces: Cliente, Producto, Factura, Auth |
| `ui/` | Componentes: Button, Input |
| `styles/` | Variables SCSS, Mixins |

---

## 🔐 Autenticación

### Flujo de Login

```
┌─────────┐    ┌─────────────┐    ┌─────────┐    ┌────────────┐
│  Login  │───>│ AuthService │───>│ Backend │───>│ LocalStorage│
│  Form   │<───│   login()   │<───│  /auth  │    │   Token    │
└─────────┘    └─────────────┘    └─────────┘    └────────────┘
```

### Almacenamiento

```typescript
// Claves en LocalStorage
TOKEN_KEY = 'auth_token';
USERNAME_KEY = 'auth_username';
```

### Guards

```typescript
// Protección de rutas
{
  path: 'facturacion',
  canActivate: [authGuard],  // Requiere autenticación
  loadChildren: () => import('./features/facturacion/...')
}
```

---

## 🛣️ Sistema de Rutas

### Rutas Principales

| Ruta | Módulo | Protegida |
|------|--------|-----------|
| `/login` | Auth | ❌ No |
| `/facturacion` | Facturación | ✅ Sí |
| `/clientes` | Clientes | ✅ Sí |
| `/productos` | Productos | ✅ Sí |

### Lazy Loading

Cada feature se carga bajo demanda:

```typescript
{
  path: 'clientes',
  canActivate: [authGuard],
  loadChildren: () =>
    import('./features/clientes/clientes.routes').then(m => m.CLIENTES_ROUTES)
}
```

---

## 📡 Comunicación con API

### Configuración

```typescript
// environment.ts (Desarrollo)
export const environment = {
  production: false,
  apiUrl: 'https://localhost:49497/api',
};

// environment.prod.ts (Producción)
export const environment = {
  production: true,
  apiUrl: '/api',
};
```

### Interceptor HTTP

El `api.interceptor.ts` inyecta automáticamente:
- Token de autenticación en headers
- Manejo de errores global

---

## 🎨 Estilos

### Variables SCSS

```scss
// _variables.scss
$primary-color: #007bff;
$secondary-color: #6c757d;
$success-color: #28a745;
$danger-color: #dc3545;
```

### Mixins

```scss
// _mixins.scss
@mixin flex-center { ... }
@mixin card-shadow { ... }
```

---

## 📦 Dependencias Principales

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| @angular/core | 19.x | Framework principal |
| @angular/router | 19.x | Enrutamiento |
| @angular/forms | 19.x | Formularios reactivos |
| rxjs | 7.x | Programación reactiva |

---

## 🚀 Comandos de Desarrollo

```bash
# Instalar dependencias
npm install

# Servidor de desarrollo
npm start
# o
ng serve

# Build de producción
npm run build
# o
ng build --configuration production

# Ejecutar tests
npm test

# Linting
npm run lint
```

---

## 🌐 URLs de Desarrollo

| Servicio | URL |
|----------|-----|
| Frontend | http://localhost:4200 |
| Backend API | https://localhost:49497/api |

---

## ✅ Buenas Prácticas Implementadas

1. **Lazy Loading** - Carga módulos bajo demanda
2. **Standalone Components** - Componentes independientes (Angular 19)
3. **Signals** - Estado reactivo moderno
4. **Barrel Exports** - `index.ts` para imports limpios
5. **Separación de Concerns** - Core, Features, Shared
6. **TypeScript Strict** - Tipado estricto
7. **SCSS Modular** - Variables y mixins reutilizables

