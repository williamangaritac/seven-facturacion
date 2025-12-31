// ============================================================================
// UNIDAD DE TRABAJO: UnidadDeTrabajo
// Descripción: Implementación del patrón Unit of Work para transacciones
// ============================================================================

namespace Seven.Facturacion.Infrastructure.Repositorios;

/// <summary>
/// Implementación del patrón Unit of Work que coordina repositorios y transacciones.
/// Utiliza primary constructor de C# 14 e inicialización lazy de repositorios.
/// </summary>
/// <param name="contexto">Contexto de base de datos.</param>
public class UnidadDeTrabajo(AppDbContext contexto) : IUnidadDeTrabajo
{
    // ========================================================================
    // CAMPOS PRIVADOS - Lazy initialization
    // ========================================================================
    
    private IClienteRepositorio? _clientes;
    private IProductoRepositorio? _productos;
    private IFacturaRepositorio? _facturas;
    private bool _disposed;

    // ========================================================================
    // REPOSITORIOS (Lazy Initialization)
    // ========================================================================

    /// <inheritdoc />
    public IClienteRepositorio Clientes => 
        _clientes ??= new ClienteRepositorio(contexto);

    /// <inheritdoc />
    public IProductoRepositorio Productos => 
        _productos ??= new ProductoRepositorio(contexto);

    /// <inheritdoc />
    public IFacturaRepositorio Facturas => 
        _facturas ??= new FacturaRepositorio(contexto);

    // ========================================================================
    // MÉTODOS DE PERSISTENCIA
    // ========================================================================

    /// <inheritdoc />
    public async Task<int> GuardarCambiosAsync(CancellationToken ct = default)
    {
        return await contexto.SaveChangesAsync(ct);
    }

    /// <inheritdoc />
    public int GuardarCambios()
    {
        return contexto.SaveChanges();
    }

    // ========================================================================
    // TRANSACCIONES
    // ========================================================================

    /// <inheritdoc />
    public async Task IniciarTransaccionAsync(CancellationToken ct = default)
    {
        await contexto.Database.BeginTransactionAsync(ct);
    }

    /// <inheritdoc />
    public async Task ConfirmarTransaccionAsync(CancellationToken ct = default)
    {
        await contexto.Database.CommitTransactionAsync(ct);
    }

    /// <inheritdoc />
    public async Task RevertirTransaccionAsync(CancellationToken ct = default)
    {
        await contexto.Database.RollbackTransactionAsync(ct);
    }

    // ========================================================================
    // DISPOSE PATTERN
    // ========================================================================

    /// <summary>
    /// Libera los recursos del contexto.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Libera los recursos de forma asíncrona.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Implementación protegida del patrón Dispose.
    /// </summary>
    /// <param name="disposing">Indica si se está liberando desde Dispose().</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                contexto.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Implementación del dispose asíncrono.
    /// </summary>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (!_disposed)
        {
            await contexto.DisposeAsync();
            _disposed = true;
        }
    }
}

