// ============================================================================
// EXTENSIONES: Mapeo entre Entidades y DTOs
// Descripción: Extension methods para convertir entidades a DTOs y viceversa
// Nota: Implementación manual sin AutoMapper según requerimientos
// ============================================================================

namespace Seven.Facturacion.Application.Extensiones;

/// <summary>
/// Extensiones de mapeo para conversión entre entidades de dominio y DTOs.
/// </summary>
public static class ExtensionesMapeo
{
    // ========================================================================
    // MAPEO DE CLIENTE
    // ========================================================================

    /// <summary>
    /// Convierte una entidad Cliente a ClienteDto.
    /// </summary>
    public static ClienteDto ToDto(this Cliente cliente) => new(
        Id: cliente.Id,
        Nombre: cliente.Nombre,
        Apellido: cliente.Apellido,
        NombreCompleto: cliente.NombreCompleto,
        CorreoElectronico: cliente.CorreoElectronico,
        Telefono: cliente.Telefono,
        FechaNacimiento: cliente.FechaNacimiento,
        Edad: cliente.Edad,
        Direccion: cliente.Direccion,
        Activo: cliente.Activo
    );

    /// <summary>
    /// Convierte una colección de clientes a DTOs.
    /// </summary>
    public static IEnumerable<ClienteDto> ToDto(this IEnumerable<Cliente> clientes) =>
        clientes.Select(c => c.ToDto());

    /// <summary>
    /// Convierte un CrearClienteDto a entidad Cliente.
    /// </summary>
    public static Cliente ToEntity(this CrearClienteDto dto) => new()
    {
        Nombre = dto.Nombre,
        Apellido = dto.Apellido,
        CorreoElectronico = dto.CorreoElectronico,
        Telefono = dto.Telefono,
        FechaNacimiento = dto.FechaNacimiento,
        Direccion = dto.Direccion
    };

    /// <summary>
    /// Actualiza una entidad Cliente con datos de ActualizarClienteDto.
    /// </summary>
    public static void ActualizarDesde(this Cliente cliente, ActualizarClienteDto dto)
    {
        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.CorreoElectronico = dto.CorreoElectronico;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.Activo = dto.Activo;
        cliente.FechaActualizacion = DateTime.UtcNow;
    }

    // ========================================================================
    // MAPEO DE PRODUCTO
    // ========================================================================

    /// <summary>
    /// Convierte una entidad Producto a ProductoDto.
    /// </summary>
    /// <param name="producto">Producto a convertir.</param>
    /// <param name="anioUltimaVenta">Año de última venta (opcional).</param>
    public static ProductoDto ToDto(this Producto producto, int? anioUltimaVenta = null) => new(
        Id: producto.Id,
        Codigo: producto.Codigo,
        Nombre: producto.Nombre,
        Descripcion: producto.Descripcion,
        Precio: producto.Precio,
        Stock: producto.Stock,
        Activo: producto.Activo,
        TieneStockBajo: producto.TieneStockBajo,
        AnioUltimaVenta: anioUltimaVenta
    );

    /// <summary>
    /// Convierte una colección de productos a DTOs.
    /// </summary>
    public static IEnumerable<ProductoDto> ToDto(this IEnumerable<Producto> productos) =>
        productos.Select(p => p.ToDto());

    /// <summary>
    /// Convierte un Producto a ListaPrecioDto.
    /// </summary>
    public static ListaPrecioDto ToListaPrecioDto(this Producto producto) => new(
        Id: producto.Id,
        Codigo: producto.Codigo,
        Nombre: producto.Nombre,
        Precio: producto.Precio
    );

    /// <summary>
    /// Convierte un Producto a ProductoBajoStockDto.
    /// </summary>
    public static ProductoBajoStockDto ToBajoStockDto(this Producto producto) => new(
        Id: producto.Id,
        Codigo: producto.Codigo,
        Nombre: producto.Nombre,
        Stock: producto.Stock,
        NivelAlerta: ProductoBajoStockDto.CalcularNivelAlerta(producto.Stock)
    );

    /// <summary>
    /// Convierte un CrearProductoDto a entidad Producto.
    /// </summary>
    public static Producto ToEntity(this CrearProductoDto dto) => new()
    {
        Codigo = dto.Codigo,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        Precio = dto.Precio,
        Stock = dto.Stock
    };

    /// <summary>
    /// Actualiza una entidad Producto con datos de ActualizarProductoDto.
    /// </summary>
    public static void ActualizarDesde(this Producto producto, ActualizarProductoDto dto)
    {
        producto.Codigo = dto.Codigo;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.Activo = dto.Activo;
        producto.FechaActualizacion = DateTime.UtcNow;
    }

    // ========================================================================
    // MAPEO DE FACTURA
    // ========================================================================

    /// <summary>
    /// Convierte una entidad Factura a FacturaDto.
    /// </summary>
    public static FacturaDto ToDto(this Factura factura) => new(
        Id: factura.Id,
        NumeroFactura: factura.NumeroFactura,
        ClienteId: factura.ClienteId,
        NombreCliente: factura.Cliente?.NombreCompleto ?? string.Empty,
        Fecha: factura.Fecha,
        Subtotal: factura.Subtotal,
        Impuesto: factura.Impuesto,
        Total: factura.Total,
        Estado: factura.Estado,
        Detalles: factura.Detalles.Select(d => d.ToDto())
    );

    /// <summary>
    /// Convierte una colección de facturas a DTOs.
    /// </summary>
    public static IEnumerable<FacturaDto> ToDto(this IEnumerable<Factura> facturas) =>
        facturas.Select(f => f.ToDto());

    /// <summary>
    /// Convierte un DetalleFactura a DetalleFacturaDto.
    /// </summary>
    public static DetalleFacturaDto ToDto(this DetalleFactura detalle) => new(
        Id: detalle.Id,
        ProductoId: detalle.ProductoId,
        NombreProducto: detalle.Producto?.Nombre ?? string.Empty,
        CodigoProducto: detalle.Producto?.Codigo ?? string.Empty,
        Cantidad: detalle.Cantidad,
        PrecioUnitario: detalle.PrecioUnitario,
        Subtotal: detalle.Subtotal
    );
}

