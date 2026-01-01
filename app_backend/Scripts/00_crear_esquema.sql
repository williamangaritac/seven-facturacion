-- ============================================================================
-- SISTEMA DE FACTURACIÓN - CREACIÓN DEL ESQUEMA
-- Base de datos: PostgreSQL 16
-- Autor: Sistema de Facturación Seven
-- ============================================================================

-- Crear el esquema facturacion si no existe
CREATE SCHEMA IF NOT EXISTS facturacion;

-- Establecer el esquema por defecto para las siguientes operaciones
SET search_path TO facturacion, public;

-- Mensaje de confirmación
DO $$
BEGIN
    RAISE NOTICE 'Esquema facturacion creado correctamente';
END $$;

