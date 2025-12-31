# ============================================================================
# Makefile - Sistema de Facturación Seven
# Comandos útiles para desarrollo y despliegue
# ============================================================================

.PHONY: help build up down restart logs clean test

# Colores para output
BLUE := \033[0;34m
GREEN := \033[0;32m
YELLOW := \033[0;33m
RED := \033[0;31m
NC := \033[0m # No Color

help: ## Mostrar esta ayuda
	@echo "$(BLUE)Sistema de Facturación Seven - Comandos Disponibles$(NC)"
	@echo ""
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "$(GREEN)%-20s$(NC) %s\n", $$1, $$2}'

# ============================================================================
# DOCKER COMMANDS
# ============================================================================

build: ## Construir todas las imágenes Docker
	@echo "$(BLUE)Construyendo imágenes Docker...$(NC)"
	docker-compose build

up: ## Levantar todos los servicios
	@echo "$(BLUE)Levantando servicios...$(NC)"
	docker-compose up -d
	@echo "$(GREEN)✅ Servicios levantados$(NC)"
	@echo "$(YELLOW)Frontend: http://localhost:4200$(NC)"
	@echo "$(YELLOW)Backend:  http://localhost:5000$(NC)"

down: ## Detener todos los servicios
	@echo "$(BLUE)Deteniendo servicios...$(NC)"
	docker-compose down
	@echo "$(GREEN)✅ Servicios detenidos$(NC)"

restart: ## Reiniciar todos los servicios
	@echo "$(BLUE)Reiniciando servicios...$(NC)"
	docker-compose restart
	@echo "$(GREEN)✅ Servicios reiniciados$(NC)"

logs: ## Ver logs de todos los servicios
	docker-compose logs -f

logs-api: ## Ver logs del backend
	docker-compose logs -f api

logs-frontend: ## Ver logs del frontend
	docker-compose logs -f frontend

logs-db: ## Ver logs de la base de datos
	docker-compose logs -f postgres

ps: ## Ver estado de los contenedores
	docker-compose ps

clean: ## Limpiar contenedores, volúmenes e imágenes
	@echo "$(RED)⚠️  Esto eliminará todos los contenedores, volúmenes e imágenes$(NC)"
	@echo "$(YELLOW)Presiona Ctrl+C para cancelar...$(NC)"
	@sleep 3
	docker-compose down -v
	docker system prune -f
	@echo "$(GREEN)✅ Limpieza completada$(NC)"

rebuild: down build up ## Reconstruir y levantar servicios

# ============================================================================
# DATABASE COMMANDS
# ============================================================================

db-shell: ## Acceder a la consola de PostgreSQL
	docker-compose exec postgres psql -U postgres -d seven_facturacion_dev

db-reset: ## Resetear la base de datos
	@echo "$(RED)⚠️  Esto eliminará todos los datos de la base de datos$(NC)"
	@echo "$(YELLOW)Presiona Ctrl+C para cancelar...$(NC)"
	@sleep 3
	docker-compose down -v
	docker-compose up -d postgres
	@echo "$(GREEN)✅ Base de datos reseteada$(NC)"

db-backup: ## Crear backup de la base de datos
	@echo "$(BLUE)Creando backup...$(NC)"
	docker-compose exec -T postgres pg_dump -U postgres seven_facturacion_dev > backup_$(shell date +%Y%m%d_%H%M%S).sql
	@echo "$(GREEN)✅ Backup creado$(NC)"

# ============================================================================
# DEVELOPMENT COMMANDS
# ============================================================================

dev-backend: ## Ejecutar backend en modo desarrollo (local)
	cd src/Seven.Facturacion.Api && dotnet run

dev-frontend: ## Ejecutar frontend en modo desarrollo (local)
	cd frontend_angular && npm start

install-backend: ## Instalar dependencias del backend
	cd src/Seven.Facturacion.Api && dotnet restore

install-frontend: ## Instalar dependencias del frontend
	cd frontend_angular && npm install

test-backend: ## Ejecutar tests del backend
	dotnet test

test-frontend: ## Ejecutar tests del frontend
	cd frontend_angular && npm test

# ============================================================================
# UTILITY COMMANDS
# ============================================================================

check: ## Verificar que Docker esté instalado y corriendo
	@echo "$(BLUE)Verificando requisitos...$(NC)"
	@command -v docker >/dev/null 2>&1 || { echo "$(RED)❌ Docker no está instalado$(NC)"; exit 1; }
	@docker info >/dev/null 2>&1 || { echo "$(RED)❌ Docker no está corriendo$(NC)"; exit 1; }
	@command -v docker-compose >/dev/null 2>&1 || { echo "$(RED)❌ Docker Compose no está instalado$(NC)"; exit 1; }
	@echo "$(GREEN)✅ Todos los requisitos están instalados$(NC)"

stats: ## Ver estadísticas de uso de recursos
	docker stats

# ============================================================================
# DEFAULT
# ============================================================================

.DEFAULT_GOAL := help

