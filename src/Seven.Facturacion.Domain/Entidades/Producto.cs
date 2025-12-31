// ============================================================================
// ENTIDAD: Producto
// Descripción: Representa un producto del catálogo de facturación
// ============================================================================

namespace Seven.Facturacion.Domain.Entidades;

/// <summary>
/// Entidad que representa un producto en el catálogo del sistema.
/// </summary>
public class Producto
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Código único del producto (SKU).
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string Codigo { get; set; }

    /// <summary>
    /// Nombre descriptivo del producto.
    /// </summary>
    [Required]
    [StringLength(200)]
    public required string Nombre { get; set; }

    /// <summary>
    /// Descripción detallada del producto (opcional).
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Precio unitario del producto.
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
    public required decimal Precio { get; set; }

    /// <summary>
    /// Cantidad disponible en inventario.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public required int Stock { get; set; }

    /// <summary>
    /// Indica si el producto está activo en el catálogo.
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
    /// Colección de detalles de factura donde aparece este producto.
    /// Inicializada con collection expression de C# 14.
    /// </summary>
    public ICollection<DetalleFactura> DetallesFactura { get; set; } = [];

    // ========================================================================
    // PROPIEDADES CALCULADAS
    // ========================================================================

    /// <summary>
    /// Indica si el producto tiene stock bajo (≤ 5 unidades).
    /// </summary>
    public bool TieneStockBajo => Stock <= 5;

    /// <summary>
    /// Indica si el producto está agotado.
    /// </summary>
    public bool EstaAgotado => Stock == 0;

    // ========================================================================
    // MÉTODOS DE DOMINIO
    // ========================================================================

    /// <summary>
    /// Verifica si hay suficiente stock para una cantidad solicitada.
    /// </summary>
    /// <param name="cantidad">Cantidad requerida.</param>
    /// <returns>True si hay stock suficiente.</returns>
    public bool TieneStockSuficiente(int cantidad) => Stock >= cantidad;

    /// <summary>
    /// Reduce el stock del producto.
    /// </summary>
    /// <param name="cantidad">Cantidad a reducir.</param>
    /// <exception cref="DomainException">Si no hay stock suficiente.</exception>
    public void ReducirStock(int cantidad)
    {
        if (!TieneStockSuficiente(cantidad))
        {
            throw new DomainException($"Stock insuficiente para el producto '{Nombre}'. Disponible: {Stock}, Solicitado: {cantidad}");
        }

        Stock -= cantidad;
        FechaActualizacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Aumenta el stock del producto.
    /// </summary>
    /// <param name="cantidad">Cantidad a agregar.</param>
    public void AumentarStock(int cantidad)
    {
        if (cantidad <= 0)
        {
            throw new DomainException("La cantidad a agregar debe ser mayor a cero");
        }

        Stock += cantidad;
        FechaActualizacion = DateTime.UtcNow;
    }
}

