-- ============================================================================
-- SISTEMA DE FACTURACIÓN - SCRIPT DE INSERCIÓN DE DATOS DE PRUEBA
-- Base de datos: PostgreSQL 16
-- Autor: Sistema de Facturación Seven
-- Nota: Datos diseñados para validar las 5 consultas SQL requeridas
-- ============================================================================

-- Usar el esquema facturacion
SET search_path TO facturacion, public;

-- ============================================================================
-- DATOS DE CLIENTES
-- Incluye clientes con edad <= 35 años para consulta específica
-- ============================================================================
INSERT INTO clientes (nombre, apellido, correo_electronico, telefono, fecha_nacimiento, direccion) VALUES
-- Clientes jóvenes (<=35 años) - nacidos después de 1989
('Carlos', 'Martínez', 'carlos.martinez@email.com', '3001234567', '1992-05-15', 'Calle 123 #45-67, Bogotá'),
('María', 'García', 'maria.garcia@email.com', '3009876543', '1995-08-22', 'Carrera 45 #12-34, Medellín'),
('Andrés', 'López', 'andres.lopez@email.com', '3005551234', '1990-03-10', 'Avenida 68 #23-45, Cali'),
('Laura', 'Rodríguez', 'laura.rodriguez@email.com', '3007778899', '1993-11-28', 'Calle 80 #15-20, Barranquilla'),
('Diego', 'Hernández', 'diego.hernandez@email.com', '3004443322', '1991-07-04', 'Carrera 15 #88-12, Bogotá'),
-- Clientes mayores (>35 años) - nacidos antes de 1989
('Patricia', 'Sánchez', 'patricia.sanchez@email.com', '3002223344', '1975-02-14', 'Calle 50 #30-40, Medellín'),
('Roberto', 'Ramírez', 'roberto.ramirez@email.com', '3006667788', '1968-09-30', 'Avenida Caracas #45-67, Bogotá'),
('Carmen', 'Torres', 'carmen.torres@email.com', '3008889900', '1980-12-05', 'Carrera 7 #120-30, Bogotá'),
('Fernando', 'Vargas', 'fernando.vargas@email.com', '3001112233', '1972-06-18', 'Calle 100 #19-45, Cali'),
('Lucía', 'Morales', 'lucia.morales@email.com', '3003334455', '1985-04-25', 'Carrera 50 #80-15, Medellín');

-- ============================================================================
-- DATOS DE PRODUCTOS
-- Incluye productos con stock <= 5 para consulta de bajo stock
-- ============================================================================
INSERT INTO productos (codigo, nombre, descripcion, precio, stock) VALUES
('PROD-001', 'Laptop HP ProBook 450', 'Laptop empresarial 15.6" Intel i7 16GB RAM', 2500000.00, 15),
('PROD-002', 'Monitor Dell 27"', 'Monitor LED Full HD 27 pulgadas', 850000.00, 8),
('PROD-003', 'Teclado Mecánico Logitech', 'Teclado mecánico RGB switches blue', 350000.00, 3),  -- Stock bajo
('PROD-004', 'Mouse Inalámbrico', 'Mouse ergonómico inalámbrico 2.4GHz', 85000.00, 25),
('PROD-005', 'Disco SSD 1TB', 'Disco sólido NVMe PCIe 4.0', 420000.00, 5),  -- Stock bajo
('PROD-006', 'Memoria RAM 16GB', 'Módulo DDR4 3200MHz', 280000.00, 2),  -- Stock bajo
('PROD-007', 'Webcam HD 1080p', 'Cámara web con micrófono integrado', 195000.00, 12),
('PROD-008', 'Audífonos Bluetooth', 'Audífonos over-ear con cancelación de ruido', 450000.00, 4),  -- Stock bajo
('PROD-009', 'Cable HDMI 2m', 'Cable HDMI 2.1 alta velocidad', 45000.00, 50),
('PROD-010', 'Hub USB-C', 'Hub 7 puertos USB-C con PD', 180000.00, 1);  -- Stock bajo

