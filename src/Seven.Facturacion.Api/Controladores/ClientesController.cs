// ============================================================================
// CONTROLADOR: ClientesController
// Descripción: API REST para gestión de clientes
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Seven.Facturacion.Application.Comun;
using Seven.Facturacion.Application.DTOs;
using Seven.Facturacion.Application.Servicios;

namespace Seven.Facturacion.Api.Controladores;

/// <summary>
/// Controlador para operaciones CRUD de clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController(IClienteServicio clienteServicio) : ControllerBase
{
    /// <summary>
    /// Obtiene todos los clientes.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de clientes.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken ct)
    {
        var resultado = await clienteServicio.ObtenerTodosAsync(ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene un cliente por su ID.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Cliente encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
    {
        var resultado = await clienteServicio.ObtenerPorIdAsync(id, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    /// <param name="dto">Datos del cliente a crear.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Cliente creado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear([FromBody] CrearClienteDto dto, CancellationToken ct)
    {
        var resultado = await clienteServicio.CrearAsync(dto, ct);
        
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
    /// Actualiza un cliente existente.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="dto">Datos actualizados del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Cliente actualizado.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarClienteDto dto, CancellationToken ct)
    {
        var resultado = await clienteServicio.ActualizarAsync(id, dto, ct);
        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Elimina un cliente.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>NoContent si se eliminó correctamente.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        var resultado = await clienteServicio.EliminarAsync(id, ct);
        
        if (resultado.EsExitoso)
            return NoContent();

        return ProcesarResultado(resultado);
    }

    /// <summary>
    /// Obtiene clientes por edad máxima y rango de fechas de compra.
    /// Implementa la consulta SQL #3 de los requerimientos.
    /// </summary>
    /// <param name="edadMaxima">Edad máxima de los clientes.</param>
    /// <param name="fechaDesde">Fecha inicial del rango.</param>
    /// <param name="fechaHasta">Fecha final del rango.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de clientes que cumplen los criterios.</returns>
    [HttpGet("por-edad-y-compra")]
    [ProducesResponseType(typeof(IEnumerable<ClientePorEdadYCompraDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorEdadYCompra(
        [FromQuery] int edadMaxima,
        [FromQuery] DateTime fechaDesde,
        [FromQuery] DateTime fechaHasta,
        CancellationToken ct)
    {
        var resultado = await clienteServicio.ObtenerPorEdadYFechaCompraAsync(
            edadMaxima, fechaDesde, fechaHasta, ct);
        return ProcesarResultado(resultado);
    }

    // ========================================================================
    // MÉTODOS AUXILIARES
    // ========================================================================

    /// <summary>
    /// Procesa el resultado del servicio y retorna la respuesta HTTP apropiada.
    /// </summary>
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

