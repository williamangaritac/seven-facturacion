// ============================================================================
// ENTIDAD: DetalleFactura
// Descripción: Representa una línea de detalle de una factura
// ============================================================================

namespace Seven.Facturacion.Domain.Entidades;

/// <summary>
/// Entidad que representa una línea de detalle en una factura.
/// </summary>
public class DetalleFactura
{
    /// <summary>
    /// Identificador único del detalle.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador de la factura a la que pertenece.
    /// </summary>
    [Required]
    public required int FacturaId { get; set; }

    /// <summary>
    /// Identificador del producto facturado.
    /// </summary>
    [Required]
    public required int ProductoId { get; set; }

    /// <summary>
    /// Cantidad de unidades del producto.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
    public required int Cantidad { get; set; }

    /// <summary>
    /// Precio unitario al momento de la venta (histórico).
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
    public required decimal PrecioUnitario { get; set; }

    // ========================================================================
    // PROPIEDADES DE NAVEGACIÓN
    // ========================================================================

    /// <summary>
    /// Factura a la que pertenece este detalle.
    /// </summary>
    public Factura Factura { get; set; } = null!;

    /// <summary>
    /// Producto facturado en este detalle.
    /// </summary>
    public Producto Producto { get; set; } = null!;

    // ========================================================================
    // PROPIEDADES CALCULADAS
    // ========================================================================

    /// <summary>
    /// Subtotal del detalle (Cantidad × PrecioUnitario).
    /// </summary>
    public decimal Subtotal => Cantidad * PrecioUnitario;

    // ========================================================================
    // MÉTODOS DE DOMINIO
    // ========================================================================

    /// <summary>
    /// Actualiza la cantidad del detalle.
    /// </summary>
    /// <param name="nuevaCantidad">Nueva cantidad.</param>
    /// <exception cref="DomainException">Si la cantidad no es válida.</exception>
    public void ActualizarCantidad(int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
        {
            throw new DomainException("La cantidad debe ser mayor a cero");
        }

        Cantidad = nuevaCantidad;
    }
}

