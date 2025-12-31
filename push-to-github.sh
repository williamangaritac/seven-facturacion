#!/bin/bash

# ============================================================================
# Script para subir el proyecto a GitHub - Linux/Mac
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
echo -e "${CYAN}Subir Proyecto a GitHub${NC}"
echo -e "${CYAN}Sistema de Facturación Seven${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""

# Verificar si Git está instalado
echo -e "${YELLOW}Verificando Git...${NC}"
if ! command -v git &> /dev/null; then
    echo -e "${RED}❌ Git no está instalado${NC}"
    echo -e "${YELLOW}Por favor instala Git:${NC}"
    echo -e "${WHITE}  - Ubuntu/Debian: sudo apt-get install git${NC}"
    echo -e "${WHITE}  - macOS: brew install git${NC}"
    exit 1
fi

git_version=$(git --version)
echo -e "${GREEN}✅ Git instalado: $git_version${NC}"
echo ""

# Verificar si ya está inicializado
echo -e "${YELLOW}Verificando repositorio Git...${NC}"
if [ ! -d ".git" ]; then
    echo -e "${YELLOW}⚠️  Repositorio no inicializado${NC}"
    echo -e "${YELLOW}Inicializando repositorio...${NC}"
    git init
    echo -e "${GREEN}✅ Repositorio inicializado${NC}"
else
    echo -e "${GREEN}✅ Repositorio ya inicializado${NC}"
fi
echo ""

# Verificar configuración de Git
echo -e "${YELLOW}Verificando configuración de Git...${NC}"
user_name=$(git config user.name)
user_email=$(git config user.email)

if [ -z "$user_name" ] || [ -z "$user_email" ]; then
    echo -e "${YELLOW}⚠️  Git no está configurado${NC}"
    echo ""
    
    read -p "Ingresa tu nombre: " name
    read -p "Ingresa tu email (el mismo de GitHub): " email
    
    git config --global user.name "$name"
    git config --global user.email "$email"
    
    echo -e "${GREEN}✅ Git configurado correctamente${NC}"
else
    echo -e "${GREEN}✅ Usuario: $user_name${NC}"
    echo -e "${GREEN}✅ Email: $user_email${NC}"
fi
echo ""

# Preguntar por la URL del repositorio
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Configuración del Repositorio Remoto${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""
echo -e "${YELLOW}Primero, crea un repositorio en GitHub:${NC}"
echo -e "${WHITE}1. Ve a https://github.com/new${NC}"
echo -e "${WHITE}2. Nombre: seven-facturacion-system (o el que prefieras)${NC}"
echo -e "${WHITE}3. NO marques 'Initialize with README'${NC}"
echo -e "${WHITE}4. Crea el repositorio${NC}"
echo ""

read -p "Ingresa la URL del repositorio (ej: https://github.com/usuario/repo.git): " repo_url

if [ -z "$repo_url" ]; then
    echo -e "${RED}❌ URL no proporcionada${NC}"
    exit 1
fi

# Verificar si ya existe el remote origin
existing_remote=$(git remote get-url origin 2>/dev/null)

if [ -n "$existing_remote" ]; then
    echo -e "${YELLOW}⚠️  Ya existe un remote 'origin': $existing_remote${NC}"
    read -p "¿Deseas reemplazarlo? (s/n): " replace
    
    if [ "$replace" = "s" ] || [ "$replace" = "S" ]; then
        git remote remove origin
        git remote add origin "$repo_url"
        echo -e "${GREEN}✅ Remote 'origin' actualizado${NC}"
    fi
else
    git remote add origin "$repo_url"
    echo -e "${GREEN}✅ Remote 'origin' agregado${NC}"
fi
echo ""

# Verificar archivos a commitear
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Preparando Archivos${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""

echo -e "${YELLOW}Archivos a subir:${NC}"
git status --short
echo ""

read -p "¿Continuar con el commit? (s/n): " continue

if [ "$continue" != "s" ] && [ "$continue" != "S" ]; then
    echo -e "${RED}❌ Operación cancelada${NC}"
    exit 0
fi

# Agregar archivos
echo -e "${YELLOW}Agregando archivos...${NC}"
git add .
echo -e "${GREEN}✅ Archivos agregados${NC}"
echo ""

# Hacer commit
echo -e "${YELLOW}Mensaje del commit:${NC}"
read -p "Ingresa el mensaje (Enter para usar el predeterminado): " commit_message

if [ -z "$commit_message" ]; then
    commit_message="Initial commit: Sistema de Facturación Seven con Docker"
fi

git commit -m "$commit_message"
echo -e "${GREEN}✅ Commit realizado${NC}"
echo ""

# Verificar rama
current_branch=$(git branch --show-current)

if [ -z "$current_branch" ]; then
    echo -e "${YELLOW}Creando rama 'main'...${NC}"
    git branch -M main
    current_branch="main"
fi

echo -e "${CYAN}Rama actual: $current_branch${NC}"
echo ""

# Subir a GitHub
echo -e "${CYAN}============================================${NC}"
echo -e "${CYAN}Subiendo a GitHub${NC}"
echo -e "${CYAN}============================================${NC}"
echo ""

echo -e "${YELLOW}Subiendo a GitHub...${NC}"
echo -e "${YELLOW}Si te pide contraseña, usa un Personal Access Token${NC}"
echo -e "${YELLOW}Más info: https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token${NC}"
echo ""

git push -u origin "$current_branch"

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}============================================${NC}"
    echo -e "${GREEN}✅ ¡Proyecto subido exitosamente a GitHub!${NC}"
    echo -e "${GREEN}============================================${NC}"
    echo ""
    echo -e "${CYAN}Tu repositorio está en:${NC}"
    echo -e "${WHITE}$repo_url${NC}"
    echo ""
    echo -e "${CYAN}Ahora puedes clonarlo con:${NC}"
    echo -e "${WHITE}git clone $repo_url${NC}"
    echo ""
else
    echo ""
    echo -e "${RED}============================================${NC}"
    echo -e "${RED}❌ Error al subir a GitHub${NC}"
    echo -e "${RED}============================================${NC}"
    echo ""
    echo -e "${YELLOW}Posibles soluciones:${NC}"
    echo -e "${WHITE}1. Verifica que la URL del repositorio sea correcta${NC}"
    echo -e "${WHITE}2. Asegúrate de tener permisos en el repositorio${NC}"
    echo -e "${WHITE}3. Usa un Personal Access Token en lugar de contraseña${NC}"
    echo -e "${WHITE}4. Consulta GITHUB_SETUP.md para más ayuda${NC}"
    echo ""
fi

