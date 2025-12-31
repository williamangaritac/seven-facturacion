// ============================================================================
// INTERFAZ: IProductoRepositorio
// Descripción: Contrato para operaciones específicas de productos
// ============================================================================

namespace Seven.Facturacion.Domain.Interfaces;

/// <summary>
/// Interfaz que define las operaciones específicas del repositorio de productos.
/// Extiende las operaciones CRUD básicas de IRepositorio.
/// </summary>
public interface IProductoRepositorio : IRepositorio<Producto>
{
    /// <summary>
    /// Obtiene productos con stock menor o igual al mínimo especificado.
    /// Implementa la consulta SQL #2 de los requerimientos.
    /// </summary>
    /// <param name="stockMinimo">Cantidad mínima de stock (por defecto 5).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de productos con stock bajo.</returns>
    Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int stockMinimo = 5, CancellationToken ct = default);

    /// <summary>
    /// Obtiene la lista de precios de productos activos ordenada por nombre.
    /// Implementa la consulta SQL #1 de los requerimientos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de productos con Id, Código, Nombre y Precio.</returns>
    Task<IEnumerable<Producto>> ObtenerListaPreciosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene un producto por su código único.
    /// </summary>
    /// <param name="codigo">Código del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>El producto encontrado o null si no existe.</returns>
    Task<Producto?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los productos activos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de productos activos.</returns>
    Task<IEnumerable<Producto>> ObtenerActivosAsync(CancellationToken ct = default);

    /// <summary>
    /// Verifica si existe un producto con el código especificado.
    /// </summary>
    /// <param name="codigo">Código a verificar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>True si existe el código.</returns>
    Task<bool> ExisteCodigoAsync(string codigo, CancellationToken ct = default);

    /// <summary>
    /// Actualiza el stock de un producto.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="cantidad">Cantidad a sumar (positivo) o restar (negativo).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>True si se actualizó correctamente.</returns>
    Task<bool> ActualizarStockAsync(int id, int cantidad, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los productos con el año de su última venta.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Colección de productos con año de última venta.</returns>
    Task<IEnumerable<(Producto Producto, int? AnioUltimaVenta)>> ObtenerConAnioUltimaVentaAsync(CancellationToken ct = default);
}

