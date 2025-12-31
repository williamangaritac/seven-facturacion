#!/bin/bash

# ============================================================================
# Script para levantar el Frontend en modo desarrollo - Linux/Mac
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
echo -e "${CYAN}Levantar Frontend Angular en Desarrollo${NC}"
echo -e "${CYAN}Sistema de Facturación Seven${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""

# Verificar si Node.js está instalado
echo -e "${YELLOW}Verificando Node.js...${NC}"
if ! command -v node &> /dev/null; then
    echo -e "${RED}❌ Node.js no está instalado${NC}"
    echo -e "${YELLOW}Por favor instala Node.js desde: https://nodejs.org/${NC}"
    exit 1
fi

node_version=$(node --version)
echo -e "${GREEN}✅ Node.js instalado: $node_version${NC}"
echo ""

# Verificar si npm está instalado
echo -e "${YELLOW}Verificando npm...${NC}"
if ! command -v npm &> /dev/null; then
    echo -e "${RED}❌ npm no está instalado${NC}"
    exit 1
fi

npm_version=$(npm --version)
echo -e "${GREEN}✅ npm instalado: $npm_version${NC}"
echo ""

# Navegar a la carpeta del frontend
frontend_path="frontend_angular"

if [ ! -d "$frontend_path" ]; then
    echo -e "${RED}❌ No se encuentra la carpeta $frontend_path${NC}"
    exit 1
fi

cd "$frontend_path"

# Verificar si node_modules existe
if [ ! -d "node_modules" ]; then
    echo -e "${YELLOW}⚠️  node_modules no encontrado${NC}"
    echo -e "${YELLOW}Instalando dependencias...${NC}"
    npm install
    
    if [ $? -ne 0 ]; then
        echo -e "${RED}❌ Error al instalar dependencias${NC}"
        exit 1
    fi
    
    echo -e "${GREEN}✅ Dependencias instaladas${NC}"
    echo ""
fi

# Verificar que el backend esté corriendo
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Verificación del Backend${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""
echo -e "${YELLOW}⚠️  IMPORTANTE: Asegúrate de que el backend esté corriendo${NC}"
echo ""
echo -e "${CYAN}El frontend se conectará a:${NC}"
echo -e "${WHITE}  https://localhost:49497/api${NC}"
echo ""
echo -e "${YELLOW}Si el backend NO está corriendo, levántalo con:${NC}"
echo -e "${WHITE}  cd ..${NC}"
echo -e "${WHITE}  cd src/Seven.Facturacion.Api${NC}"
echo -e "${WHITE}  dotnet run${NC}"
echo ""

read -p "¿Continuar levantando el frontend? (s/n): " continue

if [ "$continue" != "s" ] && [ "$continue" != "S" ]; then
    echo -e "${RED}❌ Operación cancelada${NC}"
    exit 0
fi

# Levantar el frontend
echo ""
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Levantando Frontend Angular${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""
echo -e "${YELLOW}Ejecutando: ng serve${NC}"
echo ""
echo -e "${CYAN}El frontend estará disponible en:${NC}"
echo -e "${WHITE}  http://localhost:4200${NC}"
echo ""
echo -e "${CYAN}Credenciales:${NC}"
echo -e "${WHITE}  Usuario: admin${NC}"
echo -e "${WHITE}  Contraseña: admin123${NC}"
echo ""
echo -e "${YELLOW}Presiona Ctrl+C para detener el servidor${NC}"
echo ""
echo -e "${CYAN}============================================${NC}"
echo ""

# Ejecutar ng serve
npm start

