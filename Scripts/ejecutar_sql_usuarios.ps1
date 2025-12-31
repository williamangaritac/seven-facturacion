# Script para ejecutar la creación de tabla usuarios
# Asegúrate de tener PostgreSQL corriendo

$env:PGPASSWORD = "postgres"
$scriptPath = Join-Path $PSScriptRoot "04_crear_tabla_usuarios.sql"

Write-Host "Ejecutando script de creación de tabla usuarios..." -ForegroundColor Cyan

# Intentar con psql local
try {
    psql -h localhost -U postgres -d facturacion_db -f $scriptPath
    Write-Host "✓ Tabla usuarios creada exitosamente" -ForegroundColor Green
}
catch {
    Write-Host "✗ Error: No se pudo ejecutar el script" -ForegroundColor Red
    Write-Host "Asegúrate de que PostgreSQL esté corriendo y psql esté en el PATH" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Alternativa: Ejecuta manualmente el contenido de:" -ForegroundColor Yellow
    Write-Host $scriptPath -ForegroundColor White
}

