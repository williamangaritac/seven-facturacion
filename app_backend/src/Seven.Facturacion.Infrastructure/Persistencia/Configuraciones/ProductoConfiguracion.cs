// ============================================================================
// CONFIGURACIÓN: ProductoConfiguracion
// Descripción: Configuración de Fluent API para la entidad Producto
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Producto.
/// Implementa IEntityTypeConfiguration para separar responsabilidades.
/// </summary>
public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
{
    /// <summary>
    /// Configura la entidad Producto.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad.</param>
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        // ====================================================================
        // TABLA
        // ====================================================================
        builder.ToTable("productos");

        // ====================================================================
        // CLAVE PRIMARIA
        // ====================================================================
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // ====================================================================
        // PROPIEDADES
        // ====================================================================
        builder.Property(p => p.Codigo)
            .HasColumnName("codigo")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(1000);

        builder.Property(p => p.Precio)
            .HasColumnName("precio")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Stock)
            .HasColumnName("stock")
            .IsRequired();

        builder.Property(p => p.Activo)
            .HasColumnName("activo")
            .HasDefaultValue(true)
            .IsRequired();

        // ====================================================================
        // PROPIEDADES DE AUDITORÍA
        // ====================================================================
        builder.Property(p => p.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        builder.Property(p => p.FechaActualizacion)
            .HasColumnName("fecha_actualizacion")
            .IsRequired();

        // ====================================================================
        // PROPIEDADES IGNORADAS (Calculadas)
        // ====================================================================
        builder.Ignore(p => p.TieneStockBajo);

        // ====================================================================
        // ÍNDICES
        // ====================================================================
        builder.HasIndex(p => p.Codigo)
            .IsUnique()
            .HasDatabaseName("ix_productos_codigo");

        builder.HasIndex(p => p.Activo)
            .HasDatabaseName("ix_productos_activo");

        builder.HasIndex(p => p.Stock)
            .HasDatabaseName("ix_productos_stock");

        builder.HasIndex(p => p.Nombre)
            .HasDatabaseName("ix_productos_nombre");
    }
}

