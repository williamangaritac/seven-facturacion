# ============================================================================
# Script para subir el proyecto a GitHub - Windows PowerShell
# Sistema de Facturación Seven
# ============================================================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Subir Proyecto a GitHub" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si Git está instalado
Write-Host "Verificando Git..." -ForegroundColor Yellow
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Git no está instalado" -ForegroundColor Red
    Write-Host "Por favor instala Git desde: https://git-scm.com/download/win" -ForegroundColor Yellow
    exit 1
}

$gitVersion = git --version
Write-Host "✅ Git instalado: $gitVersion" -ForegroundColor Green
Write-Host ""

# Verificar si ya está inicializado
Write-Host "Verificando repositorio Git..." -ForegroundColor Yellow
if (-not (Test-Path ".git")) {
    Write-Host "⚠️  Repositorio no inicializado" -ForegroundColor Yellow
    Write-Host "Inicializando repositorio..." -ForegroundColor Yellow
    git init
    Write-Host "✅ Repositorio inicializado" -ForegroundColor Green
} else {
    Write-Host "✅ Repositorio ya inicializado" -ForegroundColor Green
}
Write-Host ""

# Verificar configuración de Git
Write-Host "Verificando configuración de Git..." -ForegroundColor Yellow
$userName = git config user.name
$userEmail = git config user.email

if (-not $userName -or -not $userEmail) {
    Write-Host "⚠️  Git no está configurado" -ForegroundColor Yellow
    Write-Host ""
    
    $name = Read-Host "Ingresa tu nombre"
    $email = Read-Host "Ingresa tu email (el mismo de GitHub)"
    
    git config --global user.name "$name"
    git config --global user.email "$email"
    
    Write-Host "✅ Git configurado correctamente" -ForegroundColor Green
} else {
    Write-Host "✅ Usuario: $userName" -ForegroundColor Green
    Write-Host "✅ Email: $userEmail" -ForegroundColor Green
}
Write-Host ""

# Preguntar por la URL del repositorio
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Configuración del Repositorio Remoto" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Primero, crea un repositorio en GitHub:" -ForegroundColor Yellow
Write-Host "1. Ve a https://github.com/new" -ForegroundColor White
Write-Host "2. Nombre: seven-facturacion-system (o el que prefieras)" -ForegroundColor White
Write-Host "3. NO marques 'Initialize with README'" -ForegroundColor White
Write-Host "4. Crea el repositorio" -ForegroundColor White
Write-Host ""

$repoUrl = Read-Host "Ingresa la URL del repositorio (ej: https://github.com/usuario/repo.git)"

if (-not $repoUrl) {
    Write-Host "❌ URL no proporcionada" -ForegroundColor Red
    exit 1
}

# Verificar si ya existe el remote origin
$existingRemote = git remote get-url origin 2>$null

if ($existingRemote) {
    Write-Host "⚠️  Ya existe un remote 'origin': $existingRemote" -ForegroundColor Yellow
    $replace = Read-Host "¿Deseas reemplazarlo? (s/n)"
    
    if ($replace -eq "s" -or $replace -eq "S") {
        git remote remove origin
        git remote add origin $repoUrl
        Write-Host "✅ Remote 'origin' actualizado" -ForegroundColor Green
    }
} else {
    git remote add origin $repoUrl
    Write-Host "✅ Remote 'origin' agregado" -ForegroundColor Green
}
Write-Host ""

# Verificar archivos a commitear
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Preparando Archivos" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Archivos a subir:" -ForegroundColor Yellow
git status --short
Write-Host ""

$continue = Read-Host "¿Continuar con el commit? (s/n)"

if ($continue -ne "s" -and $continue -ne "S") {
    Write-Host "❌ Operación cancelada" -ForegroundColor Red
    exit 0
}

# Agregar archivos
Write-Host "Agregando archivos..." -ForegroundColor Yellow
git add .
Write-Host "✅ Archivos agregados" -ForegroundColor Green
Write-Host ""

# Hacer commit
Write-Host "Mensaje del commit:" -ForegroundColor Yellow
$commitMessage = Read-Host "Ingresa el mensaje (Enter para usar el predeterminado)"

if (-not $commitMessage) {
    $commitMessage = "Initial commit: Sistema de Facturación Seven con Docker"
}

git commit -m "$commitMessage"
Write-Host "✅ Commit realizado" -ForegroundColor Green
Write-Host ""

# Verificar rama
$currentBranch = git branch --show-current

if (-not $currentBranch) {
    Write-Host "Creando rama 'main'..." -ForegroundColor Yellow
    git branch -M main
    $currentBranch = "main"
}

Write-Host "Rama actual: $currentBranch" -ForegroundColor Cyan
Write-Host ""

# Subir a GitHub
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Subiendo a GitHub" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Subiendo a GitHub..." -ForegroundColor Yellow
Write-Host "Si te pide contraseña, usa un Personal Access Token" -ForegroundColor Yellow
Write-Host "Más info: https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token" -ForegroundColor Yellow
Write-Host ""

git push -u origin $currentBranch

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Green
    Write-Host "✅ ¡Proyecto subido exitosamente a GitHub!" -ForegroundColor Green
    Write-Host "============================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Tu repositorio está en:" -ForegroundColor Cyan
    Write-Host $repoUrl -ForegroundColor White
    Write-Host ""
    Write-Host "Ahora puedes clonarlo con:" -ForegroundColor Cyan
    Write-Host "git clone $repoUrl" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Red
    Write-Host "❌ Error al subir a GitHub" -ForegroundColor Red
    Write-Host "============================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Posibles soluciones:" -ForegroundColor Yellow
    Write-Host "1. Verifica que la URL del repositorio sea correcta" -ForegroundColor White
    Write-Host "2. Asegúrate de tener permisos en el repositorio" -ForegroundColor White
    Write-Host "3. Usa un Personal Access Token en lugar de contraseña" -ForegroundColor White
    Write-Host "4. Consulta GITHUB_SETUP.md para más ayuda" -ForegroundColor White
    Write-Host ""
}

