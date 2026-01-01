// ============================================================================
// SERVICIO: ProductoServicio
// Descripción: Implementación de lógica de negocio para productos
// ============================================================================

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Servicio que implementa los casos de uso para la gestión de productos.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="unidadDeTrabajo">Unidad de trabajo para acceso a repositorios.</param>
/// <param name="logger">Logger para registro de eventos.</param>
public class ProductoServicio(
    IUnidadDeTrabajo unidadDeTrabajo,
    ILogger<ProductoServicio> logger) : IProductoServicio
{
    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<ProductoDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Obteniendo todos los productos con año de última venta");

        var productosConAnio = await unidadDeTrabajo.Productos.ObtenerConAnioUltimaVentaAsync(ct);

        var productosDto = productosConAnio.Select(p => new ProductoDto(
            p.Producto.Id,
            p.Producto.Codigo,
            p.Producto.Nombre,
            p.Producto.Descripcion,
            p.Producto.Precio,
            p.Producto.Stock,
            p.Producto.Activo,
            p.Producto.TieneStockBajo,
            p.AnioUltimaVenta
        ));

        return Resultado<IEnumerable<ProductoDto>>.Exito(productosDto);
    }

    /// <inheritdoc />
    public async Task<Resultado<ProductoDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        logger.LogInformation("Buscando producto con ID: {ProductoId}", id);
        
        var producto = await unidadDeTrabajo.Productos.ObtenerPorIdAsync(id, ct);
        
        if (producto is null)
        {
            logger.LogWarning("Producto con ID {ProductoId} no encontrado", id);
            return Resultado<ProductoDto>.NoEncontrado($"Producto con ID {id} no encontrado");
        }

        return Resultado<ProductoDto>.Exito(producto.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<ProductoDto>> CrearAsync(CrearProductoDto dto, CancellationToken ct = default)
    {
        logger.LogInformation("Creando nuevo producto: {Codigo}", dto.Codigo);

        // Validar que el código no esté duplicado
        if (await unidadDeTrabajo.Productos.ExisteCodigoAsync(dto.Codigo, ct))
        {
            logger.LogWarning("El código {Codigo} ya está registrado", dto.Codigo);
            return Resultado<ProductoDto>.Conflicto($"El código '{dto.Codigo}' ya está registrado");
        }

        var producto = dto.ToEntity();
        await unidadDeTrabajo.Productos.AgregarAsync(producto, ct);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Producto creado exitosamente con ID: {ProductoId}", producto.Id);
        return Resultado<ProductoDto>.Creado(producto.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<ProductoDto>> ActualizarAsync(int id, ActualizarProductoDto dto, CancellationToken ct = default)
    {
        logger.LogInformation("Actualizando producto con ID: {ProductoId}", id);

        var producto = await unidadDeTrabajo.Productos.ObtenerPorIdAsync(id, ct);
        if (producto is null)
        {
            logger.LogWarning("Producto con ID {ProductoId} no encontrado para actualizar", id);
            return Resultado<ProductoDto>.NoEncontrado($"Producto con ID {id} no encontrado");
        }

        // Validar código duplicado (excepto el mismo producto)
        var productoConCodigo = await unidadDeTrabajo.Productos.ObtenerPorCodigoAsync(dto.Codigo, ct);
        if (productoConCodigo is not null && productoConCodigo.Id != id)
        {
            logger.LogWarning("El código {Codigo} ya está en uso por otro producto", dto.Codigo);
            return Resultado<ProductoDto>.Conflicto($"El código '{dto.Codigo}' ya está en uso");
        }

        producto.ActualizarDesde(dto);
        unidadDeTrabajo.Productos.Actualizar(producto);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Producto con ID {ProductoId} actualizado exitosamente", id);
        return Resultado<ProductoDto>.Exito(producto.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<bool>> EliminarAsync(int id, CancellationToken ct = default)
    {
        logger.LogInformation("Eliminando producto con ID: {ProductoId}", id);

        var producto = await unidadDeTrabajo.Productos.ObtenerPorIdAsync(id, ct);
        if (producto is null)
        {
            logger.LogWarning("Producto con ID {ProductoId} no encontrado para eliminar", id);
            return Resultado<bool>.NoEncontrado($"Producto con ID {id} no encontrado");
        }

        // Verificar si tiene detalles de factura asociados
        if (producto.DetallesFactura.Count > 0)
        {
            logger.LogWarning("No se puede eliminar producto {ProductoId} porque tiene ventas asociadas", id);
            return Resultado<bool>.Falla("No se puede eliminar el producto porque tiene ventas asociadas");
        }

        unidadDeTrabajo.Productos.Eliminar(producto);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Producto con ID {ProductoId} eliminado exitosamente", id);
        return Resultado<bool>.Exito(true);
    }

    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<ListaPrecioDto>>> ObtenerListaPreciosAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Obteniendo lista de precios");

        var productos = await unidadDeTrabajo.Productos.ObtenerListaPreciosAsync(ct);
        var listaPrecios = productos.Select(p => p.ToListaPrecioDto());

        logger.LogInformation("Lista de precios generada con {Cantidad} productos", listaPrecios.Count());
        return Resultado<IEnumerable<ListaPrecioDto>>.Exito(listaPrecios);
    }

    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<ProductoBajoStockDto>>> ObtenerConStockBajoAsync(
        int stockMinimo = 5, 
        CancellationToken ct = default)
    {
        logger.LogInformation("Buscando productos con stock <= {StockMinimo}", stockMinimo);

        var productos = await unidadDeTrabajo.Productos.ObtenerConStockBajoAsync(stockMinimo, ct);
        var productosDto = productos.Select(p => p.ToBajoStockDto());

        logger.LogInformation("Se encontraron {Cantidad} productos con stock bajo", productosDto.Count());
        return Resultado<IEnumerable<ProductoBajoStockDto>>.Exito(productosDto);
    }
}