-- ============================================================================
-- DATOS DE FACTURAS - AÑO 2000 (Para consultas específicas)
-- Facturas entre 2000-02-01 y 2000-05-25 para clientes jóvenes
-- ============================================================================
INSERT INTO facturas (numero_factura, cliente_id, fecha, subtotal, impuesto, total, estado) VALUES
-- Facturas de clientes jóvenes en el rango de fechas requerido (2000-02-01 a 2000-05-25)
('FAC-20000215001', 1, '2000-02-15 10:30:00', 2850000.00, 541500.00, 3391500.00, 'PAGADA'),
('FAC-20000301002', 2, '2000-03-01 14:45:00', 935000.00, 177650.00, 1112650.00, 'PAGADA'),
('FAC-20000320003', 3, '2000-03-20 09:15:00', 505000.00, 95950.00, 600950.00, 'PAGADA'),
('FAC-20000410004', 1, '2000-04-10 16:00:00', 630000.00, 119700.00, 749700.00, 'PAGADA'),
('FAC-20000425005', 4, '2000-04-25 11:20:00', 2500000.00, 475000.00, 2975000.00, 'PAGADA'),
('FAC-20000510006', 5, '2000-05-10 08:45:00', 1130000.00, 214700.00, 1344700.00, 'PAGADA'),
('FAC-20000520007', 2, '2000-05-20 15:30:00', 365000.00, 69350.00, 434350.00, 'PAGADA'),
-- Facturas de clientes mayores en el mismo período
('FAC-20000225008', 6, '2000-02-25 10:00:00', 850000.00, 161500.00, 1011500.00, 'PAGADA'),
('FAC-20000315009', 7, '2000-03-15 13:30:00', 420000.00, 79800.00, 499800.00, 'PAGADA'),
-- Facturas fuera del rango (para verificar filtro)
('FAC-20000601010', 1, '2000-06-01 09:00:00', 195000.00, 37050.00, 232050.00, 'PAGADA'),
('FAC-20000715011', 3, '2000-07-15 14:00:00', 450000.00, 85500.00, 535500.00, 'PAGADA');

-- ============================================================================
-- DATOS DE FACTURAS - AÑOS RECIENTES (Para pruebas de próxima compra)
-- Múltiples facturas por cliente para calcular promedio de días
-- ============================================================================
INSERT INTO facturas (numero_factura, cliente_id, fecha, subtotal, impuesto, total, estado) VALUES
-- Cliente 1: Compras cada ~30 días aproximadamente
('FAC-20241001012', 1, '2024-10-01 10:00:00', 350000.00, 66500.00, 416500.00, 'PAGADA'),
('FAC-20241102013', 1, '2024-11-02 11:00:00', 280000.00, 53200.00, 333200.00, 'PAGADA'),
('FAC-20241205014', 1, '2024-12-05 09:30:00', 420000.00, 79800.00, 499800.00, 'PENDIENTE'),
-- Cliente 2: Compras cada ~45 días
('FAC-20240901015', 2, '2024-09-01 14:00:00', 850000.00, 161500.00, 1011500.00, 'PAGADA'),
('FAC-20241015016', 2, '2024-10-15 16:00:00', 195000.00, 37050.00, 232050.00, 'PAGADA'),
('FAC-20241201017', 2, '2024-12-01 10:30:00', 530000.00, 100700.00, 630700.00, 'PAGADA');

-- ============================================================================
-- DATOS DE DETALLES DE FACTURA
-- ============================================================================
INSERT INTO detalles_factura (factura_id, producto_id, cantidad, precio_unitario) VALUES
-- Factura 1 (FAC-20000215001)
(1, 1, 1, 2500000.00),  -- Laptop
(1, 3, 1, 350000.00),   -- Teclado
-- Factura 2 (FAC-20000301002)
(2, 2, 1, 850000.00),   -- Monitor
(2, 4, 1, 85000.00),    -- Mouse
-- Factura 3 (FAC-20000320003)
(3, 5, 1, 420000.00),   -- SSD
(3, 4, 1, 85000.00),    -- Mouse
-- Factura 4 (FAC-20000410004)
(4, 6, 2, 280000.00),   -- 2x RAM
(4, 9, 2, 45000.00),    -- 2x Cable HDMI
-- Factura 5 (FAC-20000425005)
(5, 1, 1, 2500000.00),  -- Laptop
-- Factura 6 (FAC-20000510006)
(6, 2, 1, 850000.00),   -- Monitor
(6, 6, 1, 280000.00),   -- RAM
-- Factura 7 (FAC-20000520007)
(7, 3, 1, 350000.00),   -- Teclado
(7, 9, 1, 45000.00),    -- Cable HDMI
-- Factura 8 (FAC-20000225008)
(8, 2, 1, 850000.00),   -- Monitor
-- Factura 9 (FAC-20000315009)
(9, 5, 1, 420000.00),   -- SSD
-- Factura 10 (FAC-20000601010)
(10, 7, 1, 195000.00),  -- Webcam
-- Factura 11 (FAC-20000715011)
(11, 8, 1, 450000.00),  -- Audífonos
-- Facturas recientes
(12, 3, 1, 350000.00),  -- Teclado
(13, 6, 1, 280000.00),  -- RAM
(14, 5, 1, 420000.00),  -- SSD
(15, 2, 1, 850000.00),  -- Monitor
(16, 7, 1, 195000.00),  -- Webcam
(17, 8, 1, 450000.00),  -- Audífonos
(17, 4, 1, 85000.00);   -- Mouse

-- ============================================================================
-- FIN DEL SCRIPT
-- ============================================================================

