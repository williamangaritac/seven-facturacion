// ============================================================================
// DTOs: Cliente
// Descripción: Records inmutables para transferencia de datos de clientes
// ============================================================================

namespace Seven.Facturacion.Application.DTOs;

/// <summary>
/// DTO de lectura para cliente con información completa.
/// </summary>
/// <param name="Id">Identificador único.</param>
/// <param name="Nombre">Nombre del cliente.</param>
/// <param name="Apellido">Apellido del cliente.</param>
/// <param name="NombreCompleto">Nombre y apellido concatenados.</param>
/// <param name="CorreoElectronico">Correo electrónico.</param>
/// <param name="Telefono">Teléfono (opcional).</param>
/// <param name="FechaNacimiento">Fecha de nacimiento.</param>
/// <param name="Edad">Edad calculada.</param>
/// <param name="Direccion">Dirección física (opcional).</param>
/// <param name="Activo">Estado del cliente.</param>
public record ClienteDto(
    int Id,
    string Nombre,
    string Apellido,
    string NombreCompleto,
    string CorreoElectronico,
    string? Telefono,
    DateOnly FechaNacimiento,
    int Edad,
    string? Direccion,
    bool Activo
);

/// <summary>
/// DTO para crear un nuevo cliente.
/// </summary>
public record CrearClienteDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; init; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    public string Apellido { get; init; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [StringLength(255, ErrorMessage = "El correo no puede exceder 255 caracteres")]
    public string CorreoElectronico { get; init; } = string.Empty;

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Telefono { get; init; }

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateOnly FechaNacimiento { get; init; }

    [StringLength(500, ErrorMessage = "La dirección no puede exceder 500 caracteres")]
    public string? Direccion { get; init; }
}

/// <summary>
/// DTO para actualizar un cliente existente.
/// </summary>
public record ActualizarClienteDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; init; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    public string Apellido { get; init; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [StringLength(255, ErrorMessage = "El correo no puede exceder 255 caracteres")]
    public string CorreoElectronico { get; init; } = string.Empty;

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Telefono { get; init; }

    [StringLength(500, ErrorMessage = "La dirección no puede exceder 500 caracteres")]
    public string? Direccion { get; init; }

    public bool Activo { get; init; } = true;
}

/// <summary>
/// DTO para la consulta de clientes por edad y fecha de compra.
/// </summary>
/// <param name="Id">Identificador del cliente.</param>
/// <param name="NombreCompleto">Nombre completo.</param>
/// <param name="CorreoElectronico">Correo electrónico.</param>
/// <param name="Edad">Edad del cliente.</param>
/// <param name="TotalComprasPeriodo">Número de compras en el período.</param>
public record ClientePorEdadYCompraDto(
    int Id,
    string NombreCompleto,
    string CorreoElectronico,
    int Edad,
    int TotalComprasPeriodo
);

