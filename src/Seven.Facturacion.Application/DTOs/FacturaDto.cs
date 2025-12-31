// ============================================================================
// DTOs: Factura y DetalleFactura
// Descripción: Records inmutables para transferencia de datos de facturas
// ============================================================================

namespace Seven.Facturacion.Application.DTOs;

/// <summary>
/// DTO de lectura para factura con información completa.
/// </summary>
/// <param name="Id">Identificador único.</param>
/// <param name="NumeroFactura">Número único de factura.</param>
/// <param name="ClienteId">Identificador del cliente.</param>
/// <param name="NombreCliente">Nombre completo del cliente.</param>
/// <param name="Fecha">Fecha de emisión.</param>
/// <param name="Subtotal">Subtotal sin impuestos.</param>
/// <param name="Impuesto">Valor del IVA (19%).</param>
/// <param name="Total">Total de la factura.</param>
/// <param name="Estado">Estado: PENDIENTE, PAGADA, ANULADA.</param>
/// <param name="Detalles">Líneas de detalle de la factura.</param>
public record FacturaDto(
    int Id,
    string NumeroFactura,
    int ClienteId,
    string NombreCliente,
    DateTime Fecha,
    decimal Subtotal,
    decimal Impuesto,
    decimal Total,
    string Estado,
    IEnumerable<DetalleFacturaDto> Detalles
);

/// <summary>
/// DTO de lectura para detalle de factura.
/// </summary>
/// <param name="Id">Identificador del detalle.</param>
/// <param name="ProductoId">Identificador del producto.</param>
/// <param name="NombreProducto">Nombre del producto.</param>
/// <param name="CodigoProducto">Código del producto.</param>
/// <param name="Cantidad">Cantidad facturada.</param>
/// <param name="PrecioUnitario">Precio unitario al momento de la venta.</param>
/// <param name="Subtotal">Subtotal del detalle.</param>
public record DetalleFacturaDto(
    int Id,
    int ProductoId,
    string NombreProducto,
    string CodigoProducto,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal
);

/// <summary>
/// DTO para crear una nueva factura.
/// </summary>
public record CrearFacturaDto
{
    [Required(ErrorMessage = "El cliente es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El cliente es inválido")]
    public int ClienteId { get; init; }

    [Required(ErrorMessage = "Debe incluir al menos un detalle")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un detalle")]
    public IEnumerable<CrearDetalleFacturaDto> Detalles { get; init; } = [];
}

/// <summary>
/// DTO para crear un detalle de factura.
/// </summary>
public record CrearDetalleFacturaDto
{
    [Required(ErrorMessage = "El producto es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El producto es inválido")]
    public int ProductoId { get; init; }

    [Required(ErrorMessage = "La cantidad es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
    public int Cantidad { get; init; }
}

/// <summary>
/// DTO para actualizar estado de factura.
/// </summary>
/// <param name="Estado">Nuevo estado: PENDIENTE, PAGADA, ANULADA.</param>
public record ActualizarEstadoFacturaDto
{
    [Required(ErrorMessage = "El estado es requerido")]
    [RegularExpression("^(PENDIENTE|PAGADA|ANULADA)$", ErrorMessage = "Estado inválido")]
    public string Estado { get; init; } = string.Empty;
}

/// <summary>
/// DTO para actualizar una factura existente.
/// Solo se pueden editar facturas en estado PENDIENTE.
/// </summary>
public record ActualizarFacturaDto
{
    [Required(ErrorMessage = "El cliente es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El cliente es inválido")]
    public int ClienteId { get; init; }

    [Required(ErrorMessage = "Debe incluir al menos un detalle")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un detalle")]
    public IEnumerable<ActualizarDetalleFacturaDto> Detalles { get; init; } = [];
}

/// <summary>
/// DTO para actualizar un detalle de factura.
/// </summary>
public record ActualizarDetalleFacturaDto
{
    /// <summary>
    /// ID del detalle existente (null para nuevos detalles).
    /// </summary>
    public int? Id { get; init; }

    [Required(ErrorMessage = "El producto es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El producto es inválido")]
    public int ProductoId { get; init; }

    [Required(ErrorMessage = "La cantidad es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
    public int Cantidad { get; init; }
}

/// <summary>
/// DTO para reporte de ventas por producto (Consulta SQL #4).
/// </summary>
/// <param name="ProductoId">Identificador del producto.</param>
/// <param name="CodigoProducto">Código del producto.</param>
/// <param name="NombreProducto">Nombre del producto.</param>
/// <param name="CantidadTotalVendida">Unidades vendidas.</param>
/// <param name="MontoTotalVendido">Monto total de ventas.</param>
/// <param name="NumeroFacturas">Cantidad de facturas.</param>
public record VentasPorProductoDto(
    int ProductoId,
    string CodigoProducto,
    string NombreProducto,
    int CantidadTotalVendida,
    decimal MontoTotalVendido,
    int NumeroFacturas
);

/// <summary>
/// DTO para estimación de próxima compra (Consulta SQL #5).
/// </summary>
/// <param name="ClienteId">Identificador del cliente.</param>
/// <param name="NombreCliente">Nombre completo del cliente.</param>
/// <param name="TotalCompras">Número total de compras.</param>
/// <param name="UltimaCompra">Fecha de última compra.</param>
/// <param name="PromedioDiasEntreCompras">Promedio de días entre compras.</param>
/// <param name="ProximaCompraEstimada">Fecha estimada de próxima compra.</param>
/// <param name="EstadoPrediccion">Estado: VENCIDA, PRÓXIMA, FUTURA.</param>
public record ProximaCompraClienteDto(
    int ClienteId,
    string NombreCliente,
    int TotalCompras,
    DateTime UltimaCompra,
    int PromedioDiasEntreCompras,
    DateTime ProximaCompraEstimada,
    string EstadoPrediccion
)
{
    /// <summary>
    /// Calcula el estado de predicción basándose en la fecha estimada.
    /// </summary>
    public static string CalcularEstadoPrediccion(DateTime proximaCompra)
    {
        var hoy = DateTime.Today;
        return proximaCompra switch
        {
            var f when f < hoy => "VENCIDA",
            var f when f <= hoy.AddDays(7) => "PRÓXIMA",
            _ => "FUTURA"
        };
    }
}

