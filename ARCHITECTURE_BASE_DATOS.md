# 🗄️ Arquitectura de Base de Datos - Sistema de Facturación Seven

## 📋 Información General

| Aspecto | Detalle |
|---------|---------|
| **Motor** | PostgreSQL 16 |
| **Esquema** | `facturacion` |
| **Ubicación Scripts** | `app_backend/Scripts/` |
| **Puerto** | 5432 |

---

## 📊 Diagrama Entidad-Relación

```
┌─────────────────┐       ┌─────────────────┐
│    USUARIOS     │       │    CLIENTES     │
├─────────────────┤       ├─────────────────┤
│ id (PK)         │       │ id (PK)         │
│ username        │       │ nombre          │
│ password_hash   │       │ apellido        │
│ fecha_creacion  │       │ correo_electronico│
│ activo          │       │ telefono        │
└─────────────────┘       │ fecha_nacimiento│
                          │ direccion       │
                          │ activo          │
                          └────────┬────────┘
                                   │
                                   │ 1:N
                                   ▼
┌─────────────────┐       ┌─────────────────┐
│    PRODUCTOS    │       │    FACTURAS     │
├─────────────────┤       ├─────────────────┤
│ id (PK)         │       │ id (PK)         │
│ codigo          │       │ numero_factura  │
│ nombre          │       │ cliente_id (FK) │◄────┐
│ descripcion     │       │ fecha           │     │
│ precio          │       │ subtotal        │     │
│ stock           │       │ impuesto        │     │
│ activo          │       │ total           │     │
└────────┬────────┘       │ estado          │     │
         │                └────────┬────────┘     │
         │                         │              │
         │ N:M                     │ 1:N          │
         │                         ▼              │
         │              ┌──────────────────────┐  │
         └─────────────►│  DETALLES_FACTURA    │  │
                        ├──────────────────────┤  │
                        │ id (PK)              │  │
                        │ factura_id (FK)      │──┘
                        │ producto_id (FK)     │
                        │ cantidad             │
                        │ precio_unitario      │
                        │ subtotal (calculado) │
                        └──────────────────────┘
```

---

## 📋 Tablas del Sistema

### 1. `clientes`

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `id` | SERIAL | PK | Identificador único |
| `nombre` | VARCHAR(100) | NOT NULL | Nombre del cliente |
| `apellido` | VARCHAR(100) | NOT NULL | Apellido del cliente |
| `correo_electronico` | VARCHAR(255) | NOT NULL, UNIQUE | Email único |
| `telefono` | VARCHAR(20) | NULL | Teléfono de contacto |
| `fecha_nacimiento` | DATE | NOT NULL | Fecha de nacimiento |
| `direccion` | VARCHAR(500) | NULL | Dirección completa |
| `activo` | BOOLEAN | NOT NULL | Estado del cliente |
| `fecha_creacion` | TIMESTAMP | NOT NULL | Fecha de creación |
| `fecha_actualizacion` | TIMESTAMP | NOT NULL | Última actualización |

**Constraints:**
- `uk_clientes_correo` - Email único
- `ck_clientes_correo_formato` - Validación formato email
- `ck_clientes_fecha_nacimiento` - Fecha <= hoy

### 2. `productos`

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `id` | SERIAL | PK | Identificador único |
| `codigo` | VARCHAR(50) | NOT NULL, UNIQUE | Código único |
| `nombre` | VARCHAR(200) | NOT NULL | Nombre del producto |
| `descripcion` | TEXT | NULL | Descripción detallada |
| `precio` | DECIMAL(18,2) | NOT NULL | Precio unitario |
| `stock` | INTEGER | NOT NULL | Cantidad en inventario |
| `activo` | BOOLEAN | NOT NULL | Estado del producto |
| `fecha_creacion` | TIMESTAMP | NOT NULL | Fecha de creación |
| `fecha_actualizacion` | TIMESTAMP | NOT NULL | Última actualización |

**Constraints:**
- `uk_productos_codigo` - Código único
- `ck_productos_precio_positivo` - Precio > 0
- `ck_productos_stock_no_negativo` - Stock >= 0

### 3. `facturas`

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `id` | SERIAL | PK | Identificador único |
| `numero_factura` | VARCHAR(20) | NOT NULL, UNIQUE | Número FAC-YYYYMMDDHHMMSS |
| `cliente_id` | INTEGER | NOT NULL, FK | Referencia al cliente |
| `fecha` | TIMESTAMP | NOT NULL | Fecha de emisión |
| `subtotal` | DECIMAL(18,2) | NOT NULL | Subtotal sin impuestos |
| `impuesto` | DECIMAL(18,2) | NOT NULL | IVA (19%) |
| `total` | DECIMAL(18,2) | NOT NULL | Total con impuestos |
| `estado` | VARCHAR(20) | NOT NULL | PENDIENTE, PAGADA, ANULADA |
| `fecha_creacion` | TIMESTAMP | NOT NULL | Fecha de creación |
| `fecha_actualizacion` | TIMESTAMP | NOT NULL | Última actualización |

