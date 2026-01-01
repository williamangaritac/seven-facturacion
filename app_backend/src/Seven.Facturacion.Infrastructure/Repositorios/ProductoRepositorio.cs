// ============================================================================
// REPOSITORIO: ProductoRepositorio
// Descripción: Implementación del repositorio de productos con EF Core
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Repositorios;

/// <summary>
/// Repositorio que implementa las operaciones de persistencia para Producto.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="contexto">Contexto de base de datos.</param>
public class ProductoRepositorio(AppDbContext contexto) : IProductoRepositorio
{
    /// <inheritdoc />
    public async Task<IEnumerable<Producto>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        return await contexto.Productos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Productos
            .Include(p => p.DetallesFactura)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<Producto?> ObtenerPorCodigoAsync(string codigo, CancellationToken ct = default)
    {
        return await contexto.Productos
            .FirstOrDefaultAsync(p => p.Codigo == codigo, ct);
    }

    /// <inheritdoc />
    public async Task<bool> ExisteCodigoAsync(string codigo, CancellationToken ct = default)
    {
        return await contexto.Productos
            .AnyAsync(p => p.Codigo == codigo, ct);
    }

    /// <inheritdoc />
    public async Task<Producto> AgregarAsync(Producto producto, CancellationToken ct = default)
    {
        await contexto.Productos.AddAsync(producto, ct);
        return producto;
    }

    /// <inheritdoc />
    public void Actualizar(Producto producto)
    {
        contexto.Productos.Update(producto);
    }

    /// <inheritdoc />
    public void Eliminar(Producto producto)
    {
        contexto.Productos.Remove(producto);
    }

    /// <inheritdoc />
    public async Task<bool> ExisteAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Productos.AnyAsync(p => p.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Producto>> ObtenerActivosAsync(CancellationToken ct = default)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<bool> ActualizarStockAsync(int id, int cantidad, CancellationToken ct = default)
    {
        var producto = await contexto.Productos.FindAsync([id], ct);
        if (producto == null) return false;

        producto.Stock += cantidad;
        return true;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Implementa la consulta SQL #1 de los requerimientos:
    /// Obtener lista de precios de productos activos ordenada por nombre.
    /// </remarks>
    public async Task<IEnumerable<Producto>> ObtenerListaPreciosAsync(CancellationToken ct = default)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .Select(p => new Producto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            })
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Implementa la consulta SQL #2 de los requerimientos:
    /// Obtener productos con stock menor o igual al mínimo especificado.
    /// </remarks>
    public async Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(
        int stockMinimo,
        CancellationToken ct = default)
    {
        return await contexto.Productos
            .AsNoTracking()
            .Where(p => p.Activo && p.Stock <= stockMinimo)
            .OrderBy(p => p.Stock)
            .ThenBy(p => p.Nombre)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<(Producto Producto, int? AnioUltimaVenta)>> ObtenerConAnioUltimaVentaAsync(CancellationToken ct = default)
    {
        var productos = await contexto.Productos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync(ct);

        var productosConAnio = new List<(Producto, int?)>();

        foreach (var producto in productos)
        {
            // Obtener el año de la última venta del producto
            var ultimaVenta = await contexto.DetallesFactura
                .AsNoTracking()
                .Include(d => d.Factura)
                .Where(d => d.ProductoId == producto.Id && d.Factura.Estado != EstadoFactura.Anulada)
                .OrderByDescending(d => d.Factura.Fecha)
                .Select(d => d.Factura.Fecha.Year)
                .FirstOrDefaultAsync(ct);

            productosConAnio.Add((producto, ultimaVenta == 0 ? null : ultimaVenta));
        }

        return productosConAnio;
    }
}

