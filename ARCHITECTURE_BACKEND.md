# 🏗️ ARQUITECTURA BACKEND - Seven Facturación API

## 📋 Información General

| Atributo | Valor |
|----------|-------|
| **Nombre del Proyecto** | Seven Facturación API |
| **Framework** | .NET 10 |
| **Lenguaje** | C# 14 |
| **Arquitectura** | Clean Architecture |
| **Base de Datos** | PostgreSQL 15+ |
| **ORM** | Entity Framework Core 10 |
| **Documentación API** | Swagger/OpenAPI |

---

## 🏛️ Arquitectura Clean Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                         PRESENTATION LAYER                          │
│                      Seven.Facturacion.Api                          │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐     │
│  │ ClientesController│ │ProductosController│ │FacturasController│    │
│  └────────┬────────┘  └────────┬────────┘  └────────┬────────┘     │
└───────────┼─────────────────────┼─────────────────────┼─────────────┘
            │                     │                     │
            ▼                     ▼                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│                        APPLICATION LAYER                            │
│                   Seven.Facturacion.Application                     │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐     │
│  │ ClienteServicio │  │ProductoServicio │  │ FacturaServicio │     │
│  └────────┬────────┘  └────────┬────────┘  └────────┬────────┘     │
│           │                    │                    │               │
│  ┌────────┴────────────────────┴────────────────────┴────────┐     │
│  │                         DTOs                               │     │
│  │  ClienteDto, ProductoDto, FacturaDto, DetalleFacturaDto   │     │
│  └────────────────────────────────────────────────────────────┘     │
└───────────┼─────────────────────┼─────────────────────┼─────────────┘
            │                     │                     │
            ▼                     ▼                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│                          DOMAIN LAYER                               │
│                     Seven.Facturacion.Domain                        │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐     │
│  │     Cliente     │  │    Producto     │  │     Factura     │     │
│  └─────────────────┘  └─────────────────┘  └────────┬────────┘     │
│                                                      │              │
│  ┌─────────────────┐  ┌─────────────────────────────┴──────┐       │
│  │ IRepositorios   │  │           DetalleFactura           │       │
│  └─────────────────┘  └────────────────────────────────────┘       │
└───────────┼─────────────────────┼─────────────────────┼─────────────┘
            │                     │                     │
            ▼                     ▼                     ▼
┌─────────────────────────────────────────────────────────────────────┐
│                      INFRASTRUCTURE LAYER                           │
│                  Seven.Facturacion.Infrastructure                   │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐     │
│  │ClienteRepositorio│ │ProductoRepositorio│ │FacturaRepositorio│    │
│  └────────┬────────┘  └────────┬────────┘  └────────┬────────┘     │
│           │                    │                    │               │
│  ┌────────┴────────────────────┴────────────────────┴────────┐     │
│  │                      AppDbContext                          │     │
│  │              Entity Framework Core + Npgsql                │     │
│  └────────────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │     PostgreSQL 15+      │
                    │  Schema: facturacion    │
                    └─────────────────────────┘
