// ============================================================================
// CONTROLADOR: ProductosController
// Descripción: API REST para gestión de productos
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Seven.Facturacion.Application.Comun;
using Seven.Facturacion.Application.DTOs;
using Seven.Facturacion.Application.Servicios;

namespace Seven.Facturacion.Api.Controladores;

/// <summary>
/// Controlador para operaciones CRUD de productos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController(IProductoServicio productoServicio) : ControllerBase
{
    /// <summary>
    /// Obtiene todos los productos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de productos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct)
    {
        var resultado = await productoServicio.ObtenerTodosAsync(ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene un producto por su ID.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Producto encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var resultado = await productoServicio.ObtenerPorIdAsync(id, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    /// <param name="dto">Datos del producto a crear.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Producto creado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear([FromBody] CrearProductoDto dto, CancellationToken ct)
    {
        var resultado = await productoServicio.CrearAsync(dto, ct);
        
        if (resultado.EsExitoso && resultado.Valor is not null)
        {
            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = resultado.Valor.Id },
                resultado.Valor);
        }

        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="dto">Datos actualizados del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Producto actualizado.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoDto dto, CancellationToken ct)
    {
        var resultado = await productoServicio.ActualizarAsync(id, dto, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Elimina un producto.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>NoContent si se eliminó correctamente.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        var resultado = await productoServicio.EliminarAsync(id, ct);
        
        if (resultado.EsExitoso)
            return NoContent();

        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene la lista de precios de productos activos.
    /// Implementa la consulta SQL #1 de los requerimientos.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de precios ordenada por nombre.</returns>
    [HttpGet("lista-precios")]
    [ProducesResponseType(typeof(IEnumerable<ListaPrecioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerListaPrecios(CancellationToken ct)
    {
        var resultado = await productoServicio.ObtenerListaPreciosAsync(ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene productos con stock bajo.
    /// Implementa la consulta SQL #2 de los requerimientos.
    /// </summary>
    /// <param name="stockMinimo">Cantidad mínima de stock (por defecto 5).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de productos con stock bajo.</returns>
    [HttpGet("stock-bajo")]
    [ProducesResponseType(typeof(IEnumerable<ProductoBajoStockDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerConStockBajo(
        [FromQuery] int stockMinimo = 5,
        CancellationToken ct = default)
    {
        var resultado = await productoServicio.ObtenerConStockBajoAsync(stockMinimo, ct);
        return ProcesarResultado(resultado);
    }

    // ========================================================================
    // MÉTODOS AUXILIARES
    // ========================================================================

    private IActionResult ProcesarResultado<T>(Resultado<T> resultado)
    {
        if (resultado.EsExitoso)
            return Ok(resultado.Valor);

        return resultado.CodigoEstado switch
        {
            404 => NotFound(CrearProblemDetails(404, resultado.Error)),
            409 => Conflict(CrearProblemDetails(409, resultado.Error)),
            400 => BadRequest(CrearProblemDetails(400, resultado.Error)),
            _ => BadRequest(CrearProblemDetails(resultado.CodigoEstado, resultado.Error))
        };
    }

    private static ProblemDetails CrearProblemDetails(int status, string? detalle) => new()
    {
        Status = status,
        Detail = detalle
    };
}

