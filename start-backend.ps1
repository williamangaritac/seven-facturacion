# ============================================================================
# Script para levantar el Backend en modo desarrollo - Windows PowerShell
# Sistema de Facturación Seven
# ============================================================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Levantar Backend .NET en Desarrollo" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si .NET está instalado
Write-Host "Verificando .NET SDK..." -ForegroundColor Yellow
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host "❌ .NET SDK no está instalado" -ForegroundColor Red
    Write-Host "Por favor instala .NET 10 desde: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

$dotnetVersion = dotnet --version
Write-Host "✅ .NET SDK instalado: $dotnetVersion" -ForegroundColor Green
Write-Host ""

# Verificar que PostgreSQL esté corriendo
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Verificación de PostgreSQL" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  IMPORTANTE: Asegúrate de que PostgreSQL esté corriendo" -ForegroundColor Yellow
Write-Host ""
Write-Host "Opciones:" -ForegroundColor Cyan
Write-Host "  1. PostgreSQL local en puerto 5432" -ForegroundColor White
Write-Host "  2. PostgreSQL en Docker: docker-compose up -d postgres" -ForegroundColor White
Write-Host ""

$continue = Read-Host "¿Continuar levantando el backend? (s/n)"

if ($continue -ne "s" -and $continue -ne "S") {
    Write-Host "❌ Operación cancelada" -ForegroundColor Red
    exit 0
}

# Navegar a la carpeta del backend
$backendPath = "app_backend\src\Seven.Facturacion.Api"

if (-not (Test-Path $backendPath)) {
    Write-Host "❌ No se encuentra la carpeta $backendPath" -ForegroundColor Red
    exit 1
}

Set-Location $backendPath

# Restaurar dependencias
Write-Host ""
Write-Host "Restaurando dependencias..." -ForegroundColor Yellow
dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al restaurar dependencias" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Dependencias restauradas" -ForegroundColor Green
Write-Host ""

# Levantar el backend
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Levantando Backend .NET" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Ejecutando: dotnet run" -ForegroundColor Yellow
Write-Host ""
Write-Host "El backend estará disponible en:" -ForegroundColor Cyan
Write-Host "  https://localhost:49497/api" -ForegroundColor White
Write-Host "  Swagger: https://localhost:49497/swagger" -ForegroundColor White
Write-Host ""
Write-Host "Base de datos:" -ForegroundColor Cyan
Write-Host "  Host: localhost" -ForegroundColor White
Write-Host "  Puerto: 5432" -ForegroundColor White
Write-Host "  Database: seven_facturacion_dev" -ForegroundColor White
Write-Host "  Usuario: postgres" -ForegroundColor White
Write-Host "  Contraseña: postgres" -ForegroundColor White
Write-Host ""
Write-Host "Presiona Ctrl+C para detener el servidor" -ForegroundColor Yellow
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Ejecutar dotnet run
dotnet run