```

---

## 📁 Estructura de Proyectos

```
Seven.Facturacion/
├── src/
│   ├── Seven.Facturacion.Api/              # Capa de Presentación
│   │   ├── Controladores/
│   │   │   ├── ClientesController.cs
│   │   │   ├── ProductosController.cs
│   │   │   └── FacturasController.cs
│   │   ├── Middlewares/
│   │   │   └── ManejadorExcepcionesMiddleware.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── Seven.Facturacion.Application/      # Capa de Aplicación
│   │   ├── Comun/
│   │   │   └── Resultado.cs
│   │   ├── DTOs/
│   │   │   ├── ClienteDto.cs
│   │   │   ├── ProductoDto.cs
│   │   │   └── FacturaDto.cs
│   │   ├── Extensiones/
│   │   │   └── MappingExtensions.cs
│   │   └── Servicios/
│   │       ├── IClienteServicio.cs
│   │       ├── ClienteServicio.cs
│   │       ├── IProductoServicio.cs
│   │       ├── ProductoServicio.cs
│   │       ├── IFacturaServicio.cs
│   │       └── FacturaServicio.cs
│   │
│   ├── Seven.Facturacion.Domain/           # Capa de Dominio
│   │   ├── Entidades/
│   │   │   ├── Cliente.cs
│   │   │   ├── Producto.cs
│   │   │   ├── Factura.cs
│   │   │   └── DetalleFactura.cs
│   │   ├── Excepciones/
│   │   │   └── DomainException.cs
│   │   └── Interfaces/
│   │       ├── IClienteRepositorio.cs
│   │       ├── IProductoRepositorio.cs
│   │       ├── IFacturaRepositorio.cs
│   │       └── IUnidadDeTrabajo.cs
│   │
│   └── Seven.Facturacion.Infrastructure/   # Capa de Infraestructura
│       ├── Persistencia/
│       │   ├── AppDbContext.cs
│       │   └── Configuraciones/
│       │       ├── ClienteConfiguracion.cs
│       │       ├── ProductoConfiguracion.cs
│       │       ├── FacturaConfiguracion.cs
│       │       └── DetalleFacturaConfiguracion.cs
│       ├── Repositorios/
│       │   ├── ClienteRepositorio.cs
│       │   ├── ProductoRepositorio.cs
│       │   ├── FacturaRepositorio.cs
│       │   └── UnidadDeTrabajo.cs
│       └── InyeccionDependencias.cs
│
└── Scripts/
    ├── 01_crear_tablas.sql
    ├── 02_insertar_datos.sql
    └── 03_consultas.sql
```

---

## 🗄️ Modelo de Base de Datos

### Diagrama Entidad-Relación

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              ESQUEMA: facturacion                           │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────┐       ┌─────────────────────┐       ┌─────────────────────┐
│      clientes       │       │      facturas       │       │      productos      │
├─────────────────────┤       ├─────────────────────┤       ├─────────────────────┤
│ PK id              │───┐   │ PK id              │   ┌───│ PK id              │
│    nombre          │   │   │    numero_factura   │   │   │    codigo          │
│    apellido        │   │   │ FK cliente_id      │◄──┘   │    nombre          │
│    correo_electronico│   └──►│    fecha           │       │    descripcion     │
│    telefono        │       │    subtotal        │       │    precio          │
│    fecha_nacimiento│       │    impuesto        │       │    stock           │
│    direccion       │       │    total           │       │    activo          │
│    activo          │       │    estado          │       │    fecha_creacion  │
│    fecha_creacion  │       │    fecha_creacion  │       │    fecha_actualizacion│
│    fecha_actualizacion│     │    fecha_actualizacion│     └─────────┬───────────┘
└─────────────────────┘       └─────────┬───────────┘                 │
                                        │                             │
                                        │      ┌──────────────────────┘
                                        │      │
                                        ▼      ▼
                              ┌─────────────────────┐
                              │  detalles_factura   │
                              ├─────────────────────┤
                              │ PK id              │
                              │ FK factura_id      │
                              │ FK producto_id     │
                              │    cantidad        │
                              │    precio_unitario │
                              │    subtotal (GEN)  │
                              └─────────────────────┘
```

### Tablas Detalladas

#### 📋 Tabla: `clientes`

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `id` | SERIAL | NO | AUTO | Identificador único (PK) |
| `nombre` | VARCHAR(100) | NO | - | Nombre del cliente |
| `apellido` | VARCHAR(100) | NO | - | Apellido del cliente |
| `correo_electronico` | VARCHAR(255) | NO | - | Email único (UK) |
| `telefono` | VARCHAR(20) | SI | NULL | Teléfono de contacto |
| `fecha_nacimiento` | DATE | NO | - | Fecha de nacimiento |
| `direccion` | VARCHAR(500) | SI | NULL | Dirección física |
| `activo` | BOOLEAN | NO | TRUE | Estado activo/inactivo |
| `fecha_creacion` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Fecha de creación |
| `fecha_actualizacion` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Última actualización |

