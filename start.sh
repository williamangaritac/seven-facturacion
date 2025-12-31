#!/bin/bash

# ============================================================================
# Script de inicio rápido - Sistema de Facturación Seven
# Para Linux/Mac
# ============================================================================

set -e

echo "=========================================="
echo "Sistema de Facturación Seven"
echo "Inicio Rápido con Docker"
echo "=========================================="
echo ""

# Verificar si Docker está instalado
if ! command -v docker &> /dev/null; then
    echo "❌ Error: Docker no está instalado"
    echo "Por favor instala Docker desde: https://www.docker.com/get-started"
    exit 1
fi

# Verificar si Docker Compose está instalado
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Error: Docker Compose no está instalado"
    echo "Por favor instala Docker Compose desde: https://docs.docker.com/compose/install/"
    exit 1
fi

echo "✅ Docker y Docker Compose están instalados"
echo ""

# Verificar si Docker está corriendo
if ! docker info &> /dev/null; then
    echo "❌ Error: Docker no está corriendo"
    echo "Por favor inicia Docker Desktop o el servicio de Docker"
    exit 1
fi

echo "✅ Docker está corriendo"
echo ""

# Detener contenedores existentes si los hay
echo "🔄 Deteniendo contenedores existentes..."
docker-compose down 2>/dev/null || true
echo ""

# Construir y levantar servicios
echo "🏗️  Construyendo imágenes Docker..."
docker-compose build
echo ""

echo "🚀 Levantando servicios..."
docker-compose up -d
echo ""

# Esperar a que los servicios estén listos
echo "⏳ Esperando a que los servicios estén listos..."
sleep 10

# Verificar estado de los servicios
echo ""
echo "📊 Estado de los servicios:"
docker-compose ps
echo ""

# Mostrar información de acceso
echo "=========================================="
echo "✅ ¡Sistema levantado exitosamente!"
echo "=========================================="
echo ""
echo "🌐 Accede a la aplicación en:"
echo "   Frontend: http://localhost:4200"
echo "   Backend:  http://localhost:5000"
echo ""
echo "🔐 Credenciales de acceso:"
echo "   Usuario:    admin"
echo "   Contraseña: admin123"
echo ""
echo "📝 Comandos útiles:"
echo "   Ver logs:           docker-compose logs -f"
echo "   Detener servicios:  docker-compose down"
echo "   Reiniciar:          docker-compose restart"
echo ""
echo "=========================================="

