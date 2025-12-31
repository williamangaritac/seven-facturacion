namespace Seven.Facturacion.Domain.Entidades;

/// <summary>
/// Entidad Usuario para autenticación.
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
}