**Constraints:**
- `uk_clientes_correo`: UNIQUE (correo_electronico)
- `ck_clientes_correo_formato`: CHECK regex email válido
- `ck_clientes_fecha_nacimiento`: CHECK (fecha_nacimiento <= CURRENT_DATE)

**Índices:**
- `idx_clientes_nombre_apellido`: (nombre, apellido)
- `idx_clientes_activo`: (activo) WHERE activo = TRUE
- `idx_clientes_fecha_nacimiento`: (fecha_nacimiento)

---

#### 📦 Tabla: `productos`

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `id` | SERIAL | NO | AUTO | Identificador único (PK) |
| `codigo` | VARCHAR(50) | NO | - | Código SKU único (UK) |
| `nombre` | VARCHAR(200) | NO | - | Nombre del producto |
| `descripcion` | TEXT | SI | NULL | Descripción detallada |
| `precio` | DECIMAL(18,2) | NO | - | Precio unitario |
| `stock` | INTEGER | NO | 0 | Cantidad en inventario |
| `activo` | BOOLEAN | NO | TRUE | Producto activo |
| `fecha_creacion` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Fecha de creación |
| `fecha_actualizacion` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Última actualización |

**Constraints:**
- `uk_productos_codigo`: UNIQUE (codigo)
- `ck_productos_precio_positivo`: CHECK (precio > 0)
- `ck_productos_stock_no_negativo`: CHECK (stock >= 0)

**Índices:**
- `idx_productos_nombre`: (nombre)
- `idx_productos_activo`: (activo) WHERE activo = TRUE
- `idx_productos_stock_bajo`: (stock) WHERE stock <= 5

---

#### 🧾 Tabla: `facturas`

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `id` | SERIAL | NO | AUTO | Identificador único (PK) |
| `numero_factura` | VARCHAR(20) | NO | - | Número único (UK) |
| `cliente_id` | INTEGER | NO | - | FK a clientes |
| `fecha` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Fecha de emisión |
| `subtotal` | DECIMAL(18,2) | NO | 0 | Subtotal sin IVA |
| `impuesto` | DECIMAL(18,2) | NO | 0 | IVA (19%) |
| `total` | DECIMAL(18,2) | NO | 0 | Total con IVA |
| `estado` | VARCHAR(20) | NO | 'PENDIENTE' | Estado de la factura |
| `fecha_creacion` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Fecha de creación |
| `fecha_actualizacion` | TIMESTAMP | NO | CURRENT_TIMESTAMP | Última actualización |

**Constraints:**
- `uk_facturas_numero`: UNIQUE (numero_factura)
- `fk_facturas_cliente`: FOREIGN KEY (cliente_id) REFERENCES clientes(id)
- `ck_facturas_estado`: CHECK (estado IN ('PENDIENTE', 'PAGADA', 'ANULADA'))
- `ck_facturas_totales_no_negativos`: CHECK (subtotal >= 0 AND impuesto >= 0 AND total >= 0)

**Índices:**
- `idx_facturas_cliente`: (cliente_id)
- `idx_facturas_fecha`: (fecha)
- `idx_facturas_estado`: (estado)
- `idx_facturas_fecha_year`: (EXTRACT(YEAR FROM fecha))

---

#### 📝 Tabla: `detalles_factura`

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `id` | SERIAL | NO | AUTO | Identificador único (PK) |
| `factura_id` | INTEGER | NO | - | FK a facturas |
| `producto_id` | INTEGER | NO | - | FK a productos |
| `cantidad` | INTEGER | NO | - | Cantidad de productos |
| `precio_unitario` | DECIMAL(18,2) | NO | - | Precio al momento de venta |
| `subtotal` | DECIMAL(18,2) | NO | GENERATED | Columna calculada |

