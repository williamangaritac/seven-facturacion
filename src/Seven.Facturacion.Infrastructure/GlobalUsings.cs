// ============================================================================
// GLOBAL USINGS - Capa Infrastructure
// Descripción: Importaciones globales para toda la capa de infraestructura
// ============================================================================

// Entity Framework Core
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Microsoft Extensions
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

// Domain - Entidades
global using Seven.Facturacion.Domain.Entidades;

// Domain - Enumeraciones (el namespace se fusiona con Entidades)
global using EstadoFactura = Seven.Facturacion.Domain.Entidades.EstadoFactura;

// Domain - Interfaces de repositorios
global using Seven.Facturacion.Domain.Interfaces;

// Application - Servicios
global using Seven.Facturacion.Application.Servicios;

// Infrastructure - Persistencia
global using Seven.Facturacion.Infrastructure.Persistencia;
global using Seven.Facturacion.Infrastructure.Persistencia.Configuraciones;

// Infrastructure - Repositorios
global using Seven.Facturacion.Infrastructure.Repositorios;

