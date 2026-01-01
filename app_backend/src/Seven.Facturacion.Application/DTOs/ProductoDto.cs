// ============================================================================
// DTOs: Producto
// Descripción: Records inmutables para transferencia de datos de productos
// ============================================================================

namespace Seven.Facturacion.Application.DTOs;

/// <summary>
/// DTO de lectura para producto con información completa.
/// </summary>
/// <param name="Id">Identificador único.</param>
/// <param name="Codigo">Código único del producto.</param>
/// <param name="Nombre">Nombre del producto.</param>
/// <param name="Descripcion">Descripción detallada (opcional).</param>
/// <param name="Precio">Precio unitario.</param>
/// <param name="Stock">Cantidad disponible.</param>
/// <param name="Activo">Estado del producto.</param>
/// <param name="TieneStockBajo">Indica si el stock es bajo (≤5).</param>
/// <param name="AnioUltimaVenta">Año de la última venta del producto (null si nunca se ha vendido).</param>
public record ProductoDto(
    int Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock,
    bool Activo,
    bool TieneStockBajo,
    int? AnioUltimaVenta
);

/// <summary>
/// DTO para crear un nuevo producto.
/// </summary>
public record CrearProductoDto
{
    [Required(ErrorMessage = "El código es requerido")]
    [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
    public string Codigo { get; init; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; init; } = string.Empty;

    public string? Descripcion { get; init; }

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
    public decimal Precio { get; init; }

    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; init; }
}

/// <summary>
/// DTO para actualizar un producto existente.
/// </summary>
public record ActualizarProductoDto
{
    [Required(ErrorMessage = "El código es requerido")]
    [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
    public string Codigo { get; init; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; init; } = string.Empty;

    public string? Descripcion { get; init; }

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
    public decimal Precio { get; init; }

    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; init; }

    public bool Activo { get; init; } = true;
}

/// <summary>
/// DTO para la lista de precios (Consulta SQL #1).
/// </summary>
/// <param name="Id">Identificador del producto.</param>
/// <param name="Codigo">Código del producto.</param>
/// <param name="Nombre">Nombre del producto.</param>
/// <param name="Precio">Precio unitario.</param>
public record ListaPrecioDto(
    int Id,
    string Codigo,
    string Nombre,
    decimal Precio
);

/// <summary>
/// DTO para productos con stock bajo (Consulta SQL #2).
/// </summary>
/// <param name="Id">Identificador del producto.</param>
/// <param name="Codigo">Código del producto.</param>
/// <param name="Nombre">Nombre del producto.</param>
/// <param name="Stock">Cantidad disponible.</param>
/// <param name="NivelAlerta">Nivel de alerta: AGOTADO, CRÍTICO, BAJO.</param>
public record ProductoBajoStockDto(
    int Id,
    string Codigo,
    string Nombre,
    int Stock,
    string NivelAlerta
)
{
    /// <summary>
    /// Calcula el nivel de alerta basándose en el stock.
    /// </summary>
    public static string CalcularNivelAlerta(int stock) => stock switch
    {
        0 => "AGOTADO",
        <= 2 => "CRÍTICO",
        _ => "BAJO"
    };
}

