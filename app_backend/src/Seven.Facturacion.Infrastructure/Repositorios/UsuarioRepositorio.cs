using Microsoft.EntityFrameworkCore;
using Seven.Facturacion.Domain.Entidades;
using Seven.Facturacion.Domain.Interfaces;
using Seven.Facturacion.Infrastructure.Persistencia;

namespace Seven.Facturacion.Infrastructure.Repositorios;

/// <summary>
/// Implementación del repositorio de usuarios.
/// </summary>
public class UsuarioRepositorio(AppDbContext contexto) : IUsuarioRepositorio
{
    public async Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await contexto.Usuarios.FindAsync([id], ct);

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await contexto.Usuarios.ToListAsync(ct);

    public async Task<Usuario> AgregarAsync(Usuario entidad, CancellationToken ct = default)
    {
        await contexto.Usuarios.AddAsync(entidad, ct);
        return entidad;
    }

    public void Actualizar(Usuario entidad)
    {
        contexto.Usuarios.Update(entidad);
    }

    public void Eliminar(Usuario entidad)
    {
        contexto.Usuarios.Remove(entidad);
    }

    public async Task<bool> ExisteAsync(int id, CancellationToken ct = default) =>
        await contexto.Usuarios.AnyAsync(u => u.Id == id, ct);

    public async Task<Usuario?> ObtenerPorUsernameAsync(string username) =>
        await contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Username == username && u.Activo);
}

