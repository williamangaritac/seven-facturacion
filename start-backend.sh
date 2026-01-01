#!/bin/bash

# ============================================================================
# Script para levantar el Backend en modo desarrollo - Linux/Mac
# Sistema de Facturación Seven
# ============================================================================

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
WHITE='\033[1;37m'
NC='\033[0m' # No Color

echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Levantar Backend .NET en Desarrollo${NC}"
echo -e "${CYAN}Sistema de Facturación Seven${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""

# Verificar si .NET está instalado
echo -e "${YELLOW}Verificando .NET SDK...${NC}"
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}❌ .NET SDK no está instalado${NC}"
    echo -e "${YELLOW}Por favor instala .NET 10 desde: https://dotnet.microsoft.com/download${NC}"
    exit 1
fi

dotnet_version=$(dotnet --version)
echo -e "${GREEN}✅ .NET SDK instalado: $dotnet_version${NC}"
echo ""

# Verificar que PostgreSQL esté corriendo
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Verificación de PostgreSQL${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""
echo -e "${YELLOW}⚠️  IMPORTANTE: Asegúrate de que PostgreSQL esté corriendo${NC}"
echo ""
echo -e "${CYAN}Opciones:${NC}"
echo -e "${WHITE}  1. PostgreSQL local en puerto 5432${NC}"
echo -e "${WHITE}  2. PostgreSQL en Docker: docker-compose up -d postgres${NC}"
echo ""

read -p "¿Continuar levantando el backend? (s/n): " continue

if [ "$continue" != "s" ] && [ "$continue" != "S" ]; then
    echo -e "${RED}❌ Operación cancelada${NC}"
    exit 0
fi

# Navegar a la carpeta del backend
backend_path="app_backend/src/Seven.Facturacion.Api"

if [ ! -d "$backend_path" ]; then
    echo -e "${RED}❌ No se encuentra la carpeta $backend_path${NC}"
    exit 1
fi

cd "$backend_path"

# Restaurar dependencias
echo ""
echo -e "${YELLOW}Restaurando dependencias...${NC}"
dotnet restore

if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Error al restaurar dependencias${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Dependencias restauradas${NC}"
echo ""

# Levantar el backend
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Levantando Backend .NET${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""
echo -e "${YELLOW}Ejecutando: dotnet run${NC}"
echo ""
echo -e "${CYAN}El backend estará disponible en:${NC}"
echo -e "${WHITE}  https://localhost:49497/api${NC}"
echo -e "${WHITE}  Swagger: https://localhost:49497/swagger${NC}"
echo ""
echo -e "${CYAN}Base de datos:${NC}"
echo -e "${WHITE}  Host: localhost${NC}"
echo -e "${WHITE}  Puerto: 5432${NC}"
echo -e "${WHITE}  Database: seven_facturacion_dev${NC}"
echo -e "${WHITE}  Usuario: postgres${NC}"
echo -e "${WHITE}  Contraseña: postgres${NC}"
echo ""
echo -e "${YELLOW}Presiona Ctrl+C para detener el servidor${NC}"
echo ""
echo -e "${CYAN}============================================${NC}"
echo ""

# Ejecutar dotnet run
dotnet run

