namespace Seven.Facturacion.Application.DTOs;

/// <summary>
/// DTO para solicitud de login.
/// </summary>
public record LoginRequestDto(string Username, string Password);

/// <summary>
/// DTO para respuesta de login.
/// </summary>
public record LoginResponseDto(string Token, string Username);

