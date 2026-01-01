-- =============================================
-- Script: Crear tabla de usuarios
-- Descripción: Tabla para autenticación del sistema
-- Autor: Seven Facturación Team
-- Fecha: 2025-12-31
-- =============================================

-- Crear tabla usuarios
CREATE TABLE IF NOT EXISTS facturacion.usuarios (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

-- Insertar usuario admin
-- Password: admin123 (hash BCrypt)
INSERT INTO facturacion.usuarios (username, password_hash, activo)
VALUES ('admin', '$2a$11$8K1p/a0dL3LHR/nHkfuBiOCEZZ8QeKhQkrXfzIU4OqgnE0.jjKZ6e', TRUE)
ON CONFLICT (username) DO NOTHING;

-- Índice para búsqueda rápida por username
CREATE INDEX IF NOT EXISTS idx_usuarios_username ON facturacion.usuarios(username);

COMMENT ON TABLE facturacion.usuarios IS 'Tabla de usuarios del sistema';
COMMENT ON COLUMN facturacion.usuarios.username IS 'Nombre de usuario único';
COMMENT ON COLUMN facturacion.usuarios.password_hash IS 'Contraseña hasheada con BCrypt';