**Constraints:**
- `uk_facturas_numero` - Número único
- `fk_facturas_cliente` - FK a clientes (ON DELETE RESTRICT)
- `ck_facturas_estado` - Valores válidos de estado
- `ck_facturas_totales_no_negativos` - Valores >= 0

### 4. `detalles_factura`

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `id` | SERIAL | PK | Identificador único |
| `factura_id` | INTEGER | NOT NULL, FK | Referencia a factura |
| `producto_id` | INTEGER | NOT NULL, FK | Referencia a producto |
| `cantidad` | INTEGER | NOT NULL | Cantidad vendida |
| `precio_unitario` | DECIMAL(18,2) | NOT NULL | Precio al momento de venta |
| `subtotal` | DECIMAL(18,2) | GENERATED | Calculado: cantidad × precio |

**Constraints:**
- `fk_detalles_factura` - FK a facturas (ON DELETE CASCADE)
- `fk_detalles_producto` - FK a productos (ON DELETE RESTRICT)
- `ck_detalles_cantidad_positiva` - Cantidad > 0
- `ck_detalles_precio_positivo` - Precio > 0

### 5. `usuarios`

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `id` | SERIAL | PK | Identificador único |
| `username` | VARCHAR(50) | NOT NULL, UNIQUE | Nombre de usuario |
| `password_hash` | VARCHAR(255) | NOT NULL | Contraseña BCrypt |
| `fecha_creacion` | TIMESTAMP | NOT NULL | Fecha de creación |
| `activo` | BOOLEAN | NOT NULL | Estado del usuario |

---

## 🔗 Relaciones

| Relación | Tipo | Descripción |
|----------|------|-------------|
| `clientes` → `facturas` | 1:N | Un cliente tiene muchas facturas |
| `facturas` → `detalles_factura` | 1:N | Una factura tiene muchos detalles |
| `productos` → `detalles_factura` | 1:N | Un producto en muchos detalles |

---

## 📑 Índices

### Clientes
- `idx_clientes_nombre_apellido` - Búsqueda por nombre
- `idx_clientes_activo` - Filtro de activos
- `idx_clientes_fecha_nacimiento` - Ordenamiento por fecha

### Productos
- `idx_productos_nombre` - Búsqueda por nombre
- `idx_productos_activo` - Filtro de activos
- `idx_productos_stock_bajo` - Alertas de stock bajo

### Facturas
- `idx_facturas_cliente` - Facturas por cliente
- `idx_facturas_fecha` - Ordenamiento por fecha
- `idx_facturas_estado` - Filtro por estado
- `idx_facturas_fecha_year` - Reportes anuales

### Detalles
- `idx_detalles_factura` - Detalles por factura
- `idx_detalles_producto` - Detalles por producto

### Usuarios
- `idx_usuarios_username` - Búsqueda por username

---

## 📜 Scripts SQL

| Script | Propósito |
|--------|-----------|
| `00_crear_esquema.sql` | Crea esquema `facturacion` |
| `01_crear_tablas.sql` | Crea todas las tablas |
| `02_insertar_datos.sql` | Datos de prueba |
| `03_consultas.sql` | Consultas de ejemplo |
| `04_crear_tabla_usuarios.sql` | Tabla de usuarios |
| `05_actualizar_password_admin.sql` | Password del admin |

---

## ⚙️ Configuración de Conexión

### Desarrollo

```
Host: localhost
Port: 5432
Database: seven_facturacion_dev
Username: postgres
Password: postgres
```

### Connection String (.NET)

```
Host=localhost;Port=5432;Database=seven_facturacion_dev;Username=postgres;Password=postgres
```

---

## 🔐 Seguridad

### Contraseñas
- **Algoritmo:** BCrypt
- **Work Factor:** 11 rounds
- **Usuario por defecto:** admin / admin123

### Restricciones
- `ON DELETE RESTRICT` - Protege integridad referencial
- `ON DELETE CASCADE` - Solo en detalles de factura
- Validaciones CHECK en todas las tablas

---

## 🐳 Docker

```yaml
postgres:
  image: postgres:16
  environment:
    POSTGRES_DB: seven_facturacion_dev
    POSTGRES_USER: postgres
    POSTGRES_PASSWORD: postgres
  ports:
    - "5432:5432"
  volumes:
    - ./app_backend/Scripts/:/docker-entrypoint-initdb.d/
```

---

## 📊 Cálculos Automáticos

### IVA
```sql
impuesto = subtotal * 0.19  -- 19%
total = subtotal + impuesto
```

### Subtotal de Detalle
```sql
-- Columna generada automáticamente
subtotal DECIMAL(18, 2) GENERATED ALWAYS AS (cantidad * precio_unitario) STORED
```

---

## ✅ Buenas Prácticas Implementadas

1. **Normalización** - 3ra forma normal
2. **Integridad Referencial** - Foreign keys definidas
3. **Validaciones** - CHECK constraints
4. **Índices Optimizados** - Para consultas frecuentes
5. **Columnas de Auditoría** - fecha_creacion, fecha_actualizacion
6. **Soft Delete** - Campo `activo` en lugar de borrar
7. **Comentarios** - Documentación en tablas y columnas
8. **Esquema Separado** - Aislamiento en `facturacion`

