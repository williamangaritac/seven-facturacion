// ============================================================================
// CONFIGURACIÓN: FacturaConfiguracion
// Descripción: Configuración de Fluent API para la entidad Factura
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Factura.
/// Implementa IEntityTypeConfiguration para separar responsabilidades.
/// </summary>
public class FacturaConfiguracion : IEntityTypeConfiguration<Factura>
{
    /// <summary>
    /// Configura la entidad Factura.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad.</param>
    public void Configure(EntityTypeBuilder<Factura> builder)
    {
        // ====================================================================
        // TABLA
        // ====================================================================
        builder.ToTable("facturas");

        // ====================================================================
        // CLAVE PRIMARIA
        // ====================================================================
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // ====================================================================
        // PROPIEDADES
        // ====================================================================
        builder.Property(f => f.NumeroFactura)
            .HasColumnName("numero_factura")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(f => f.ClienteId)
            .HasColumnName("cliente_id")
            .IsRequired();

        builder.Property(f => f.Fecha)
            .HasColumnName("fecha")
            .IsRequired();

        builder.Property(f => f.Subtotal)
            .HasColumnName("subtotal")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(f => f.Impuesto)
            .HasColumnName("impuesto")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(f => f.Total)
            .HasColumnName("total")
            .HasPrecision(18, 2)
            .IsRequired();

        // Conversión de enum a string para PostgreSQL
        builder.Property(f => f.Estado)
            .HasColumnName("estado")
            .HasMaxLength(20)
            .HasConversion<string>()
            .IsRequired();

        // ====================================================================
        // PROPIEDADES DE AUDITORÍA
        // ====================================================================
        builder.Property(f => f.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        builder.Property(f => f.FechaActualizacion)
            .HasColumnName("fecha_actualizacion")
            .IsRequired();

        // ====================================================================
        // RELACIONES
        // ====================================================================
        
        // Relación Many-to-One: Factura -> Cliente
        builder.HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_facturas_clientes");

        // Relación One-to-Many: Factura -> Detalles
        builder.HasMany(f => f.Detalles)
            .WithOne(d => d.Factura)
            .HasForeignKey(d => d.FacturaId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_detalles_facturas");

        // ====================================================================
        // ÍNDICES
        // ====================================================================
        builder.HasIndex(f => f.NumeroFactura)
            .IsUnique()
            .HasDatabaseName("ix_facturas_numero_factura");

        builder.HasIndex(f => f.ClienteId)
            .HasDatabaseName("ix_facturas_cliente_id");

        builder.HasIndex(f => f.Fecha)
            .HasDatabaseName("ix_facturas_fecha");

        builder.HasIndex(f => f.Estado)
            .HasDatabaseName("ix_facturas_estado");

        // Índice compuesto para consultas por año y cliente
        builder.HasIndex(f => new { f.Fecha, f.ClienteId })
            .HasDatabaseName("ix_facturas_fecha_cliente");
    }
}

