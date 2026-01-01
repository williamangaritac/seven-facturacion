// ============================================================================
// INTERFAZ: IFacturaRepositorio
// Descripción: Contrato para operaciones específicas de facturas
// ============================================================================

namespace Seven.Facturacion.Domain.Interfaces;

/// <summary>
/// Interfaz que define las operaciones específicas del repositorio de facturas.
/// Extiende las operaciones CRUD básicas de IRepositorio.
/// </summary>
public interface IFacturaRepositorio : IRepositorio<Factura>
{
    /// <summary>
    /// Obtiene una factura por su ID incluyendo cliente y detalles con productos.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La factura con todos sus detalles o null si no existe.</returns>
    Task<Factura?> ObtenerPorIdConDetallesAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todas las facturas incluyendo cliente y detalles.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de facturas con detalles.</returns>
    Task<IEnumerable<Factura>> ObtenerTodosConDetallesAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene una factura por su número único.
    /// </summary>
    /// <param name="numeroFactura">Número de factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La factura encontrada o null si no existe.</returns>
    Task<Factura?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todas las facturas de un cliente.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de facturas del cliente.</returns>
    Task<IEnumerable<Factura>> ObtenerPorClienteAsync(int clienteId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene las facturas en un rango de fechas.
    /// </summary>
    /// <param name="fechaDesde">Fecha inicial.</param>
    /// <param name="fechaHasta">Fecha final.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de facturas en el rango.</returns>
    Task<IEnumerable<Factura>> ObtenerPorRangoFechasAsync(
        DateTime fechaDesde, 
        DateTime fechaHasta, 
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene el total vendido por producto en un año específico.
    /// Implementa la consulta SQL #4 de los requerimientos.
    /// </summary>
    /// <param name="anio">Año a consultar (ej: 2000).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección con ProductoId, Nombre, TotalVendido, CantidadVendida.</returns>
    Task<IEnumerable<VentasPorProductoResultado>> ObtenerVentasPorProductoAsync(
        int anio, 
        CancellationToken ct = default);

    /// <summary>
    /// Calcula la estimación de próxima compra para un cliente.
    /// Implementa la consulta SQL #5 de los requerimientos.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Datos de estimación o null si no hay suficientes compras.</returns>
    Task<ProximaCompraResultado?> ObtenerProximaCompraClienteAsync(
        int clienteId,
        CancellationToken ct = default);

    /// <summary>
    /// Elimina un detalle de factura.
    /// </summary>
    /// <param name="detalle">Detalle a eliminar.</param>
    void EliminarDetalle(DetalleFactura detalle);
}

// ============================================================================
// RECORDS DE RESULTADO PARA CONSULTAS ESPECÍFICAS
// ============================================================================

/// <summary>
/// Resultado de la consulta de ventas por producto.
/// </summary>
/// <param name="ProductoId">Identificador del producto.</param>
/// <param name="CodigoProducto">Código del producto.</param>
/// <param name="NombreProducto">Nombre del producto.</param>
/// <param name="CantidadTotalVendida">Unidades vendidas.</param>
/// <param name="MontoTotalVendido">Monto total de ventas.</param>
/// <param name="NumeroFacturas">Cantidad de facturas.</param>
public record VentasPorProductoResultado(
    int ProductoId,
    string CodigoProducto,
    string NombreProducto,
    int CantidadTotalVendida,
    decimal MontoTotalVendido,
    int NumeroFacturas
);

/// <summary>
/// Resultado de la consulta de próxima compra estimada.
/// </summary>
/// <param name="ClienteId">Identificador del cliente.</param>
/// <param name="NombreCliente">Nombre completo del cliente.</param>
/// <param name="TotalCompras">Número total de compras.</param>
/// <param name="UltimaCompra">Fecha de la última compra.</param>
/// <param name="PromedioDiasEntreCompras">Promedio de días entre compras.</param>
/// <param name="ProximaCompraEstimada">Fecha estimada de próxima compra.</param>
public record ProximaCompraResultado(
    int ClienteId,
    string NombreCliente,
    int TotalCompras,
    DateTime UltimaCompra,
    int PromedioDiasEntreCompras,
    DateTime ProximaCompraEstimada
);

