# ============================================================================
# Script para levantar el Frontend en modo desarrollo - Windows PowerShell
# Sistema de Facturación Seven
# ============================================================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Levantar Frontend Angular en Desarrollo" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si Node.js está instalado
Write-Host "Verificando Node.js..." -ForegroundColor Yellow
if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Node.js no está instalado" -ForegroundColor Red
    Write-Host "Por favor instala Node.js desde: https://nodejs.org/" -ForegroundColor Yellow
    exit 1
}

$nodeVersion = node --version
Write-Host "✅ Node.js instalado: $nodeVersion" -ForegroundColor Green
Write-Host ""

# Verificar si npm está instalado
Write-Host "Verificando npm..." -ForegroundColor Yellow
if (-not (Get-Command npm -ErrorAction SilentlyContinue)) {
    Write-Host "❌ npm no está instalado" -ForegroundColor Red
    exit 1
}

$npmVersion = npm --version
Write-Host "✅ npm instalado: $npmVersion" -ForegroundColor Green
Write-Host ""

# Navegar a la carpeta del frontend
$frontendPath = "frontend_angular"

if (-not (Test-Path $frontendPath)) {
    Write-Host "❌ No se encuentra la carpeta $frontendPath" -ForegroundColor Red
    exit 1
}

Set-Location $frontendPath

# Verificar si node_modules existe
if (-not (Test-Path "node_modules")) {
    Write-Host "⚠️  node_modules no encontrado" -ForegroundColor Yellow
    Write-Host "Instalando dependencias..." -ForegroundColor Yellow
    npm install
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Error al instalar dependencias" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "✅ Dependencias instaladas" -ForegroundColor Green
    Write-Host ""
}

# Verificar que el backend esté corriendo
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Verificación del Backend" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  IMPORTANTE: Asegúrate de que el backend esté corriendo" -ForegroundColor Yellow
Write-Host ""
Write-Host "El frontend se conectará a:" -ForegroundColor Cyan
Write-Host "  https://localhost:49497/api" -ForegroundColor White
Write-Host ""
Write-Host "Si el backend NO está corriendo, levántalo con:" -ForegroundColor Yellow
Write-Host "  cd .." -ForegroundColor White
Write-Host "  cd src/Seven.Facturacion.Api" -ForegroundColor White
Write-Host "  dotnet run" -ForegroundColor White
Write-Host ""

$continue = Read-Host "¿Continuar levantando el frontend? (s/n)"

if ($continue -ne "s" -and $continue -ne "S") {
    Write-Host "❌ Operación cancelada" -ForegroundColor Red
    exit 0
}

# Levantar el frontend
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Levantando Frontend Angular" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Ejecutando: ng serve" -ForegroundColor Yellow
Write-Host ""
Write-Host "El frontend estará disponible en:" -ForegroundColor Cyan
Write-Host "  http://localhost:4200" -ForegroundColor White
Write-Host ""
Write-Host "Credenciales:" -ForegroundColor Cyan
Write-Host "  Usuario: admin" -ForegroundColor White
Write-Host "  Contraseña: admin123" -ForegroundColor White
Write-Host ""
Write-Host "Presiona Ctrl+C para detener el servidor" -ForegroundColor Yellow
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Ejecutar ng serve
npm start

