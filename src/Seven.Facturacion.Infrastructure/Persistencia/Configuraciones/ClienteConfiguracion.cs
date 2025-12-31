// ============================================================================
// CONFIGURACIÓN: ClienteConfiguracion
// Descripción: Configuración de Fluent API para la entidad Cliente
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Configuración de Entity Framework Core para la entidad Cliente.
/// Implementa IEntityTypeConfiguration para separar responsabilidades.
/// </summary>
public class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
{
    /// <summary>
    /// Configura la entidad Cliente.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad.</param>
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        // ====================================================================
        // TABLA
        // ====================================================================
        builder.ToTable("clientes");

        // ====================================================================
        // CLAVE PRIMARIA
        // ====================================================================
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // ====================================================================
        // PROPIEDADES
        // ====================================================================
        builder.Property(c => c.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Apellido)
            .HasColumnName("apellido")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.CorreoElectronico)
            .HasColumnName("correo_electronico")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(20);

        builder.Property(c => c.FechaNacimiento)
            .HasColumnName("fecha_nacimiento")
            .IsRequired();

        builder.Property(c => c.Direccion)
            .HasColumnName("direccion")
            .HasMaxLength(500);

        builder.Property(c => c.Activo)
            .HasColumnName("activo")
            .HasDefaultValue(true)
            .IsRequired();

        // ====================================================================
        // PROPIEDADES DE AUDITORÍA
        // ====================================================================
        builder.Property(c => c.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        builder.Property(c => c.FechaActualizacion)
            .HasColumnName("fecha_actualizacion")
            .IsRequired();

        // ====================================================================
        // PROPIEDADES IGNORADAS (Calculadas)
        // ====================================================================
        builder.Ignore(c => c.NombreCompleto);
        builder.Ignore(c => c.Edad);

        // ====================================================================
        // ÍNDICES
        // ====================================================================
        builder.HasIndex(c => c.CorreoElectronico)
            .IsUnique()
            .HasDatabaseName("ix_clientes_correo_electronico");

        builder.HasIndex(c => c.Activo)
            .HasDatabaseName("ix_clientes_activo");

        builder.HasIndex(c => c.FechaNacimiento)
            .HasDatabaseName("ix_clientes_fecha_nacimiento");
    }
}

