-- =============================================
-- Script: Actualizar password del usuario admin
-- Descripción: Actualiza el hash de la contraseña admin123
-- Autor: Seven Facturación Team
-- Fecha: 2025-12-31
-- =============================================

-- Actualizar password del usuario admin
-- Password: admin123
-- Hash BCrypt generado correctamente
UPDATE facturacion.usuarios 
SET password_hash = '$2a$11$alcPRbYR8aC4HLV5MrGJZuGFgU50yqPYgD/DuC0itP3SSFkRLp02.'
WHERE username = 'admin';

-- Verificar actualización
SELECT id, username, password_hash, activo 
FROM facturacion.usuarios 
WHERE username = 'admin';