**Constraints:**
- `fk_detalles_factura`: FOREIGN KEY (factura_id) REFERENCES facturas(id) ON DELETE CASCADE
- `fk_detalles_producto`: FOREIGN KEY (producto_id) REFERENCES productos(id) ON DELETE RESTRICT
- `ck_detalles_cantidad_positiva`: CHECK (cantidad > 0)
- `ck_detalles_precio_positivo`: CHECK (precio_unitario > 0)

**Columna Generada:**
```sql
subtotal DECIMAL(18, 2) GENERATED ALWAYS AS (cantidad * precio_unitario) STORED
```

---

## 🌐 API REST - Endpoints

### Base URL
```
http://localhost:5000/api
```

### Swagger UI
```
http://localhost:5000/swagger
```

---

### 👥 Clientes (`/api/Clientes`)

| Método | Endpoint | Descripción | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/Clientes` | Obtener todos los clientes | - | `ClienteDto[]` |
| `GET` | `/api/Clientes/{id}` | Obtener cliente por ID | - | `ClienteDto` |
| `POST` | `/api/Clientes` | Crear nuevo cliente | `CrearClienteDto` | `ClienteDto` |
| `PUT` | `/api/Clientes/{id}` | Actualizar cliente | `ActualizarClienteDto` | `ClienteDto` |
| `DELETE` | `/api/Clientes/{id}` | Eliminar cliente | - | `204 No Content` |
| `GET` | `/api/Clientes/por-edad-y-compra` | Clientes ≤ edad con compras en fechas | Query params | `ClientePorEdadYCompraDto[]` |

#### DTOs de Cliente

**ClienteDto (Response)**
```json
{
  "id": 1,
  "nombre": "Carlos",
  "apellido": "Martínez",
  "nombreCompleto": "Carlos Martínez",
  "correoElectronico": "carlos.martinez@email.com",
  "telefono": "3001234567",
  "fechaNacimiento": "1992-05-15",
  "edad": 33,
  "direccion": "Calle 123 #45-67, Bogotá",
  "activo": true
}
```

**CrearClienteDto (Request)**
```json
{
  "nombre": "Carlos",
  "apellido": "Martínez",
  "correoElectronico": "carlos.martinez@email.com",
  "telefono": "3001234567",
  "fechaNacimiento": "1992-05-15",
  "direccion": "Calle 123 #45-67, Bogotá"
}
```

**Endpoint Especial: Clientes por Edad y Compra**
```
GET /api/Clientes/por-edad-y-compra?edadMaxima=35&fechaDesde=2000-02-01&fechaHasta=2000-05-25
```

---

### 📦 Productos (`/api/Productos`)

| Método | Endpoint | Descripción | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/Productos` | Obtener todos los productos | - | `ProductoDto[]` |
| `GET` | `/api/Productos/{id}` | Obtener producto por ID | - | `ProductoDto` |
| `POST` | `/api/Productos` | Crear nuevo producto | `CrearProductoDto` | `ProductoDto` |
| `PUT` | `/api/Productos/{id}` | Actualizar producto | `ActualizarProductoDto` | `ProductoDto` |
| `DELETE` | `/api/Productos/{id}` | Eliminar producto | - | `204 No Content` |
| `GET` | `/api/Productos/lista-precios` | Lista de precios activos | - | `ListaPrecioDto[]` |
| `GET` | `/api/Productos/stock-bajo` | Productos con stock ≤ 5 | Query: stockMinimo | `ProductoBajoStockDto[]` |

#### DTOs de Producto

**ProductoDto (Response)**
```json
{
  "id": 1,
  "codigo": "PROD-001",
  "nombre": "Laptop HP ProBook 450",
  "descripcion": "Laptop empresarial 15.6\" Intel i7 16GB RAM",
  "precio": 2500000.00,
  "stock": 15,
  "activo": true,
  "tieneStockBajo": false,
  "estaAgotado": false
}
```

