// ============================================================================
// INTERFAZ: IRepositorio<T>
// Descripción: Contrato genérico CRUD para repositorios
// ============================================================================

namespace Seven.Facturacion.Domain.Interfaces;

/// <summary>
/// Interfaz genérica que define las operaciones CRUD básicas para repositorios.
/// </summary>
/// <typeparam name="T">Tipo de entidad del dominio.</typeparam>
public interface IRepositorio<T> where T : class
{
    /// <summary>
    /// Obtiene una entidad por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la entidad.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La entidad encontrada o null si no existe.</returns>
    Task<T?> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todas las entidades del repositorio.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de todas las entidades.</returns>
    Task<IEnumerable<T>> ObtenerTodosAsync(CancellationToken ct = default);

    /// <summary>
    /// Agrega una nueva entidad al repositorio.
    /// </summary>
    /// <param name="entidad">Entidad a agregar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La entidad agregada con su ID asignado.</returns>
    Task<T> AgregarAsync(T entidad, CancellationToken ct = default);

    /// <summary>
    /// Marca una entidad para actualización.
    /// </summary>
    /// <param name="entidad">Entidad a actualizar.</param>
    void Actualizar(T entidad);

    /// <summary>
    /// Marca una entidad para eliminación.
    /// </summary>
    /// <param name="entidad">Entidad a eliminar.</param>
    void Eliminar(T entidad);

    /// <summary>
    /// Verifica si existe una entidad con el identificador especificado.
    /// </summary>
    /// <param name="id">Identificador a buscar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>True si existe la entidad.</returns>
    Task<bool> ExisteAsync(int id, CancellationToken ct = default);
}

