# ============================================================================
# Script de inicio rápido - Sistema de Facturación Seven
# Para Windows PowerShell
# ============================================================================

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "Inicio Rápido con Docker" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si Docker está instalado
try {
    $dockerVersion = docker --version
    Write-Host "✅ Docker está instalado: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: Docker no está instalado" -ForegroundColor Red
    Write-Host "Por favor instala Docker Desktop desde: https://www.docker.com/get-started" -ForegroundColor Yellow
    exit 1
}

# Verificar si Docker Compose está disponible
try {
    $composeVersion = docker-compose --version
    Write-Host "✅ Docker Compose está instalado: $composeVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: Docker Compose no está instalado" -ForegroundColor Red
    Write-Host "Por favor instala Docker Compose desde: https://docs.docker.com/compose/install/" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# Verificar si Docker está corriendo
try {
    docker info | Out-Null
    Write-Host "✅ Docker está corriendo" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: Docker no está corriendo" -ForegroundColor Red
    Write-Host "Por favor inicia Docker Desktop" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# Detener contenedores existentes si los hay
Write-Host "🔄 Deteniendo contenedores existentes..." -ForegroundColor Yellow
docker-compose down 2>$null
Write-Host ""

# Construir y levantar servicios
Write-Host "🏗️  Construyendo imágenes Docker..." -ForegroundColor Yellow
docker-compose build
Write-Host ""

Write-Host "🚀 Levantando servicios..." -ForegroundColor Yellow
docker-compose up -d
Write-Host ""

# Esperar a que los servicios estén listos
Write-Host "⏳ Esperando a que los servicios estén listos..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Verificar estado de los servicios
Write-Host ""
Write-Host "📊 Estado de los servicios:" -ForegroundColor Cyan
docker-compose ps
Write-Host ""

# Mostrar información de acceso
Write-Host "==========================================" -ForegroundColor Green
Write-Host "✅ ¡Sistema levantado exitosamente!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 Accede a la aplicación en:" -ForegroundColor Cyan
Write-Host "   Frontend: http://localhost:4200" -ForegroundColor White
Write-Host "   Backend:  http://localhost:5000" -ForegroundColor White
Write-Host ""
Write-Host "🔐 Credenciales de acceso:" -ForegroundColor Cyan
Write-Host "   Usuario:    admin" -ForegroundColor White
Write-Host "   Contraseña: admin123" -ForegroundColor White
Write-Host ""
Write-Host "📝 Comandos útiles:" -ForegroundColor Cyan
Write-Host "   Ver logs:           docker-compose logs -f" -ForegroundColor White
Write-Host "   Detener servicios:  docker-compose down" -ForegroundColor White
Write-Host "   Reiniciar:          docker-compose restart" -ForegroundColor White
Write-Host ""
Write-Host "==========================================" -ForegroundColor Green

