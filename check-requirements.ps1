# ============================================================================
# Script de verificación de requisitos - Sistema de Facturación Seven
# Verifica que todas las herramientas necesarias estén instaladas
# ============================================================================

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Verificación de Requisitos" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$allOk = $true

# Función para verificar comando
function Test-Command {
    param(
        [string]$CommandName,
        [string]$DisplayName,
        [string]$MinVersion = $null,
        [bool]$Required = $true
    )
    
    Write-Host "Verificando $DisplayName... " -NoNewline
    
    try {
        $command = Get-Command $CommandName -ErrorAction Stop
        
        if ($MinVersion) {
            $version = & $CommandName --version 2>&1 | Select-Object -First 1
            Write-Host "✅ Instalado ($version)" -ForegroundColor Green
        } else {
            Write-Host "✅ Instalado" -ForegroundColor Green
        }
        
        return $true
    } catch {
        if ($Required) {
            Write-Host "❌ No instalado (REQUERIDO)" -ForegroundColor Red
            return $false
        } else {
            Write-Host "⚠️  No instalado (Opcional)" -ForegroundColor Yellow
            return $true
        }
    }
}

Write-Host "=== Requisitos para Docker ===" -ForegroundColor Cyan
Write-Host ""

# Docker
$dockerOk = Test-Command "docker" "Docker" -Required $true
if ($dockerOk) {
    # Verificar que Docker esté corriendo
    try {
        docker info | Out-Null 2>&1
        Write-Host "   Docker está corriendo ✅" -ForegroundColor Green
    } catch {
        Write-Host "   Docker está instalado pero NO está corriendo ⚠️" -ForegroundColor Yellow
        Write-Host "   Por favor inicia Docker Desktop" -ForegroundColor Yellow
        $allOk = $false
    }
}
$allOk = $allOk -and $dockerOk

# Docker Compose
$composeOk = Test-Command "docker-compose" "Docker Compose" -Required $true
$allOk = $allOk -and $composeOk

Write-Host ""
Write-Host "=== Requisitos para Desarrollo Local (Opcional) ===" -ForegroundColor Cyan
Write-Host ""

# .NET SDK
$dotnetOk = Test-Command "dotnet" ".NET SDK" -Required $false
if ($dotnetOk) {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -match "^10\.") {
        Write-Host "   .NET 10 detectado ✅" -ForegroundColor Green
    } else {
        Write-Host "   Versión: $dotnetVersion (Se requiere .NET 10) ⚠️" -ForegroundColor Yellow
    }
}

# Node.js
$nodeOk = Test-Command "node" "Node.js" -Required $false
if ($nodeOk) {
    $nodeVersion = node --version
    $versionNumber = [int]($nodeVersion -replace 'v(\d+)\..*', '$1')
    if ($versionNumber -ge 20) {
        Write-Host "   Node.js >= 20 detectado ✅" -ForegroundColor Green
    } else {
        Write-Host "   Versión: $nodeVersion (Se requiere >= 20) ⚠️" -ForegroundColor Yellow
    }
}

# npm
Test-Command "npm" "npm" -Required $false | Out-Null

# PostgreSQL
$psqlOk = Test-Command "psql" "PostgreSQL Client" -Required $false

# Git
Test-Command "git" "Git" -Required $false | Out-Null

Write-Host ""
Write-Host "=== Herramientas Adicionales (Opcional) ===" -ForegroundColor Cyan
Write-Host ""

# Angular CLI
$ngOk = Test-Command "ng" "Angular CLI" -Required $false

# Make (para usar Makefile)
Test-Command "make" "Make" -Required $false | Out-Null

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan

if ($allOk) {
    Write-Host "✅ Todos los requisitos necesarios están instalados" -ForegroundColor Green
    Write-Host ""
    Write-Host "Puedes ejecutar el proyecto con:" -ForegroundColor Cyan
    Write-Host "  .\start.ps1" -ForegroundColor White
    Write-Host ""
    Write-Host "O manualmente con:" -ForegroundColor Cyan
    Write-Host "  docker-compose up -d" -ForegroundColor White
} else {
    Write-Host "❌ Faltan algunos requisitos necesarios" -ForegroundColor Red
    Write-Host ""
    Write-Host "Por favor instala:" -ForegroundColor Yellow
    
    if (-not $dockerOk) {
        Write-Host "  - Docker Desktop: https://www.docker.com/products/docker-desktop/" -ForegroundColor White
    }
    
    if (-not $composeOk) {
        Write-Host "  - Docker Compose (incluido en Docker Desktop)" -ForegroundColor White
    }
    
    Write-Host ""
}

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Información adicional
Write-Host "📚 Documentación:" -ForegroundColor Cyan
Write-Host "  - README.md - Guía general del proyecto" -ForegroundColor White
Write-Host "  - DOCKER_SETUP.md - Guía detallada de Docker" -ForegroundColor White
Write-Host "  - CONTRIBUTING.md - Guía para desarrolladores" -ForegroundColor White
Write-Host ""

if ($allOk) {
    exit 0
} else {
    exit 1
}

