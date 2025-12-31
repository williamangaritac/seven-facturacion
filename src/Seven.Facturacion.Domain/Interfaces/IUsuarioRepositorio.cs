namespace Seven.Facturacion.Domain.Interfaces;

/// <summary>
/// Repositorio de usuarios.
/// </summary>
public interface IUsuarioRepositorio : IRepositorio<Usuario>
{
    Task<Usuario?> ObtenerPorUsernameAsync(string username);
}