**ListaPrecioDto (Response)**
```json
{
  "productoId": 1,
  "codigo": "PROD-001",
  "nombre": "Laptop HP ProBook 450",
  "precio": 2500000.00,
  "stock": 15
}
```

**ProductoBajoStockDto (Response)**
```json
{
  "productoId": 3,
  "codigo": "PROD-003",
  "nombre": "Teclado Mecánico Logitech",
  "stock": 3,
  "precio": 350000.00,
  "nivelAlerta": "BAJO"
}
```

---

### 🧾 Facturas (`/api/Facturas`)

| Método | Endpoint | Descripción | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/Facturas` | Obtener todas las facturas | - | `FacturaDto[]` |
| `GET` | `/api/Facturas/{id}` | Obtener factura por ID | - | `FacturaDto` |
| `POST` | `/api/Facturas` | Crear nueva factura | `CrearFacturaDto` | `FacturaDto` |
| `PATCH` | `/api/Facturas/{id}/estado` | Actualizar estado | `ActualizarEstadoFacturaDto` | `FacturaDto` |
| `DELETE` | `/api/Facturas/{id}` | Eliminar factura | - | `204 No Content` |
| `GET` | `/api/Facturas/cliente/{clienteId}` | Facturas por cliente | - | `FacturaDto[]` |
| `GET` | `/api/Facturas/ventas-por-producto` | Ventas por producto/año | Query: anio | `VentasPorProductoDto[]` |
| `GET` | `/api/Facturas/proxima-compra/{clienteId}` | Estimación próxima compra | - | `ProximaCompraClienteDto` |

#### DTOs de Factura

**FacturaDto (Response)**
```json
{
  "id": 1,
  "numeroFactura": "FAC-20000215001",
  "clienteId": 1,
  "nombreCliente": "Carlos Martínez",
  "fecha": "2000-02-15T10:30:00",
  "subtotal": 2850000.00,
  "impuesto": 541500.00,
  "total": 3391500.00,
  "estado": "PAGADA",
  "detalles": [
    {
      "id": 1,
      "productoId": 1,
      "nombreProducto": "Laptop HP ProBook 450",
      "cantidad": 1,
      "precioUnitario": 2500000.00,
      "subtotal": 2500000.00
    }
  ]
}
```

**CrearFacturaDto (Request)**
```json
{
  "clienteId": 1,
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2
    },
    {
      "productoId": 3,
      "cantidad": 1
    }
  ]
}
```

**VentasPorProductoDto (Response)**
```json
{
  "productoId": 1,
  "codigo": "PROD-001",
  "nombre": "Laptop HP ProBook 450",
  "cantidadTotalVendida": 3,
  "montoTotalVendido": 7500000.00,
  "numeroFacturas": 3,
  "precioPromedioVenta": 2500000.00
}
```

**ProximaCompraClienteDto (Response)**
```json
{
  "clienteId": 1,
  "nombreCompleto": "Carlos Martínez",
  "totalCompras": 5,
  "primeraCompra": "2000-02-15",
  "ultimaCompra": "2024-12-05",
  "promedioDiasEntreCompras": 32,
  "proximaCompraEstimada": "2025-01-06",
  "estadoPrediccion": "PRÓXIMA"
}
```

---

## 📊 Consultas SQL Implementadas

### Consulta 1: Lista de Precios Activos
**Endpoint:** `GET /api/Productos/lista-precios`

```sql
SELECT 
    id              AS producto_id,
    codigo          AS codigo_producto,
    nombre          AS nombre_producto,
    precio          AS precio_unitario,
    stock
