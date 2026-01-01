// ============================================================================
// INTERFAZ: IClienteServicio
// Descripción: Contrato de operaciones de negocio para clientes
// ============================================================================

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Interfaz que define los casos de uso para la gestión de clientes.
/// </summary>
public interface IClienteServicio
{
    /// <summary>
    /// Obtiene todos los clientes.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de clientes.</returns>
    Task<Resultado<IEnumerable<ClienteDto>>> ObtenerTodosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene un cliente por su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el cliente encontrado o error si no existe.</returns>
    Task<Resultado<ClienteDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    /// <param name="dto">Datos del cliente a crear.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el cliente creado o error de validación.</returns>
    Task<Resultado<ClienteDto>> CrearAsync(CrearClienteDto dto, CancellationToken ct = default);

    /// <summary>
    /// Actualiza un cliente existente.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="dto">Datos actualizados del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el cliente actualizado o error.</returns>
    Task<Resultado<ClienteDto>> ActualizarAsync(int id, ActualizarClienteDto dto, CancellationToken ct = default);

    /// <summary>
    /// Elimina un cliente por su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado indicando éxito o error.</returns>
    Task<Resultado<bool>> EliminarAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Obtiene clientes por edad máxima que hayan comprado en un rango de fechas.
    /// Implementa la consulta SQL #3 de los requerimientos.
    /// </summary>
    /// <param name="edadMaxima">Edad máxima de los clientes (ej: 35).</param>
    /// <param name="fechaDesde">Fecha inicial del rango de compras.</param>
    /// <param name="fechaHasta">Fecha final del rango de compras.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de clientes que cumplen los criterios.</returns>
    Task<Resultado<IEnumerable<ClientePorEdadYCompraDto>>> ObtenerPorEdadYFechaCompraAsync(
        int edadMaxima,
        DateTime fechaDesde,
        DateTime fechaHasta,
        CancellationToken ct = default);
}

