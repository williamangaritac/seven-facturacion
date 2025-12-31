// ============================================================================
// CONTEXTO: AppDbContext
// Descripción: Contexto de Entity Framework Core para PostgreSQL
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Persistencia;

/// <summary>
/// Contexto de base de datos para la aplicación de facturación.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="opciones">Opciones de configuración del contexto.</param>
public class AppDbContext(DbContextOptions<AppDbContext> opciones) : DbContext(opciones)
{
    // ========================================================================
    // DBSETS
    // ========================================================================

    /// <summary>
    /// Conjunto de entidades Cliente.
    /// </summary>
    public DbSet<Cliente> Clientes => Set<Cliente>();

    /// <summary>
    /// Conjunto de entidades Producto.
    /// </summary>
    public DbSet<Producto> Productos => Set<Producto>();

    /// <summary>
    /// Conjunto de entidades Factura.
    /// </summary>
    public DbSet<Factura> Facturas => Set<Factura>();

    /// <summary>
    /// Conjunto de entidades DetalleFactura.
    /// </summary>
    public DbSet<DetalleFactura> DetallesFactura => Set<DetalleFactura>();

    // ========================================================================
    // CONFIGURACIÓN DEL MODELO
    // ========================================================================

    /// <summary>
    /// Configura el modelo utilizando Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones desde clases separadas
        modelBuilder.ApplyConfiguration(new ClienteConfiguracion());
        modelBuilder.ApplyConfiguration(new ProductoConfiguracion());
        modelBuilder.ApplyConfiguration(new FacturaConfiguracion());
        modelBuilder.ApplyConfiguration(new DetalleFacturaConfiguracion());

        // Configurar esquema por defecto para PostgreSQL
        modelBuilder.HasDefaultSchema("facturacion");
    }

    // ========================================================================
    // SOBRESCRITURA DE GUARDADO
    // ========================================================================

    /// <summary>
    /// Sobrescribe SaveChangesAsync para auditoría automática.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ActualizarFechasAuditoria();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Sobrescribe SaveChanges para auditoría automática.
    /// </summary>
    public override int SaveChanges()
    {
        ActualizarFechasAuditoria();
        return base.SaveChanges();
    }

    // ========================================================================
    // MÉTODOS PRIVADOS
    // ========================================================================

    /// <summary>
    /// Actualiza las fechas de auditoría automáticamente para entidades con propiedades de auditoría.
    /// </summary>
    private void ActualizarFechasAuditoria()
    {
        var ahora = DateTime.UtcNow;

        // Actualizar Clientes
        foreach (var entrada in ChangeTracker.Entries<Cliente>())
        {
            if (entrada.State == EntityState.Added)
            {
                entrada.Entity.FechaCreacion = ahora;
                entrada.Entity.FechaActualizacion = ahora;
            }
            else if (entrada.State == EntityState.Modified)
            {
                entrada.Entity.FechaActualizacion = ahora;
                entrada.Property(e => e.FechaCreacion).IsModified = false;
            }
        }

        // Actualizar Productos
        foreach (var entrada in ChangeTracker.Entries<Producto>())
        {
            if (entrada.State == EntityState.Added)
            {
                entrada.Entity.FechaCreacion = ahora;
                entrada.Entity.FechaActualizacion = ahora;
            }
            else if (entrada.State == EntityState.Modified)
            {
                entrada.Entity.FechaActualizacion = ahora;
                entrada.Property(e => e.FechaCreacion).IsModified = false;
            }
        }

        // Actualizar Facturas
        foreach (var entrada in ChangeTracker.Entries<Factura>())
        {
            if (entrada.State == EntityState.Added)
            {
                entrada.Entity.FechaCreacion = ahora;
                entrada.Entity.FechaActualizacion = ahora;
            }
            else if (entrada.State == EntityState.Modified)
            {
                entrada.Entity.FechaActualizacion = ahora;
                entrada.Property(e => e.FechaCreacion).IsModified = false;
            }
        }
    }
}