FROM facturacion.productos
WHERE activo = TRUE
ORDER BY nombre ASC;
```

---

### Consulta 2: Productos con Stock Bajo
**Endpoint:** `GET /api/Productos/stock-bajo?stockMinimo=5`

```sql
SELECT 
    id              AS producto_id,
    codigo          AS codigo_producto,
    nombre          AS nombre_producto,
    stock           AS cantidad_disponible,
    precio          AS precio_unitario,
    CASE 
        WHEN stock = 0 THEN 'AGOTADO'
        WHEN stock <= 2 THEN 'CRÍTICO'
        ELSE 'BAJO'
    END             AS nivel_alerta
FROM facturacion.productos
WHERE stock <= @stockMinimo
  AND activo = TRUE
ORDER BY stock ASC, nombre ASC;
```

---

### Consulta 3: Clientes Jóvenes con Compras en Rango
**Endpoint:** `GET /api/Clientes/por-edad-y-compra?edadMaxima=35&fechaDesde=2000-02-01&fechaHasta=2000-05-25`

```sql
SELECT DISTINCT
    c.id                    AS cliente_id,
    c.nombre                AS nombre,
    c.apellido              AS apellido,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_completo,
    c.correo_electronico    AS correo,
    c.fecha_nacimiento      AS fecha_nacimiento,
    EXTRACT(YEAR FROM AGE(CURRENT_DATE, c.fecha_nacimiento))::INTEGER AS edad,
    COUNT(f.id) OVER (PARTITION BY c.id) AS total_compras_periodo
FROM facturacion.clientes c
INNER JOIN facturacion.facturas f ON c.id = f.cliente_id
WHERE EXTRACT(YEAR FROM AGE(CURRENT_DATE, c.fecha_nacimiento)) <= @edadMaxima
  AND f.fecha >= @fechaDesde
  AND f.fecha <= @fechaHasta
  AND c.activo = TRUE
ORDER BY c.apellido, c.nombre;
```

---

### Consulta 4: Ventas por Producto en un Año
**Endpoint:** `GET /api/Facturas/ventas-por-producto?anio=2000`

```sql
SELECT 
    p.id                    AS producto_id,
    p.codigo                AS codigo_producto,
    p.nombre                AS nombre_producto,
    SUM(df.cantidad)        AS cantidad_total_vendida,
    SUM(df.subtotal)        AS monto_total_vendido,
    COUNT(DISTINCT f.id)    AS numero_facturas,
    ROUND(AVG(df.precio_unitario), 2) AS precio_promedio_venta
FROM facturacion.detalles_factura df
INNER JOIN facturacion.productos p ON df.producto_id = p.id
INNER JOIN facturacion.facturas f ON df.factura_id = f.id
WHERE EXTRACT(YEAR FROM f.fecha) = @anio
  AND f.estado != 'ANULADA'
GROUP BY p.id, p.codigo, p.nombre
ORDER BY monto_total_vendido DESC;
```

---

### Consulta 5: Estimación Próxima Compra
**Endpoint:** `GET /api/Facturas/proxima-compra/{clienteId}`

```sql
SELECT 
    c.id                    AS cliente_id,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_completo,
    c.correo_electronico    AS correo,
    estadisticas.total_compras,
    estadisticas.primera_compra,
    estadisticas.ultima_compra,
    estadisticas.promedio_dias_entre_compras,
    (estadisticas.ultima_compra + 
        (estadisticas.promedio_dias_entre_compras || ' days')::INTERVAL)::DATE 
        AS proxima_compra_estimada,
    CASE 
        WHEN (estadisticas.ultima_compra + 
            (estadisticas.promedio_dias_entre_compras || ' days')::INTERVAL) < CURRENT_DATE 
        THEN 'VENCIDA'
        WHEN (estadisticas.ultima_compra + 
            (estadisticas.promedio_dias_entre_compras || ' days')::INTERVAL) <= CURRENT_DATE + INTERVAL '7 days'
        THEN 'PRÓXIMA'
        ELSE 'FUTURA'
    END AS estado_prediccion
