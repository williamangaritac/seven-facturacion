// ============================================================================
// MIDDLEWARE: ManejadorExcepcionesMiddleware
// Descripción: Manejo global de excepciones con respuestas RFC 7807
// ============================================================================

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Seven.Facturacion.Domain.Excepciones;

namespace Seven.Facturacion.Api.Middlewares;

/// <summary>
/// Middleware para capturar excepciones globalmente y retornar respuestas consistentes.
/// Implementa el estándar RFC 7807 (Problem Details).
/// </summary>
/// <param name="siguiente">Siguiente middleware en el pipeline.</param>
/// <param name="logger">Logger para registro de errores.</param>
/// <param name="entorno">Información del entorno de ejecución.</param>
public class ManejadorExcepcionesMiddleware(
    RequestDelegate siguiente,
    ILogger<ManejadorExcepcionesMiddleware> logger,
    IHostEnvironment entorno)
{
    /// <summary>
    /// Procesa la solicitud HTTP y captura excepciones.
    /// </summary>
    /// <param name="contexto">Contexto HTTP de la solicitud.</param>
    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await siguiente(contexto);
        }
        catch (Exception excepcion)
        {
            await ManejarExcepcionAsync(contexto, excepcion);
        }
    }

    /// <summary>
    /// Maneja la excepción y genera una respuesta apropiada.
    /// </summary>
    private async Task ManejarExcepcionAsync(HttpContext contexto, Exception excepcion)
    {
        var (statusCode, problemDetails) = CrearProblemDetails(excepcion, contexto);

        logger.LogError(
            excepcion,
            "Error procesando solicitud {Metodo} {Ruta}: {Mensaje}",
            contexto.Request.Method,
            contexto.Request.Path,
            excepcion.Message);

        contexto.Response.ContentType = "application/problem+json";
        contexto.Response.StatusCode = statusCode;

        var opciones = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        await contexto.Response.WriteAsJsonAsync(problemDetails, opciones);
    }

    /// <summary>
    /// Crea el objeto ProblemDetails según el tipo de excepción.
    /// </summary>
    private (int StatusCode, ProblemDetails Details) CrearProblemDetails(
        Exception excepcion,
        HttpContext contexto)
    {
        return excepcion switch
        {
            // Excepciones de dominio → 400 Bad Request
            DomainException ex => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación",
                    Detail = ex.Message,
                    Instance = contexto.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                }),

            // Excepciones de argumento → 400 Bad Request
            ArgumentException ex => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Argumento inválido",
                    Detail = ex.Message,
                    Instance = contexto.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                }),

            // Operación inválida → 409 Conflict
            InvalidOperationException ex => (
                StatusCodes.Status409Conflict,
                new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Operación no permitida",
                    Detail = ex.Message,
                    Instance = contexto.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
                }),

            // Cualquier otra excepción → 500 Internal Server Error
            _ => (
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
                    Detail = entorno.IsDevelopment() 
                        ? excepcion.Message 
                        : "Ha ocurrido un error inesperado. Por favor, intente más tarde.",
                    Instance = contexto.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Extensions =
                    {
                        ["traceId"] = contexto.TraceIdentifier
                    }
                })
        };
    }
}

