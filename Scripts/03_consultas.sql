-- ============================================================================
-- SISTEMA DE FACTURACIÓN - CONSULTAS SQL DE NEGOCIO
-- Base de datos: PostgreSQL 15.13
-- Autor: Sistema de Facturación Seven
-- Descripción: 5 consultas obligatorias especificadas en requerimientos
-- ============================================================================

-- ============================================================================
-- CONSULTA 1: Lista de precios de productos activos
-- Descripción: Obtiene el catálogo de precios ordenado alfabéticamente
-- Uso: Endpoint GET /api/productos/lista-precios
-- ============================================================================
SELECT 
    id              AS producto_id,
    codigo          AS codigo_producto,
    nombre          AS nombre_producto,
    precio          AS precio_unitario
FROM productos
WHERE activo = TRUE
ORDER BY nombre ASC;

-- ============================================================================
-- CONSULTA 2: Productos con stock bajo (≤ 5 unidades)
-- Descripción: Alerta de productos que requieren reabastecimiento
-- Parámetro: stock_minimo (por defecto 5)
-- Uso: Endpoint GET /api/productos/bajo-stock?stockMinimo=5
-- ============================================================================
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
FROM productos
WHERE stock <= 5  -- Parámetro configurable: @stock_minimo
  AND activo = TRUE
ORDER BY stock ASC, nombre ASC;

-- ============================================================================
-- CONSULTA 3: Clientes jóvenes (≤ 35 años) con compras en rango de fechas
-- Descripción: Clientes menores de 35 años que compraron entre fechas específicas
-- Parámetros: edad_maxima=35, fecha_desde='2000-02-01', fecha_hasta='2000-05-25'
-- Uso: Endpoint GET /api/clientes/por-edad-y-compras
-- ============================================================================
SELECT DISTINCT
    c.id                    AS cliente_id,
    c.nombre                AS nombre,
    c.apellido              AS apellido,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_completo,
    c.correo_electronico    AS correo,
    c.fecha_nacimiento      AS fecha_nacimiento,
    EXTRACT(YEAR FROM AGE(CURRENT_DATE, c.fecha_nacimiento))::INTEGER AS edad,
    COUNT(f.id) OVER (PARTITION BY c.id) AS total_compras_periodo
FROM clientes c
INNER JOIN facturas f ON c.id = f.cliente_id
WHERE EXTRACT(YEAR FROM AGE(CURRENT_DATE, c.fecha_nacimiento)) <= 35  -- Parámetro: @edad_maxima
  AND f.fecha >= '2000-02-01'  -- Parámetro: @fecha_desde
  AND f.fecha <= '2000-05-25'  -- Parámetro: @fecha_hasta
  AND c.activo = TRUE
ORDER BY c.apellido, c.nombre;

-- ============================================================================
-- CONSULTA 4: Total vendido por producto en un año específico
-- Descripción: Reporte de ventas agrupado por producto para análisis
-- Parámetro: anio=2000
-- Uso: Endpoint GET /api/facturas/ventas-por-producto/2000
-- ============================================================================
SELECT 
    p.id                    AS producto_id,
    p.codigo                AS codigo_producto,
    p.nombre                AS nombre_producto,
    SUM(df.cantidad)        AS cantidad_total_vendida,
    SUM(df.subtotal)        AS monto_total_vendido,
    COUNT(DISTINCT f.id)    AS numero_facturas,
    ROUND(AVG(df.precio_unitario), 2) AS precio_promedio_venta
FROM detalles_factura df
INNER JOIN productos p ON df.producto_id = p.id
INNER JOIN facturas f ON df.factura_id = f.id
WHERE EXTRACT(YEAR FROM f.fecha) = 2000  -- Parámetro: @anio
  AND f.estado != 'ANULADA'
GROUP BY p.id, p.codigo, p.nombre
ORDER BY monto_total_vendido DESC;

-- ============================================================================
-- CONSULTA 5: Estimación de próxima compra por cliente
-- Descripción: Calcula el promedio de días entre compras y estima la próxima
-- Requisito: Cliente debe tener al menos 2 compras para calcular promedio
-- Uso: Endpoint GET /api/facturas/proxima-compra/{clienteId}
-- ============================================================================

-- 5.1: Vista general de todos los clientes con predicción
SELECT 
    c.id                    AS cliente_id,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_completo,
    c.correo_electronico    AS correo,
    estadisticas.total_compras,
    estadisticas.primera_compra,
    estadisticas.ultima_compra,
    estadisticas.promedio_dias_entre_compras,
    (estadisticas.ultima_compra + (estadisticas.promedio_dias_entre_compras || ' days')::INTERVAL)::DATE 
        AS proxima_compra_estimada,
    CASE 
        WHEN (estadisticas.ultima_compra + (estadisticas.promedio_dias_entre_compras || ' days')::INTERVAL) < CURRENT_DATE 
        THEN 'VENCIDA'
        WHEN (estadisticas.ultima_compra + (estadisticas.promedio_dias_entre_compras || ' days')::INTERVAL) <= CURRENT_DATE + INTERVAL '7 days'
        THEN 'PRÓXIMA'
        ELSE 'FUTURA'
    END AS estado_prediccion
FROM clientes c
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
    FROM facturas
    WHERE estado != 'ANULADA'
    GROUP BY cliente_id
    HAVING COUNT(*) >= 2  -- Mínimo 2 compras para calcular promedio
) estadisticas ON c.id = estadisticas.cliente_id
WHERE c.activo = TRUE
ORDER BY proxima_compra_estimada ASC;

-- 5.2: Consulta específica para un cliente (paramétrica)
-- Reemplazar $1 con el ID del cliente
SELECT 
    c.id                    AS cliente_id,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_completo,
    f.fecha                 AS fecha_compra,
    f.numero_factura,
    f.total,
    LAG(f.fecha) OVER (ORDER BY f.fecha) AS compra_anterior,
    EXTRACT(DAY FROM (f.fecha - LAG(f.fecha) OVER (ORDER BY f.fecha)))::INTEGER AS dias_desde_anterior
FROM clientes c
INNER JOIN facturas f ON c.id = f.cliente_id
WHERE c.id = 1  -- Parámetro: @cliente_id
  AND f.estado != 'ANULADA'
ORDER BY f.fecha ASC;

-- ============================================================================
-- CONSULTAS AUXILIARES ÚTILES
-- ============================================================================

-- Resumen ejecutivo de ventas por estado
SELECT 
    estado,
    COUNT(*) AS cantidad_facturas,
    SUM(total) AS monto_total
FROM facturas
GROUP BY estado
ORDER BY estado;

-- Top 5 clientes por monto total de compras
SELECT 
    c.id AS cliente_id,
    CONCAT(c.nombre, ' ', c.apellido) AS nombre_completo,
    COUNT(f.id) AS total_facturas,
    SUM(f.total) AS monto_total_compras
FROM clientes c
INNER JOIN facturas f ON c.id = f.cliente_id
WHERE f.estado = 'PAGADA'
GROUP BY c.id, c.nombre, c.apellido
ORDER BY monto_total_compras DESC
LIMIT 5;

-- ============================================================================
-- FIN DEL SCRIPT
-- ============================================================================

