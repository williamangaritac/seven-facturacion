// ============================================================================
// CONFIGURACIÓN: InyeccionDependencias
// Descripción: Extension method para registrar servicios de infraestructura
// ============================================================================

namespace Seven.Facturacion.Infrastructure;

/// <summary>
/// Extensiones para configurar la inyección de dependencias de la capa Infrastructure.
/// </summary>
public static class InyeccionDependencias
{
    /// <summary>
    /// Agrega los servicios de infraestructura al contenedor de DI.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="connectionString">Cadena de conexión a PostgreSQL.</param>
    /// <returns>La colección de servicios para encadenamiento.</returns>
    public static IServiceCollection AgregarInfraestructura(
        this IServiceCollection services,
        string connectionString)
    {
        // ====================================================================
        // ENTITY FRAMEWORK CORE - PostgreSQL
        // ====================================================================
        services.AddDbContext<AppDbContext>(opciones =>
        {
            opciones.UseNpgsql(connectionString, npgsqlOpciones =>
            {
                // Configuración de resiliencia para PostgreSQL
                npgsqlOpciones.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);

                // Configurar el esquema de migraciones
                npgsqlOpciones.MigrationsHistoryTable("__EFMigrationsHistory", "facturacion");
            });

            // Habilitar logging detallado solo en desarrollo
            #if DEBUG
            opciones.EnableSensitiveDataLogging();
            opciones.EnableDetailedErrors();
            #endif
        });

        // ====================================================================
        // REPOSITORIOS (Scoped - Por request)
        // ====================================================================
        services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
        services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
        services.AddScoped<IFacturaRepositorio, FacturaRepositorio>();

        // ====================================================================
        // UNIDAD DE TRABAJO (Scoped - Por request)
        // ====================================================================
        services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

        // ====================================================================
        // SERVICIOS DE APLICACIÓN (Scoped - Por request)
        // ====================================================================
        services.AddScoped<IClienteServicio, ClienteServicio>();
        services.AddScoped<IProductoServicio, ProductoServicio>();
        services.AddScoped<IFacturaServicio, FacturaServicio>();

        return services;
    }

    /// <summary>
    /// Aplica las migraciones pendientes a la base de datos.
    /// Útil para desarrollo y despliegue automático.
    /// </summary>
    /// <param name="serviceProvider">Proveedor de servicios.</param>
    public static void AplicarMigraciones(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        contexto.Database.Migrate();
    }

    /// <summary>
    /// Verifica la conexión a la base de datos.
    /// </summary>
    /// <param name="serviceProvider">Proveedor de servicios.</param>
    /// <returns>True si la conexión es exitosa.</returns>
    public static async Task<bool> VerificarConexionAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var contexto = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await contexto.Database.CanConnectAsync();
    }
}

