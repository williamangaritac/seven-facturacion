# ============================================================================
# Script de prueba - Sistema de Facturación Seven
# Verifica que todos los servicios estén funcionando correctamente
# ============================================================================

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Prueba de Servicios Docker" -ForegroundColor Cyan
Write-Host "Sistema de Facturación Seven" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Función para verificar servicio
function Test-Service {
    param(
        [string]$ServiceName,
        [string]$Url,
        [int]$ExpectedStatus
    )
    
    Write-Host "Verificando $ServiceName... " -NoNewline
    
    try {
        $response = Invoke-WebRequest -Uri $Url -Method Get -UseBasicParsing -ErrorAction Stop
        $statusCode = $response.StatusCode
        
        if ($statusCode -eq $ExpectedStatus) {
            Write-Host "✅ OK (HTTP $statusCode)" -ForegroundColor Green
            return $true
        } else {
            Write-Host "❌ FAIL (HTTP $statusCode, esperado $ExpectedStatus)" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Host "❌ FAIL (Error: $($_.Exception.Message))" -ForegroundColor Red
        return $false
    }
}

# 1. Verificar que Docker esté corriendo
Write-Host "1. Verificando Docker..." -ForegroundColor Yellow
try {
    docker info | Out-Null
    Write-Host "✅ Docker está corriendo" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker no está corriendo" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 2. Verificar que los contenedores estén corriendo
Write-Host "2. Verificando contenedores..." -ForegroundColor Yellow
$runningContainers = docker-compose ps --services --filter "status=running"
$containerCount = ($runningContainers | Measure-Object).Count

if ($containerCount -lt 3) {
    Write-Host "❌ No todos los contenedores están corriendo" -ForegroundColor Red
    docker-compose ps
    exit 1
}
Write-Host "✅ Todos los contenedores están corriendo" -ForegroundColor Green
Write-Host ""

# 3. Esperar a que los servicios estén listos
Write-Host "3. Esperando a que los servicios estén listos..." -ForegroundColor Yellow
Start-Sleep -Seconds 5
Write-Host ""

# 4. Verificar servicios HTTP
Write-Host "4. Verificando servicios HTTP..." -ForegroundColor Yellow
Write-Host ""

$allPassed = $true

# Frontend
$allPassed = $allPassed -and (Test-Service "Frontend (Angular)" "http://localhost:4200" 200)

# Backend - Clientes
$allPassed = $allPassed -and (Test-Service "Backend - Clientes" "http://localhost:5000/api/clientes" 200)

# Backend - Productos
$allPassed = $allPassed -and (Test-Service "Backend - Productos" "http://localhost:5000/api/productos" 200)

# Backend - Facturas
$allPassed = $allPassed -and (Test-Service "Backend - Facturas" "http://localhost:5000/api/facturas" 200)

Write-Host ""

# 5. Verificar base de datos
Write-Host "5. Verificando base de datos..." -ForegroundColor Yellow
try {
    $dbCheck = docker-compose exec -T postgres psql -U postgres -d seven_facturacion_dev -c "SELECT COUNT(*) FROM facturacion.usuarios;" 2>$null
    
    if ($dbCheck -match '\d+' -and [int]$Matches[0] -ge 1) {
        Write-Host "✅ Base de datos tiene datos" -ForegroundColor Green
    } else {
        Write-Host "❌ Base de datos no tiene datos" -ForegroundColor Red
        $allPassed = $false
    }
} catch {
    Write-Host "❌ Error al verificar base de datos" -ForegroundColor Red
    $allPassed = $false
}

Write-Host ""

# 6. Verificar login
Write-Host "6. Verificando autenticación..." -ForegroundColor Yellow
try {
    $body = @{
        username = "admin"
        password = "admin123"
    } | ConvertTo-Json

    $loginResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" `
        -Method Post `
        -Body $body `
        -ContentType "application/json" `
        -ErrorAction Stop

    if ($loginResponse.token) {
        Write-Host "✅ Login funciona correctamente" -ForegroundColor Green
        Write-Host "   Token: $($loginResponse.token)" -ForegroundColor Gray
    } else {
        Write-Host "❌ Login falló - No se recibió token" -ForegroundColor Red
        $allPassed = $false
    }
} catch {
    Write-Host "❌ Login falló - $($_.Exception.Message)" -ForegroundColor Red
    $allPassed = $false
}

Write-Host ""

# Resultado final
if ($allPassed) {
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host "✅ Todas las pruebas pasaron exitosamente" -ForegroundColor Green
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "El sistema está funcionando correctamente." -ForegroundColor White
    Write-Host "Puedes acceder a:" -ForegroundColor Cyan
    Write-Host "  - Frontend: http://localhost:4200" -ForegroundColor White
    Write-Host "  - Backend:  http://localhost:5000" -ForegroundColor White
    Write-Host ""
    Write-Host "Credenciales:" -ForegroundColor Cyan
    Write-Host "  - Usuario: admin" -ForegroundColor White
    Write-Host "  - Contraseña: admin123" -ForegroundColor White
    Write-Host ""
    exit 0
} else {
    Write-Host "==========================================" -ForegroundColor Red
    Write-Host "❌ Algunas pruebas fallaron" -ForegroundColor Red
    Write-Host "==========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Revisa los logs con: docker-compose logs -f" -ForegroundColor Yellow
    Write-Host ""
    exit 1
}

