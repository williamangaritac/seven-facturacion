// ============================================================================
// INTERFAZ: IProductoServicio
// Descripción: Contrato de operaciones de negocio para productos
// ============================================================================

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Interfaz que define los casos de uso para la gestión de productos.
/// </summary>
public interface IProductoServicio
{
    /// <summary>
    /// Obtiene todos los productos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de productos.</returns>
    Task<Resultado<IEnumerable<ProductoDto>>> ObtenerTodosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el producto encontrado o error si no existe.</returns>
    Task<Resultado<ProductoDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    /// <param name="dto">Datos del producto a crear.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el producto creado o error de validación.</returns>
    Task<Resultado<ProductoDto>> CrearAsync(CrearProductoDto dto, CancellationToken ct = default);

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="dto">Datos actualizados del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con el producto actualizado o error.</returns>
    Task<Resultado<ProductoDto>> ActualizarAsync(int id, ActualizarProductoDto dto, CancellationToken ct = default);

    /// <summary>
    /// Elimina un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado indicando éxito o error.</returns>
    Task<Resultado<bool>> EliminarAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Obtiene la lista de precios de productos activos ordenada por nombre.
    /// Implementa la consulta SQL #1 de los requerimientos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de precios.</returns>
    Task<Resultado<IEnumerable<ListaPrecioDto>>> ObtenerListaPreciosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene productos con stock bajo.
    /// Implementa la consulta SQL #2 de los requerimientos.
    /// </summary>
    /// <param name="stockMinimo">Cantidad mínima de stock (por defecto 5).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Resultado con la lista de productos con stock bajo.</returns>
    Task<Resultado<IEnumerable<ProductoBajoStockDto>>> ObtenerConStockBajoAsync(
        int stockMinimo = 5, 
        CancellationToken ct = default);
}

