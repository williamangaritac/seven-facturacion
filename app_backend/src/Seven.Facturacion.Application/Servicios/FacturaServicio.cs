// ============================================================================
// SERVICIO: FacturaServicio
// Descripción: Implementación de lógica de negocio para facturas
// ============================================================================

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Servicio que implementa los casos de uso para la gestión de facturas.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="unidadDeTrabajo">Unidad de trabajo para acceso a repositorios.</param>
/// <param name="logger">Logger para registro de eventos.</param>
public class FacturaServicio(
    IUnidadDeTrabajo unidadDeTrabajo,
    ILogger<FacturaServicio> logger) : IFacturaServicio
{
    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<FacturaDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Obteniendo todas las facturas");
        
        var facturas = await unidadDeTrabajo.Facturas.ObtenerTodosConDetallesAsync(ct);
        return Resultado<IEnumerable<FacturaDto>>.Exito(facturas.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<FacturaDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        logger.LogInformation("Buscando factura con ID: {FacturaId}", id);
        
        var factura = await unidadDeTrabajo.Facturas.ObtenerPorIdConDetallesAsync(id, ct);
        
        if (factura is null)
        {
            logger.LogWarning("Factura con ID {FacturaId} no encontrada", id);
            return Resultado<FacturaDto>.NoEncontrado($"Factura con ID {id} no encontrada");
        }

        return Resultado<FacturaDto>.Exito(factura.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<FacturaDto>> CrearAsync(CrearFacturaDto dto, CancellationToken ct = default)
    {
        logger.LogInformation("Creando nueva factura para cliente: {ClienteId}", dto.ClienteId);

        // Validar que el cliente existe
        var cliente = await unidadDeTrabajo.Clientes.ObtenerPorIdAsync(dto.ClienteId, ct);
        if (cliente is null)
        {
            logger.LogWarning("Cliente con ID {ClienteId} no encontrado", dto.ClienteId);
            return Resultado<FacturaDto>.NoEncontrado($"Cliente con ID {dto.ClienteId} no encontrado");
        }

        // Crear factura
        var factura = new Factura
        {
            NumeroFactura = Factura.GenerarNumeroFactura(),
            ClienteId = dto.ClienteId,
            Fecha = DateTime.UtcNow,
            Detalles = []
        };

        // Procesar detalles y validar stock
        foreach (var detalleDto in dto.Detalles)
        {
            var producto = await unidadDeTrabajo.Productos.ObtenerPorIdAsync(detalleDto.ProductoId, ct);
            
            if (producto is null)
            {
                logger.LogWarning("Producto con ID {ProductoId} no encontrado", detalleDto.ProductoId);
                return Resultado<FacturaDto>.NoEncontrado($"Producto con ID {detalleDto.ProductoId} no encontrado");
            }

            if (!producto.TieneStockSuficiente(detalleDto.Cantidad))
            {
                logger.LogWarning("Stock insuficiente para producto {ProductoId}", detalleDto.ProductoId);
                return Resultado<FacturaDto>.Falla(
                    $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, Solicitado: {detalleDto.Cantidad}");
            }

            // Crear detalle
            var detalle = new DetalleFactura
            {
                FacturaId = factura.Id,
                ProductoId = detalleDto.ProductoId,
                Cantidad = detalleDto.Cantidad,
                PrecioUnitario = producto.Precio
            };

            factura.Detalles.Add(detalle);

            // Reducir stock
            producto.ReducirStock(detalleDto.Cantidad);
            unidadDeTrabajo.Productos.Actualizar(producto);
        }

        // Calcular totales
        factura.CalcularTotales();

        // Guardar factura
        await unidadDeTrabajo.Facturas.AgregarAsync(factura, ct);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Factura {NumeroFactura} creada exitosamente", factura.NumeroFactura);

        // Recargar con detalles completos
        var facturaCreada = await unidadDeTrabajo.Facturas.ObtenerPorIdConDetallesAsync(factura.Id, ct);
        return Resultado<FacturaDto>.Creado(facturaCreada!.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<FacturaDto>> ActualizarAsync(
        int id,
        ActualizarFacturaDto dto,
        CancellationToken ct = default)
    {
        logger.LogInformation("Actualizando factura {FacturaId}", id);

        // Obtener factura existente
        var factura = await unidadDeTrabajo.Facturas.ObtenerPorIdConDetallesAsync(id, ct);
        if (factura is null)
        {
            logger.LogWarning("Factura con ID {FacturaId} no encontrada", id);
            return Resultado<FacturaDto>.NoEncontrado($"Factura con ID {id} no encontrada");
        }

        // Solo se pueden editar facturas PENDIENTES
        if (factura.Estado != EstadoFactura.Pendiente)
        {
            logger.LogWarning("No se puede editar factura {FacturaId} en estado {Estado}", id, factura.Estado);
            return Resultado<FacturaDto>.Falla($"Solo se pueden editar facturas en estado PENDIENTE. Estado actual: {factura.Estado}");
        }

        // Validar que el cliente existe
        var cliente = await unidadDeTrabajo.Clientes.ObtenerPorIdAsync(dto.ClienteId, ct);
        if (cliente is null)
        {
            logger.LogWarning("Cliente con ID {ClienteId} no encontrado", dto.ClienteId);
            return Resultado<FacturaDto>.NoEncontrado($"Cliente con ID {dto.ClienteId} no encontrado");
        }

        // 1. Restaurar stock de todos los detalles actuales
        await RestaurarStockAsync(factura, ct);

        // 2. Eliminar detalles actuales
        foreach (var detalle in factura.Detalles.ToList())
        {
            unidadDeTrabajo.Facturas.EliminarDetalle(detalle);
        }
        factura.Detalles.Clear();

        // 3. Actualizar cliente
        factura.ClienteId = dto.ClienteId;

        // 4. Procesar nuevos detalles y validar stock
        foreach (var detalleDto in dto.Detalles)
        {
            var producto = await unidadDeTrabajo.Productos.ObtenerPorIdAsync(detalleDto.ProductoId, ct);

            if (producto is null)
            {
                logger.LogWarning("Producto con ID {ProductoId} no encontrado", detalleDto.ProductoId);
                return Resultado<FacturaDto>.NoEncontrado($"Producto con ID {detalleDto.ProductoId} no encontrado");
            }

            if (!producto.TieneStockSuficiente(detalleDto.Cantidad))
            {
                logger.LogWarning("Stock insuficiente para producto {ProductoId}", detalleDto.ProductoId);
                return Resultado<FacturaDto>.Falla(
                    $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, Solicitado: {detalleDto.Cantidad}");
            }

            // Crear nuevo detalle
            var detalle = new DetalleFactura
            {
                FacturaId = factura.Id,
                ProductoId = detalleDto.ProductoId,
                Cantidad = detalleDto.Cantidad,
                PrecioUnitario = producto.Precio
            };

            factura.Detalles.Add(detalle);

            // Reducir stock
            producto.ReducirStock(detalleDto.Cantidad);
            unidadDeTrabajo.Productos.Actualizar(producto);
        }

        // 5. Recalcular totales
        factura.CalcularTotales();
        factura.FechaActualizacion = DateTime.UtcNow;

        // 6. Guardar cambios
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Factura {FacturaId} actualizada exitosamente", id);

        // Recargar con detalles completos
        var facturaActualizada = await unidadDeTrabajo.Facturas.ObtenerPorIdConDetallesAsync(id, ct);
        return Resultado<FacturaDto>.Exito(facturaActualizada!.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<FacturaDto>> ActualizarEstadoAsync(
        int id, 
        ActualizarEstadoFacturaDto dto, 
        CancellationToken ct = default)
    {
        logger.LogInformation("Actualizando estado de factura {FacturaId} a {Estado}", id, dto.Estado);

        var factura = await unidadDeTrabajo.Facturas.ObtenerPorIdConDetallesAsync(id, ct);
        if (factura is null)
        {
            logger.LogWarning("Factura con ID {FacturaId} no encontrada", id);
            return Resultado<FacturaDto>.NoEncontrado($"Factura con ID {id} no encontrada");
        }

        // Aplicar cambio de estado según reglas de negocio
        try
        {
            switch (dto.Estado)
            {
                case EstadoFactura.Pagada:
                    factura.MarcarComoPagada();
                    break;
                case EstadoFactura.Anulada:
                    factura.Anular();
                    // Restaurar stock de productos
                    await RestaurarStockAsync(factura, ct);
                    break;
                default:
                    factura.Estado = dto.Estado;
                    factura.FechaActualizacion = DateTime.UtcNow;
                    break;
            }
        }
        catch (DomainException ex)
        {
            logger.LogWarning("Error al cambiar estado: {Error}", ex.Message);
            return Resultado<FacturaDto>.Falla(ex.Message);
        }

        // No es necesario llamar a Actualizar porque la entidad ya está siendo rastreada
        // unidadDeTrabajo.Facturas.Actualizar(factura);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Estado de factura {FacturaId} actualizado a {Estado}", id, dto.Estado);
        return Resultado<FacturaDto>.Exito(factura.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<bool>> EliminarAsync(int id, CancellationToken ct = default)
    {
        logger.LogInformation("Eliminando factura con ID: {FacturaId}", id);

        var factura = await unidadDeTrabajo.Facturas.ObtenerPorIdConDetallesAsync(id, ct);
        if (factura is null)
        {
            logger.LogWarning("Factura con ID {FacturaId} no encontrada para eliminar", id);
            return Resultado<bool>.NoEncontrado($"Factura con ID {id} no encontrada");
        }

        if (factura.Estado == EstadoFactura.Pagada)
        {
            logger.LogWarning("No se puede eliminar factura pagada {FacturaId}", id);
            return Resultado<bool>.Falla("No se puede eliminar una factura que ya fue pagada");
        }

        // Restaurar stock si no está anulada
        if (factura.Estado != EstadoFactura.Anulada)
        {
            await RestaurarStockAsync(factura, ct);
        }

        unidadDeTrabajo.Facturas.Eliminar(factura);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Factura con ID {FacturaId} eliminada exitosamente", id);
        return Resultado<bool>.Exito(true);
    }

    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<VentasPorProductoDto>>> ObtenerVentasPorProductoAsync(
        int anio,
        CancellationToken ct = default)
    {
        logger.LogInformation("Obteniendo ventas por producto del año {Anio}", anio);

        var ventas = await unidadDeTrabajo.Facturas.ObtenerVentasPorProductoAsync(anio, ct);

        var ventasDto = ventas.Select(v => new VentasPorProductoDto(
            ProductoId: v.ProductoId,
            CodigoProducto: v.CodigoProducto,
            NombreProducto: v.NombreProducto,
            CantidadTotalVendida: v.CantidadTotalVendida,
            MontoTotalVendido: v.MontoTotalVendido,
            NumeroFacturas: v.NumeroFacturas
        ));

        logger.LogInformation("Reporte generado con {Cantidad} productos", ventasDto.Count());
        return Resultado<IEnumerable<VentasPorProductoDto>>.Exito(ventasDto);
    }

    /// <inheritdoc />
    public async Task<Resultado<ProximaCompraClienteDto>> ObtenerProximaCompraClienteAsync(
        int clienteId,
        CancellationToken ct = default)
    {
        logger.LogInformation("Calculando próxima compra para cliente {ClienteId}", clienteId);

        // Verificar que el cliente existe
        var cliente = await unidadDeTrabajo.Clientes.ObtenerPorIdAsync(clienteId, ct);
        if (cliente is null)
        {
            return Resultado<ProximaCompraClienteDto>.NoEncontrado($"Cliente con ID {clienteId} no encontrado");
        }

        var resultado = await unidadDeTrabajo.Facturas.ObtenerProximaCompraClienteAsync(clienteId, ct);

        if (resultado is null)
        {
            logger.LogWarning("Cliente {ClienteId} no tiene suficientes compras para estimar", clienteId);
            return Resultado<ProximaCompraClienteDto>.Falla(
                "El cliente debe tener al menos 2 compras para estimar la próxima");
        }

        var dto = new ProximaCompraClienteDto(
            ClienteId: resultado.ClienteId,
            NombreCliente: resultado.NombreCliente,
            TotalCompras: resultado.TotalCompras,
            UltimaCompra: resultado.UltimaCompra,
            PromedioDiasEntreCompras: resultado.PromedioDiasEntreCompras,
            ProximaCompraEstimada: resultado.ProximaCompraEstimada,
            EstadoPrediccion: ProximaCompraClienteDto.CalcularEstadoPrediccion(resultado.ProximaCompraEstimada)
        );

        logger.LogInformation("Próxima compra estimada: {Fecha}", dto.ProximaCompraEstimada);
        return Resultado<ProximaCompraClienteDto>.Exito(dto);
    }

    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<FacturaDto>>> ObtenerPorClienteAsync(
        int clienteId,
        CancellationToken ct = default)
    {
        logger.LogInformation("Obteniendo facturas del cliente {ClienteId}", clienteId);

        var cliente = await unidadDeTrabajo.Clientes.ObtenerPorIdAsync(clienteId, ct);
        if (cliente is null)
        {
            return Resultado<IEnumerable<FacturaDto>>.NoEncontrado($"Cliente con ID {clienteId} no encontrado");
        }

        var facturas = await unidadDeTrabajo.Facturas.ObtenerPorClienteAsync(clienteId, ct);
        return Resultado<IEnumerable<FacturaDto>>.Exito(facturas.ToDto());
    }

    // ========================================================================
    // MÉTODOS PRIVADOS
    // ========================================================================

    /// <summary>
    /// Restaura el stock de productos cuando se anula o elimina una factura.
    /// </summary>
    private async Task RestaurarStockAsync(Factura factura, CancellationToken ct)
    {
        foreach (var detalle in factura.Detalles)
        {
            var producto = await unidadDeTrabajo.Productos.ObtenerPorIdAsync(detalle.ProductoId, ct);
            if (producto is not null)
            {
                producto.AumentarStock(detalle.Cantidad);
                unidadDeTrabajo.Productos.Actualizar(producto);
            }
        }
    }
}
