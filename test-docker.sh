#!/bin/bash

# ============================================================================
# Script de prueba - Sistema de Facturación Seven
# Verifica que todos los servicios estén funcionando correctamente
# ============================================================================

set -e

echo "=========================================="
echo "Prueba de Servicios Docker"
echo "Sistema de Facturación Seven"
echo "=========================================="
echo ""

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Función para verificar servicio
check_service() {
    local service_name=$1
    local url=$2
    local expected_status=$3
    
    echo -n "Verificando $service_name... "
    
    status_code=$(curl -s -o /dev/null -w "%{http_code}" $url)
    
    if [ "$status_code" -eq "$expected_status" ]; then
        echo -e "${GREEN}✅ OK (HTTP $status_code)${NC}"
        return 0
    else
        echo -e "${RED}❌ FAIL (HTTP $status_code, esperado $expected_status)${NC}"
        return 1
    fi
}

# Verificar que Docker esté corriendo
echo "1. Verificando Docker..."
if ! docker info >/dev/null 2>&1; then
    echo -e "${RED}❌ Docker no está corriendo${NC}"
    exit 1
fi
echo -e "${GREEN}✅ Docker está corriendo${NC}"
echo ""

# Verificar que los contenedores estén corriendo
echo "2. Verificando contenedores..."
containers=$(docker-compose ps --services --filter "status=running" | wc -l)
if [ "$containers" -lt 3 ]; then
    echo -e "${RED}❌ No todos los contenedores están corriendo${NC}"
    docker-compose ps
    exit 1
fi
echo -e "${GREEN}✅ Todos los contenedores están corriendo${NC}"
echo ""

# Esperar a que los servicios estén listos
echo "3. Esperando a que los servicios estén listos..."
sleep 5
echo ""

# Verificar servicios
echo "4. Verificando servicios HTTP..."
echo ""

# Frontend
check_service "Frontend (Angular)" "http://localhost:4200" 200

# Backend - Clientes
check_service "Backend - Clientes" "http://localhost:5000/api/clientes" 200

# Backend - Productos
check_service "Backend - Productos" "http://localhost:5000/api/productos" 200

# Backend - Facturas
check_service "Backend - Facturas" "http://localhost:5000/api/facturas" 200

echo ""

# Verificar base de datos
echo "5. Verificando base de datos..."
db_check=$(docker-compose exec -T postgres psql -U postgres -d seven_facturacion_dev -c "SELECT COUNT(*) FROM facturacion.usuarios;" 2>/dev/null | grep -o '[0-9]\+' | head -1)

if [ "$db_check" -ge 1 ]; then
    echo -e "${GREEN}✅ Base de datos tiene datos${NC}"
else
    echo -e "${RED}❌ Base de datos no tiene datos${NC}"
    exit 1
fi

echo ""

# Verificar login
echo "6. Verificando autenticación..."
login_response=$(curl -s -X POST http://localhost:5000/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{"username":"admin","password":"admin123"}')

if echo "$login_response" | grep -q "token"; then
    echo -e "${GREEN}✅ Login funciona correctamente${NC}"
    token=$(echo "$login_response" | grep -o '"token":"[^"]*' | cut -d'"' -f4)
    echo "   Token: $token"
else
    echo -e "${RED}❌ Login falló${NC}"
    echo "   Respuesta: $login_response"
    exit 1
fi

echo ""
echo "=========================================="
echo -e "${GREEN}✅ Todas las pruebas pasaron exitosamente${NC}"
echo "=========================================="
echo ""
echo "El sistema está funcionando correctamente."
echo "Puedes acceder a:"
echo "  - Frontend: http://localhost:4200"
echo "  - Backend:  http://localhost:5000"
echo ""
echo "Credenciales:"
echo "  - Usuario: admin"
echo "  - Contraseña: admin123"
echo ""

