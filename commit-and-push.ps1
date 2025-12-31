# ============================================================================
# Script rápido para hacer commit y push - Windows PowerShell
# Sistema de Facturación Seven
# ============================================================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Commit y Push a GitHub" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar estado
Write-Host "Estado actual del repositorio:" -ForegroundColor Yellow
Write-Host ""
git status --short
Write-Host ""

# Preguntar si continuar
$continue = Read-Host "¿Deseas hacer commit de estos cambios? (s/n)"

if ($continue -ne "s" -and $continue -ne "S") {
    Write-Host "❌ Operación cancelada" -ForegroundColor Red
    exit 0
}

# Pedir mensaje de commit
Write-Host ""
Write-Host "Mensaje del commit:" -ForegroundColor Yellow
$commitMessage = Read-Host "Ingresa el mensaje"

if (-not $commitMessage) {
    $commitMessage = "Update: cambios en el proyecto"
}

# Agregar todos los archivos
Write-Host ""
Write-Host "Agregando archivos..." -ForegroundColor Yellow
git add .

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al agregar archivos" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Archivos agregados" -ForegroundColor Green

# Hacer commit
Write-Host ""
Write-Host "Haciendo commit..." -ForegroundColor Yellow
git commit -m "$commitMessage"

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al hacer commit" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Commit realizado" -ForegroundColor Green

# Obtener rama actual
$currentBranch = git branch --show-current

# Hacer push
Write-Host ""
Write-Host "Subiendo a GitHub (rama: $currentBranch)..." -ForegroundColor Yellow
git push origin $currentBranch

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Green
    Write-Host "✅ ¡Cambios subidos exitosamente!" -ForegroundColor Green
    Write-Host "============================================" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Red
    Write-Host "❌ Error al subir a GitHub" -ForegroundColor Red
    Write-Host "============================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Intenta:" -ForegroundColor Yellow
    Write-Host "  git push origin $currentBranch" -ForegroundColor White
    Write-Host ""
}

