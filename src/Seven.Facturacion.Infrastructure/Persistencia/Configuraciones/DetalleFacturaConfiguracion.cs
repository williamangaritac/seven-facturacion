// ============================================================================
// CONFIGURACIÓN: DetalleFacturaConfiguracion
// Descripción: Configuración de Fluent API para la entidad DetalleFactura
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Configuración de Entity Framework Core para la entidad DetalleFactura.
/// Implementa IEntityTypeConfiguration para separar responsabilidades.
/// </summary>
public class DetalleFacturaConfiguracion : IEntityTypeConfiguration<DetalleFactura>
{
    /// <summary>
    /// Configura la entidad DetalleFactura.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad.</param>
    public void Configure(EntityTypeBuilder<DetalleFactura> builder)
    {
        // ====================================================================
        // TABLA
        // ====================================================================
        builder.ToTable("detalles_factura");

        // ====================================================================
        // CLAVE PRIMARIA
        // ====================================================================
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // ====================================================================
        // PROPIEDADES
        // ====================================================================
        builder.Property(d => d.FacturaId)
            .HasColumnName("factura_id")
            .IsRequired();

        builder.Property(d => d.ProductoId)
            .HasColumnName("producto_id")
            .IsRequired();

        builder.Property(d => d.Cantidad)
            .HasColumnName("cantidad")
            .IsRequired();

        builder.Property(d => d.PrecioUnitario)
            .HasColumnName("precio_unitario")
            .HasPrecision(18, 2)
            .IsRequired();

        // ====================================================================
        // PROPIEDADES IGNORADAS (Calculadas)
        // ====================================================================
        builder.Ignore(d => d.Subtotal);

        // ====================================================================
        // RELACIONES
        // ====================================================================
        
        // Relación Many-to-One: DetalleFactura -> Factura
        // (Ya configurada en FacturaConfiguracion, aquí solo referencia)
        builder.HasOne(d => d.Factura)
            .WithMany(f => f.Detalles)
            .HasForeignKey(d => d.FacturaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Many-to-One: DetalleFactura -> Producto
        builder.HasOne(d => d.Producto)
            .WithMany(p => p.DetallesFactura)
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_detalles_productos");

        // ====================================================================
        // ÍNDICES
        // ====================================================================
        builder.HasIndex(d => d.FacturaId)
            .HasDatabaseName("ix_detalles_factura_factura_id");

        builder.HasIndex(d => d.ProductoId)
            .HasDatabaseName("ix_detalles_factura_producto_id");

        // Índice compuesto para evitar duplicados de producto en misma factura
        builder.HasIndex(d => new { d.FacturaId, d.ProductoId })
            .IsUnique()
            .HasDatabaseName("ix_detalles_factura_factura_producto");
    }
}

