#!/bin/bash

# ============================================================================
# Script rápido para hacer commit y push - Linux/Mac
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
echo -e "${CYAN}Commit y Push a GitHub${NC}"
echo -e "${CYAN}Sistema de Facturación Seven${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""

# Verificar estado
echo -e "${YELLOW}Estado actual del repositorio:${NC}"
echo ""
git status --short
echo ""

# Preguntar si continuar
read -p "¿Deseas hacer commit de estos cambios? (s/n): " continue

if [ "$continue" != "s" ] && [ "$continue" != "S" ]; then
    echo -e "${RED}❌ Operación cancelada${NC}"
    exit 0
fi

# Pedir mensaje de commit
echo ""
echo -e "${YELLOW}Mensaje del commit:${NC}"
read -p "Ingresa el mensaje: " commit_message

if [ -z "$commit_message" ]; then
    commit_message="Update: cambios en el proyecto"
fi

# Agregar todos los archivos
echo ""
echo -e "${YELLOW}Agregando archivos...${NC}"
git add .

if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Error al agregar archivos${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Archivos agregados${NC}"

# Hacer commit
echo ""
echo -e "${YELLOW}Haciendo commit...${NC}"
git commit -m "$commit_message"

if [ $? -ne 0 ]; then
    echo -e "${RED}❌ Error al hacer commit${NC}"
    exit 1
fi

echo -e "${GREEN}✅ Commit realizado${NC}"

# Obtener rama actual
current_branch=$(git branch --show-current)

# Hacer push
echo ""
echo -e "${YELLOW}Subiendo a GitHub (rama: $current_branch)...${NC}"
git push origin "$current_branch"

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}============================================${NC}"
    echo -e "${GREEN}✅ ¡Cambios subidos exitosamente!${NC}"
    echo -e "${GREEN}============================================${NC}"
    echo ""
else
    echo ""
    echo -e "${RED}============================================${NC}"
    echo -e "${RED}❌ Error al subir a GitHub${NC}"
    echo -e "${RED}============================================${NC}"
    echo ""
    echo -e "${YELLOW}Intenta:${NC}"
    echo -e "${WHITE}  git push origin $current_branch${NC}"
    echo ""
fi

