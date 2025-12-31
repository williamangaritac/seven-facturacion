// ============================================================================
// INTERFAZ: IClienteRepositorio
// Descripción: Contrato para operaciones específicas de clientes
// ============================================================================

namespace Seven.Facturacion.Domain.Interfaces;

/// <summary>
/// Interfaz que define las operaciones específicas del repositorio de clientes.
/// Extiende las operaciones CRUD básicas de IRepositorio.
/// </summary>
public interface IClienteRepositorio : IRepositorio<Cliente>
{
    /// <summary>
    /// Obtiene clientes por edad máxima que hayan realizado compras en un rango de fechas.
    /// Implementa la consulta SQL #3 de los requerimientos.
    /// </summary>
    /// <param name="edadMaxima">Edad máxima de los clientes (ej: 35 años).</param>
    /// <param name="fechaDesde">Fecha inicial del rango de compras.</param>
    /// <param name="fechaHasta">Fecha final del rango de compras.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de clientes que cumplen los criterios.</returns>
    Task<IEnumerable<Cliente>> ObtenerPorEdadYFechaCompraAsync(
        int edadMaxima, 
        DateTime fechaDesde, 
        DateTime fechaHasta, 
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene un cliente por su correo electrónico.
    /// </summary>
    /// <param name="correoElectronico">Correo electrónico a buscar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>El cliente encontrado o null si no existe.</returns>
    Task<Cliente?> ObtenerPorCorreoAsync(string correoElectronico, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los clientes activos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de clientes activos.</returns>
    Task<IEnumerable<Cliente>> ObtenerActivosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene un cliente con sus facturas cargadas.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>El cliente con sus facturas o null si no existe.</returns>
    Task<Cliente?> ObtenerConFacturasAsync(int id, CancellationToken ct = default);
}

