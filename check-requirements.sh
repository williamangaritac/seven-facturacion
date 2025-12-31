#!/bin/bash

# ============================================================================
# Script de verificación de requisitos - Sistema de Facturación Seven
# Verifica que todas las herramientas necesarias estén instaladas
# ============================================================================

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

echo -e "${CYAN}==========================================${NC}"
echo -e "${CYAN}Verificación de Requisitos${NC}"
echo -e "${CYAN}Sistema de Facturación Seven${NC}"
echo -e "${CYAN}==========================================${NC}"
echo ""

all_ok=true

# Función para verificar comando
check_command() {
    local cmd=$1
    local name=$2
    local required=$3
    
    echo -n "Verificando $name... "
    
    if command -v $cmd &> /dev/null; then
        version=$($cmd --version 2>&1 | head -n 1)
        echo -e "${GREEN}✅ Instalado ($version)${NC}"
        return 0
    else
        if [ "$required" = "true" ]; then
            echo -e "${RED}❌ No instalado (REQUERIDO)${NC}"
            return 1
        else
            echo -e "${YELLOW}⚠️  No instalado (Opcional)${NC}"
            return 0
        fi
    fi
}

echo -e "${CYAN}=== Requisitos para Docker ===${NC}"
echo ""

# Docker
if check_command "docker" "Docker" "true"; then
    # Verificar que Docker esté corriendo
    if docker info &> /dev/null; then
        echo -e "   ${GREEN}Docker está corriendo ✅${NC}"
    else
        echo -e "   ${YELLOW}Docker está instalado pero NO está corriendo ⚠️${NC}"
        echo -e "   ${YELLOW}Por favor inicia Docker${NC}"
        all_ok=false
    fi
else
    all_ok=false
fi

# Docker Compose
if ! check_command "docker-compose" "Docker Compose" "true"; then
    all_ok=false
fi

echo ""
echo -e "${CYAN}=== Requisitos para Desarrollo Local (Opcional) ===${NC}"
echo ""

# .NET SDK
if check_command "dotnet" ".NET SDK" "false"; then
    dotnet_version=$(dotnet --version)
    if [[ $dotnet_version == 10.* ]]; then
        echo -e "   ${GREEN}.NET 10 detectado ✅${NC}"
    else
        echo -e "   ${YELLOW}Versión: $dotnet_version (Se requiere .NET 10) ⚠️${NC}"
    fi
fi

# Node.js
if check_command "node" "Node.js" "false"; then
    node_version=$(node --version | sed 's/v//')
    major_version=$(echo $node_version | cut -d. -f1)
    if [ "$major_version" -ge 20 ]; then
        echo -e "   ${GREEN}Node.js >= 20 detectado ✅${NC}"
    else
        echo -e "   ${YELLOW}Versión: v$node_version (Se requiere >= 20) ⚠️${NC}"
    fi
fi

# npm
check_command "npm" "npm" "false" > /dev/null

# PostgreSQL
check_command "psql" "PostgreSQL Client" "false" > /dev/null

# Git
check_command "git" "Git" "false" > /dev/null

echo ""
echo -e "${CYAN}=== Herramientas Adicionales (Opcional) ===${NC}"
echo ""

# Angular CLI
check_command "ng" "Angular CLI" "false" > /dev/null

# Make
check_command "make" "Make" "false" > /dev/null

echo ""
echo -e "${CYAN}==========================================${NC}"

if [ "$all_ok" = true ]; then
    echo -e "${GREEN}✅ Todos los requisitos necesarios están instalados${NC}"
    echo ""
    echo -e "${CYAN}Puedes ejecutar el proyecto con:${NC}"
    echo -e "${NC}  ./start.sh${NC}"
    echo ""
    echo -e "${CYAN}O manualmente con:${NC}"
    echo -e "${NC}  docker-compose up -d${NC}"
else
    echo -e "${RED}❌ Faltan algunos requisitos necesarios${NC}"
    echo ""
    echo -e "${YELLOW}Por favor instala:${NC}"
    
    if ! command -v docker &> /dev/null; then
        echo -e "${NC}  - Docker: https://docs.docker.com/get-docker/${NC}"
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        echo -e "${NC}  - Docker Compose: https://docs.docker.com/compose/install/${NC}"
    fi
    
    echo ""
fi

echo -e "${CYAN}==========================================${NC}"
echo ""

# Información adicional
echo -e "${CYAN}📚 Documentación:${NC}"
echo -e "${NC}  - README.md - Guía general del proyecto${NC}"
echo -e "${NC}  - DOCKER_SETUP.md - Guía detallada de Docker${NC}"
echo -e "${NC}  - CONTRIBUTING.md - Guía para desarrolladores${NC}"
echo ""

if [ "$all_ok" = true ]; then
    exit 0
else
    exit 1
fi

