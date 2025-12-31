// ============================================================================
// INTERFAZ: IFacturaServicio
// Descripción: Contrato de operaciones de negocio para facturas
// ============================================================================

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Interfaz que define los casos de uso para la gestión de facturas.
/// </summary>
public interface IFacturaServicio
{
    /// <summary>
    /// Obtiene todas las facturas con sus detalles.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de facturas.</returns>
    Task<Resultado<IEnumerable<FacturaDto>>> ObtenerTodosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene una factura por su identificador con detalles.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la factura encontrada o error si no existe.</returns>
    Task<Resultado<FacturaDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Crea una nueva factura con sus detalles.
    /// </summary>
    /// <param name="dto">Datos de la factura a crear.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la factura creada o error de validación.</returns>
    Task<Resultado<FacturaDto>> CrearAsync(CrearFacturaDto dto, CancellationToken ct = default);

    /// <summary>
    /// Actualiza una factura existente (cliente y detalles).
    /// Solo se pueden editar facturas en estado PENDIENTE.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="dto">Datos actualizados de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la factura actualizada o error.</returns>
    Task<Resultado<FacturaDto>> ActualizarAsync(int id, ActualizarFacturaDto dto, CancellationToken ct = default);

    /// <summary>
    /// Actualiza el estado de una factura existente.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="dto">Datos con el nuevo estado.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la factura actualizada o error.</returns>
    Task<Resultado<FacturaDto>> ActualizarEstadoAsync(int id, ActualizarEstadoFacturaDto dto, CancellationToken ct = default);

    /// <summary>
    /// Elimina (anula) una factura por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado indicando éxito o error.</returns>
    Task<Resultado<bool>> EliminarAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Obtiene el total vendido por producto en un año específico.
    /// Implementa la consulta SQL #4 de los requerimientos.
    /// </summary>
    /// <param name="anio">Año a consultar (ej: 2000).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el reporte de ventas por producto.</returns>
    Task<Resultado<IEnumerable<VentasPorProductoDto>>> ObtenerVentasPorProductoAsync(
        int anio, 
        CancellationToken ct = default);

    /// <summary>
    /// Calcula la estimación de próxima compra para un cliente.
    /// Implementa la consulta SQL #5 de los requerimientos.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la estimación o error si no hay suficientes datos.</returns>
    Task<Resultado<ProximaCompraClienteDto>> ObtenerProximaCompraClienteAsync(
        int clienteId, 
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene las facturas de un cliente específico.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de facturas del cliente.</returns>
    Task<Resultado<IEnumerable<FacturaDto>>> ObtenerPorClienteAsync(
        int clienteId, 
        CancellationToken ct = default);
}

