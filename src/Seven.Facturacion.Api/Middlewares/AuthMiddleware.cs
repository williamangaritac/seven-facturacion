using Seven.Facturacion.Application.Servicios;

namespace Seven.Facturacion.Api.Middlewares;

/// <summary>
/// Middleware de autenticación simple.
/// Valida el token en todas las peticiones excepto /api/auth/*.
/// </summary>
public class AuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IAuthServicio authServicio)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        // Rutas públicas (sin autenticación)
        if (path.StartsWith("/api/auth/") ||
            path.StartsWith("/swagger") ||
            path.StartsWith("/health") ||
            path == "/" ||
            path == "")
        {
            await next(context);
            return;
        }

        // Solo validar rutas de API
        if (!path.StartsWith("/api/"))
        {
            await next(context);
            return;
        }

        // Validar token
        var authorization = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "No autorizado" });
            return;
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        var esValido = await authServicio.ValidarTokenAsync(token);

        if (!esValido)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token inválido" });
            return;
        }

        await next(context);
    }
}

