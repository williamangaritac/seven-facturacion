// ============================================================================
// CONTROLADOR: FacturasController
// Descripción: API REST para gestión de facturas
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Seven.Facturacion.Application.Comun;
using Seven.Facturacion.Application.DTOs;
using Seven.Facturacion.Application.Servicios;

namespace Seven.Facturacion.Api.Controladores;

/// <summary>
/// Controlador para operaciones de facturas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FacturasController(IFacturaServicio facturaServicio) : ControllerBase
{
    /// <summary>
    /// Obtiene todas las facturas con sus detalles.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de facturas.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FacturaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct)
    {
        var resultado = await facturaServicio.ObtenerTodosAsync(ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene una factura por su ID.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Factura encontrada.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FacturaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var resultado = await facturaServicio.ObtenerPorIdAsync(id, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Crea una nueva factura.
    /// </summary>
    /// <param name="dto">Datos de la factura a crear.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Factura creada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(FacturaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Crear([FromBody] CrearFacturaDto dto, CancellationToken ct)
    {
        var resultado = await facturaServicio.CrearAsync(dto, ct);
        
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
    /// Actualiza una factura existente (cliente y detalles).
    /// Solo se pueden editar facturas en estado PENDIENTE.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="dto">Datos actualizados de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Factura actualizada.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(FacturaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarFacturaDto dto,
        CancellationToken ct)
    {
        var resultado = await facturaServicio.ActualizarAsync(id, dto, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Actualiza el estado de una factura.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="dto">Nuevo estado de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Factura actualizada.</returns>
    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(typeof(FacturaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActualizarEstado(
        int id,
        [FromBody] ActualizarEstadoFacturaDto dto,
        CancellationToken ct)
    {
        var resultado = await facturaServicio.ActualizarEstadoAsync(id, dto, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Elimina una factura.
    /// </summary>
    /// <param name="id">Identificador de la factura.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>NoContent si se eliminó correctamente.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        var resultado = await facturaServicio.EliminarAsync(id, ct);

        if (resultado.EsExitoso)
            return NoContent();

        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene las facturas de un cliente específico.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de facturas del cliente.</returns>
    [HttpGet("cliente/{clienteId:int}")]
    [ProducesResponseType(typeof(IEnumerable<FacturaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorCliente(int clienteId, CancellationToken ct)
    {
        var resultado = await facturaServicio.ObtenerPorClienteAsync(clienteId, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene el total vendido por producto en un año.
    /// Implementa la consulta SQL #4 de los requerimientos.
    /// </summary>
    /// <param name="anio">Año a consultar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Reporte de ventas por producto.</returns>
    [HttpGet("ventas-por-producto")]
    [ProducesResponseType(typeof(IEnumerable<VentasPorProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerVentasPorProducto(
        [FromQuery] int anio,
        CancellationToken ct)
    {
        var resultado = await facturaServicio.ObtenerVentasPorProductoAsync(anio, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Calcula la estimación de próxima compra de un cliente.
    /// Implementa la consulta SQL #5 de los requerimientos.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Estimación de próxima compra.</returns>
    [HttpGet("proxima-compra/{clienteId:int}")]
    [ProducesResponseType(typeof(ProximaCompraClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerProximaCompra(int clienteId, CancellationToken ct)
    {
        var resultado = await facturaServicio.ObtenerProximaCompraClienteAsync(clienteId, ct);
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