FROM facturacion.clientes c
INNER JOIN (
    SELECT 
        cliente_id,
        COUNT(*) AS total_compras,
        MIN(fecha) AS primera_compra,
        MAX(fecha) AS ultima_compra,
        CASE 
            WHEN COUNT(*) > 1 
            THEN ROUND(EXTRACT(EPOCH FROM (MAX(fecha) - MIN(fecha))) / 86400 / (COUNT(*) - 1))::INTEGER
            ELSE NULL
        END AS promedio_dias_entre_compras
    FROM facturacion.facturas
    WHERE estado != 'ANULADA'
    GROUP BY cliente_id
    HAVING COUNT(*) >= 2
) estadisticas ON c.id = estadisticas.cliente_id
WHERE c.id = @clienteId
  AND c.activo = TRUE;
```

---

## ⚙️ Configuración

### Cadena de Conexión (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=seven_facturacion_dev;Username=postgres;Password=postgres"
  }
}
```

### Variables de Entorno
| Variable | Descripción | Valor Default |
|----------|-------------|---------------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente de ejecución | Development |
| `ASPNETCORE_URLS` | URLs de escucha | http://localhost:5000 |

---

## 🚀 Comandos de Ejecución

### Iniciar la API
```bash
cd c:\Users\willi\OneDrive\Escritorio\digitalware
dotnet run --project src/Seven.Facturacion.Api --urls "http://localhost:5000"
```

### Detener la API
```
Ctrl + C
```

### Compilar Solución
```bash
dotnet build
```

### Crear Base de Datos (PostgreSQL)
```bash
# Ejecutar desde psql
psql -U postgres -c "CREATE DATABASE seven_facturacion_dev;"
psql -U postgres -d seven_facturacion_dev -f Scripts/01_crear_tablas.sql
psql -U postgres -d seven_facturacion_dev -f Scripts/02_insertar_datos.sql

# Mover tablas al esquema facturacion
psql -U postgres -d seven_facturacion_dev -c "CREATE SCHEMA IF NOT EXISTS facturacion;"
psql -U postgres -d seven_facturacion_dev -c "ALTER TABLE detalles_factura SET SCHEMA facturacion; ALTER TABLE facturas SET SCHEMA facturacion; ALTER TABLE productos SET SCHEMA facturacion; ALTER TABLE clientes SET SCHEMA facturacion;"
```

---

## 📦 Dependencias NuGet

### Seven.Facturacion.Api
- `Swashbuckle.AspNetCore` - Swagger/OpenAPI

### Seven.Facturacion.Infrastructure
- `Microsoft.EntityFrameworkCore` 10.x
- `Npgsql.EntityFrameworkCore.PostgreSQL` 10.x
- `Microsoft.EntityFrameworkCore.InMemory` 10.x (opcional)

---

## 🔒 Códigos de Estado HTTP

| Código | Descripción | Uso |
|--------|-------------|-----|
| `200 OK` | Éxito | GET, PUT, PATCH exitosos |
| `201 Created` | Recurso creado | POST exitoso |
| `204 No Content` | Éxito sin contenido | DELETE exitoso |
| `400 Bad Request` | Error de validación | Datos inválidos |
| `404 Not Found` | No encontrado | Recurso inexistente |
| `409 Conflict` | Conflicto | Duplicado (email, código) |
| `500 Internal Server Error` | Error servidor | Excepción no controlada |

---

## 📝 Notas Técnicas

### Características de C# 14 Utilizadas
- **Collection expressions**: `ICollection<T> = []`
- **Primary constructors**: `class Controller(IService service)`
- **Required members**: `required string Nombre`
- **File-scoped namespaces**
- **Pattern matching avanzado**

### Características de .NET 10 Utilizadas
- **Minimal APIs** (endpoint raíz)
- **DateOnly** para fechas sin hora
- **Global usings**
- **Nullable reference types**

---

**Documentación generada el:** 2025-12-28  
**Versión:** 1.0.0  
**Autor:** Seven Facturación Team

