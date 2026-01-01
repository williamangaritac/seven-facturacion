-- ============================================================================
-- SISTEMA DE FACTURACIÓN - SCRIPT DE CREACIÓN DE TABLAS
-- Base de datos: PostgreSQL 16
-- Autor: Sistema de Facturación Seven
-- Fecha: 2024
-- ============================================================================

-- Usar el esquema facturacion
SET search_path TO facturacion, public;

-- Eliminar tablas si existen (en orden inverso por dependencias)
DROP TABLE IF EXISTS facturacion.detalles_factura CASCADE;
DROP TABLE IF EXISTS facturacion.facturas CASCADE;
DROP TABLE IF EXISTS facturacion.productos CASCADE;
DROP TABLE IF EXISTS facturacion.clientes CASCADE;

-- ============================================================================
-- TABLA: clientes
-- Descripción: Almacena la información de los clientes del sistema
-- ============================================================================
CREATE TABLE clientes (
    id                  SERIAL PRIMARY KEY,
    nombre              VARCHAR(100) NOT NULL,
    apellido            VARCHAR(100) NOT NULL,
    correo_electronico  VARCHAR(255) NOT NULL,
    telefono            VARCHAR(20),
    fecha_nacimiento    DATE NOT NULL,
    direccion           VARCHAR(500),
    activo              BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Constraints
    CONSTRAINT uk_clientes_correo UNIQUE (correo_electronico),
    CONSTRAINT ck_clientes_correo_formato CHECK (correo_electronico ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
    CONSTRAINT ck_clientes_fecha_nacimiento CHECK (fecha_nacimiento <= CURRENT_DATE)
);

-- Índices para búsquedas frecuentes
CREATE INDEX idx_clientes_nombre_apellido ON clientes (nombre, apellido);
CREATE INDEX idx_clientes_activo ON clientes (activo) WHERE activo = TRUE;
CREATE INDEX idx_clientes_fecha_nacimiento ON clientes (fecha_nacimiento);

COMMENT ON TABLE clientes IS 'Tabla principal de clientes del sistema de facturación';
COMMENT ON COLUMN clientes.id IS 'Identificador único del cliente';
COMMENT ON COLUMN clientes.correo_electronico IS 'Correo electrónico único del cliente';
COMMENT ON COLUMN clientes.fecha_nacimiento IS 'Fecha de nacimiento para cálculo de edad';

-- ============================================================================
-- TABLA: productos
-- Descripción: Catálogo de productos disponibles para facturación
-- ============================================================================
CREATE TABLE productos (
    id                  SERIAL PRIMARY KEY,
    codigo              VARCHAR(50) NOT NULL,
    nombre              VARCHAR(200) NOT NULL,
    descripcion         TEXT,
    precio              DECIMAL(18, 2) NOT NULL,
    stock               INTEGER NOT NULL DEFAULT 0,
    activo              BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Constraints
    CONSTRAINT uk_productos_codigo UNIQUE (codigo),
    CONSTRAINT ck_productos_precio_positivo CHECK (precio > 0),
    CONSTRAINT ck_productos_stock_no_negativo CHECK (stock >= 0)
);

-- Índices para búsquedas frecuentes
CREATE INDEX idx_productos_nombre ON productos (nombre);
CREATE INDEX idx_productos_activo ON productos (activo) WHERE activo = TRUE;
CREATE INDEX idx_productos_stock_bajo ON productos (stock) WHERE stock <= 5;

COMMENT ON TABLE productos IS 'Catálogo de productos del sistema de facturación';
COMMENT ON COLUMN productos.codigo IS 'Código único del producto';
COMMENT ON COLUMN productos.precio IS 'Precio unitario del producto';
COMMENT ON COLUMN productos.stock IS 'Cantidad disponible en inventario';

-- ============================================================================
-- TABLA: facturas
-- Descripción: Encabezado de facturas emitidas
-- ============================================================================
CREATE TABLE facturas (
    id                  SERIAL PRIMARY KEY,
    numero_factura      VARCHAR(20) NOT NULL,
    cliente_id          INTEGER NOT NULL,
    fecha               TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal            DECIMAL(18, 2) NOT NULL DEFAULT 0,
    impuesto            DECIMAL(18, 2) NOT NULL DEFAULT 0,
    total               DECIMAL(18, 2) NOT NULL DEFAULT 0,
    estado              VARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',
    fecha_creacion      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fecha_actualizacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    -- Constraints
    CONSTRAINT uk_facturas_numero UNIQUE (numero_factura),
    CONSTRAINT fk_facturas_cliente FOREIGN KEY (cliente_id) REFERENCES clientes(id) ON DELETE RESTRICT,
    CONSTRAINT ck_facturas_estado CHECK (estado IN ('PENDIENTE', 'PAGADA', 'ANULADA')),
    CONSTRAINT ck_facturas_totales_no_negativos CHECK (subtotal >= 0 AND impuesto >= 0 AND total >= 0)
);

-- Índices para búsquedas frecuentes
CREATE INDEX idx_facturas_cliente ON facturas (cliente_id);
CREATE INDEX idx_facturas_fecha ON facturas (fecha);
CREATE INDEX idx_facturas_estado ON facturas (estado);
CREATE INDEX idx_facturas_fecha_year ON facturas (EXTRACT(YEAR FROM fecha));

COMMENT ON TABLE facturas IS 'Encabezado de facturas del sistema';
COMMENT ON COLUMN facturas.numero_factura IS 'Número único de factura formato FAC-YYYYMMDDHHMMSS';
COMMENT ON COLUMN facturas.estado IS 'Estado de la factura: PENDIENTE, PAGADA, ANULADA';
COMMENT ON COLUMN facturas.impuesto IS 'IVA calculado al 19%';

-- ============================================================================
-- TABLA: detalles_factura
-- Descripción: Líneas de detalle de cada factura
-- ============================================================================
CREATE TABLE detalles_factura (
    id                  SERIAL PRIMARY KEY,
    factura_id          INTEGER NOT NULL,
    producto_id         INTEGER NOT NULL,
    cantidad            INTEGER NOT NULL,
    precio_unitario     DECIMAL(18, 2) NOT NULL,
    subtotal            DECIMAL(18, 2) GENERATED ALWAYS AS (cantidad * precio_unitario) STORED,
    
    -- Constraints
    CONSTRAINT fk_detalles_factura FOREIGN KEY (factura_id) REFERENCES facturas(id) ON DELETE CASCADE,
    CONSTRAINT fk_detalles_producto FOREIGN KEY (producto_id) REFERENCES productos(id) ON DELETE RESTRICT,
    CONSTRAINT ck_detalles_cantidad_positiva CHECK (cantidad > 0),
    CONSTRAINT ck_detalles_precio_positivo CHECK (precio_unitario > 0)
);

-- Índices para búsquedas frecuentes
CREATE INDEX idx_detalles_factura ON detalles_factura (factura_id);
CREATE INDEX idx_detalles_producto ON detalles_factura (producto_id);

COMMENT ON TABLE detalles_factura IS 'Líneas de detalle de las facturas';
COMMENT ON COLUMN detalles_factura.subtotal IS 'Columna calculada: cantidad * precio_unitario';
COMMENT ON COLUMN detalles_factura.precio_unitario IS 'Precio al momento de la venta (histórico)';

-- ============================================================================
-- FIN DEL SCRIPT
-- ============================================================================

