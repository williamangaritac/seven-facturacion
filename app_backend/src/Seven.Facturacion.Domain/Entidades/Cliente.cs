// ============================================================================
// ENTIDAD: Cliente
// Descripción: Representa un cliente del sistema de facturación
// ============================================================================

namespace Seven.Facturacion.Domain.Entidades;

/// <summary>
/// Entidad que representa un cliente en el sistema de facturación.
/// </summary>
public class Cliente
{
    /// <summary>
    /// Identificador único del cliente.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del cliente.
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string Nombre { get; set; }

    /// <summary>
    /// Apellido del cliente.
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string Apellido { get; set; }

    /// <summary>
    /// Correo electrónico único del cliente.
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public required string CorreoElectronico { get; set; }

    /// <summary>
    /// Número de teléfono del cliente (opcional).
    /// </summary>
    [Phone]
    [StringLength(20)]
    public string? Telefono { get; set; }

    /// <summary>
    /// Fecha de nacimiento del cliente.
    /// </summary>
    [Required]
    public required DateOnly FechaNacimiento { get; set; }

    /// <summary>
    /// Dirección física del cliente (opcional).
    /// </summary>
    [StringLength(500)]
    public string? Direccion { get; set; }

    /// <summary>
    /// Indica si el cliente está activo en el sistema.
    /// </summary>
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización del registro.
    /// </summary>
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    // ========================================================================
    // PROPIEDADES DE NAVEGACIÓN
    // ========================================================================

    /// <summary>
    /// Colección de facturas asociadas al cliente.
    /// Inicializada con collection expression de C# 14.
    /// </summary>
    public ICollection<Factura> Facturas { get; set; } = [];

    // ========================================================================
    // PROPIEDADES CALCULADAS
    // ========================================================================

    /// <summary>
    /// Calcula la edad del cliente basándose en su fecha de nacimiento.
    /// </summary>
    public int Edad
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var edad = hoy.Year - FechaNacimiento.Year;
            
            // Ajustar si aún no ha cumplido años este año
            if (FechaNacimiento > hoy.AddYears(-edad))
            {
                edad--;
            }
            
            return edad;
        }
    }

    /// <summary>
    /// Nombre completo del cliente (Nombre + Apellido).
    /// </summary>
    public string NombreCompleto => $"{Nombre} {Apellido}";
}

