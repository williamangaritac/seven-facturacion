// ============================================================================
// REPOSITORIO: FacturaRepositorio
// Descripción: Implementación del repositorio de facturas con EF Core
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Repositorios;

/// <summary>
/// Repositorio que implementa las operaciones de persistencia para Factura.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="contexto">Contexto de base de datos.</param>
public class FacturaRepositorio(AppDbContext contexto) : IFacturaRepositorio
{
    /// <inheritdoc />
    public async Task<IEnumerable<Factura>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        return await contexto.Facturas
            .AsNoTracking()
            .Include(f => f.Cliente)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Factura>> ObtenerTodosConDetallesAsync(CancellationToken ct = default)
    {
        return await contexto.Facturas
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Factura?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Facturas
            .Include(f => f.Cliente)
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<Factura?> ObtenerPorIdConDetallesAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Factura>> ObtenerPorClienteAsync(int clienteId, CancellationToken ct = default)
    {
        return await contexto.Facturas
            .AsNoTracking()
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(f => f.ClienteId == clienteId)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Factura> AgregarAsync(Factura factura, CancellationToken ct = default)
    {
        await contexto.Facturas.AddAsync(factura, ct);
        return factura;
    }

    /// <inheritdoc />
    public void Actualizar(Factura factura)
    {
        contexto.Facturas.Update(factura);
    }

    /// <inheritdoc />
    public void Eliminar(Factura factura)
    {
        contexto.Facturas.Remove(factura);
    }

    /// <inheritdoc />
    public void EliminarDetalle(DetalleFactura detalle)
    {
        contexto.DetallesFactura.Remove(detalle);
    }

    /// <inheritdoc />
    public async Task<bool> ExisteAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Facturas.AnyAsync(f => f.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<Factura?> ObtenerPorNumeroAsync(string numeroFactura, CancellationToken ct = default)
    {
        return await contexto.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Factura>> ObtenerPorRangoFechasAsync(
        DateTime fechaDesde,
        DateTime fechaHasta,
        CancellationToken ct = default)
    {
        return await contexto.Facturas
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(f => f.Fecha >= fechaDesde && f.Fecha <= fechaHasta)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Implementa la consulta SQL #4 de los requerimientos:
    /// Obtener el total vendido por producto en un año específico.
    /// </remarks>
    public async Task<IEnumerable<VentasPorProductoResultado>> ObtenerVentasPorProductoAsync(
        int anio,
        CancellationToken ct = default)
    {
        var detalles = await contexto.DetallesFactura
            .AsNoTracking()
            .Include(d => d.Factura)
            .Include(d => d.Producto)
            .Where(d =>
                d.Factura.Fecha.Year == anio &&
                d.Factura.Estado != EstadoFactura.Anulada)
            .ToListAsync(ct);

        return detalles
            .GroupBy(d => new
            {
                d.ProductoId,
                d.Producto.Codigo,
                d.Producto.Nombre
            })
            .Select(g => new VentasPorProductoResultado(
                g.Key.ProductoId,
                g.Key.Codigo,
                g.Key.Nombre,
                g.Sum(d => d.Cantidad),
                g.Sum(d => d.Cantidad * d.PrecioUnitario),
                g.Select(d => d.FacturaId).Distinct().Count()
            ))
            .OrderByDescending(r => r.MontoTotalVendido)
            .ToList();
    }

    /// <inheritdoc />
    /// <remarks>
    /// Implementa la consulta SQL #5 de los requerimientos:
    /// Calcular la estimación de próxima compra basada en historial.
    /// </remarks>
    public async Task<ProximaCompraResultado?> ObtenerProximaCompraClienteAsync(
        int clienteId,
        CancellationToken ct = default)
    {
        var facturas = await contexto.Facturas
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Where(f => f.ClienteId == clienteId && f.Estado != EstadoFactura.Anulada)
            .OrderBy(f => f.Fecha)
            .ToListAsync(ct);

        // Se necesitan al menos 2 compras para calcular promedio
        if (facturas.Count < 2)
            return null;

        var cliente = facturas.First().Cliente;
        var fechas = facturas.Select(f => f.Fecha).ToList();

        // Calcular diferencias entre compras consecutivas
        var diferencias = new List<double>();
        for (int i = 1; i < fechas.Count; i++)
        {
            diferencias.Add((fechas[i] - fechas[i - 1]).TotalDays);
        }

        var promedioDias = diferencias.Average();
        var ultimaCompra = fechas.Last();
        var proximaEstimada = ultimaCompra.AddDays(promedioDias);

        return new ProximaCompraResultado(
            clienteId,
            cliente?.NombreCompleto ?? string.Empty,
            facturas.Count,
            ultimaCompra,
            (int)Math.Round(promedioDias),
            proximaEstimada
        );
    }
}

