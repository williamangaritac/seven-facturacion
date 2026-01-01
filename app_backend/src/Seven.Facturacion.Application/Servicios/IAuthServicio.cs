using Seven.Facturacion.Application.Comun;
using Seven.Facturacion.Application.DTOs;

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Interfaz del servicio de autenticación.
/// </summary>
public interface IAuthServicio
{
    Task<Resultado<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<bool> ValidarTokenAsync(string token);
}

