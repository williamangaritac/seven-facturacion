namespace Seven.Facturacion.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Configuración de la entidad Usuario.
/// </summary>
public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios", "facturacion");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .IsRequired();

        builder.Property(u => u.Activo)
            .HasColumnName("activo")
            .IsRequired();

        builder.HasIndex(u => u.Username)
            .IsUnique();
    }
}

