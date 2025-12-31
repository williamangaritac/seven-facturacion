using Seven.Facturacion.Application.Comun;
using Seven.Facturacion.Application.DTOs;
using Seven.Facturacion.Domain.Interfaces;

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Servicio de autenticación simple con tokens en memoria.
/// Implementación minimalista siguiendo KISS y YAGNI.
/// </summary>
public class AuthServicio(IUsuarioRepositorio usuarioRepositorio) : IAuthServicio
{
    // Almacén simple de tokens en memoria (para producción usar Redis/DB)
    private static readonly Dictionary<string, string> _tokens = new();

    public async Task<Resultado<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        // Validar entrada
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Resultado<LoginResponseDto>.Falla("Usuario y contraseña son requeridos");

        // Buscar usuario
        var usuario = await usuarioRepositorio.ObtenerPorUsernameAsync(request.Username);
        if (usuario == null)
            return Resultado<LoginResponseDto>.Falla("Credenciales inválidas");

        // Verificar contraseña con BCrypt
        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            return Resultado<LoginResponseDto>.Falla("Credenciales inválidas");

        // Generar token simple (GUID)
        var token = Guid.NewGuid().ToString();
        _tokens[token] = usuario.Username;

        var response = new LoginResponseDto(token, usuario.Username);
        return Resultado<LoginResponseDto>.Exito(response);
    }

    public Task<bool> ValidarTokenAsync(string token)
    {
        return Task.FromResult(_tokens.ContainsKey(token));
    }
}

