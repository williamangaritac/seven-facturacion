using Microsoft.AspNetCore.Mvc;
using Seven.Facturacion.Application.DTOs;
using Seven.Facturacion.Application.Servicios;

namespace Seven.Facturacion.Api.Controladores;

/// <summary>
/// Controlador de autenticación.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthServicio authServicio) : ControllerBase
{
    /// <summary>
    /// Endpoint de login.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var resultado = await authServicio.LoginAsync(request);

        if (!resultado.EsExitoso)
            return BadRequest(new { message = resultado.Error });

        return Ok(resultado.Valor);
    }

    /// <summary>
    /// Endpoint para validar token.
    /// </summary>
    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ValidateToken([FromHeader(Name = "Authorization")] string? authorization)
    {
        if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Bearer "))
            return Unauthorized();

        var token = authorization.Substring("Bearer ".Length).Trim();
        var esValido = await authServicio.ValidarTokenAsync(token);

        if (!esValido)
            return Unauthorized();

        return Ok(new { valid = true });
    }
}

