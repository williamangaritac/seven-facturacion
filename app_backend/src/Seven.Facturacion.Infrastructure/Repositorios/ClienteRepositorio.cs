// ============================================================================
// REPOSITORIO: ClienteRepositorio
// Descripción: Implementación del repositorio de clientes con EF Core
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Repositorios;

/// <summary>
/// Repositorio que implementa las operaciones de persistencia para Cliente.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="contexto">Contexto de base de datos.</param>
public class ClienteRepositorio(AppDbContext contexto) : IClienteRepositorio
{
    /// <inheritdoc />
    public async Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        return await contexto.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Clientes
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<Cliente?> ObtenerPorCorreoAsync(string correoElectronico, CancellationToken ct = default)
    {
        return await contexto.Clientes
            .FirstOrDefaultAsync(c => c.CorreoElectronico == correoElectronico, ct);
    }

    /// <inheritdoc />
    public async Task<Cliente?> ObtenerConFacturasAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Clientes
            .Include(c => c.Facturas)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<Cliente> AgregarAsync(Cliente cliente, CancellationToken ct = default)
    {
        await contexto.Clientes.AddAsync(cliente, ct);
        return cliente;
    }

    /// <inheritdoc />
    public void Actualizar(Cliente cliente)
    {
        contexto.Clientes.Update(cliente);
    }

    /// <inheritdoc />
    public void Eliminar(Cliente cliente)
    {
        contexto.Clientes.Remove(cliente);
    }

    /// <inheritdoc />
    public async Task<bool> ExisteAsync(int id, CancellationToken ct = default)
    {
        return await contexto.Clientes.AnyAsync(c => c.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Cliente>> ObtenerActivosAsync(CancellationToken ct = default)
    {
        return await contexto.Clientes
            .AsNoTracking()
            .Where(c => c.Activo)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Implementa la consulta SQL #3 de los requerimientos:
    /// Obtener todos los clientes menores de X años que hayan comprado en un rango de fechas.
    /// </remarks>
    public async Task<IEnumerable<Cliente>> ObtenerPorEdadYFechaCompraAsync(
        int edadMaxima,
        DateTime fechaDesde,
        DateTime fechaHasta,
        CancellationToken ct = default)
    {
        // Calcular la fecha de nacimiento mínima para la edad máxima
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var fechaNacimientoMinima = hoy.AddYears(-edadMaxima);

        return await contexto.Clientes
            .AsNoTracking()
            .Include(c => c.Facturas)
            .Where(c => 
                // Cliente menor de X años (nacido después de la fecha calculada)
                c.FechaNacimiento >= fechaNacimientoMinima &&
                // Tiene al menos una factura en el rango de fechas
                c.Facturas.Any(f => 
                    f.Fecha >= fechaDesde && 
                    f.Fecha <= fechaHasta &&
                    f.Estado != EstadoFactura.Anulada))
            .OrderBy(c => c.FechaNacimiento)
            .ToListAsync(ct);
    }
}

