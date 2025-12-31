// ============================================================================
// INTERFAZ: IUnidadDeTrabajo (Unit of Work)
// Descripción: Coordina transacciones entre múltiples repositorios
// ============================================================================

namespace Seven.Facturacion.Domain.Interfaces;

/// <summary>
/// Interfaz que implementa el patrón Unit of Work.
/// Coordina las operaciones de múltiples repositorios en una transacción única.
/// </summary>
public interface IUnidadDeTrabajo : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Repositorio de clientes.
    /// </summary>
    IClienteRepositorio Clientes { get; }

    /// <summary>
    /// Repositorio de productos.
    /// </summary>
    IProductoRepositorio Productos { get; }

    /// <summary>
    /// Repositorio de facturas.
    /// </summary>
    IFacturaRepositorio Facturas { get; }

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Número de entidades afectadas.</returns>
    Task<int> GuardarCambiosAsync(CancellationToken ct = default);

    /// <summary>
    /// Inicia una transacción explícita.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task IniciarTransaccionAsync(CancellationToken ct = default);

    /// <summary>
    /// Confirma la transacción actual.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task ConfirmarTransaccionAsync(CancellationToken ct = default);

    /// <summary>
    /// Revierte la transacción actual.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task RevertirTransaccionAsync(CancellationToken ct = default);
}

