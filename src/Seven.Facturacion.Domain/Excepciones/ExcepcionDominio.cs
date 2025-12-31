// ============================================================================
// EXCEPCIÓN: DomainException
// Descripción: Excepción personalizada para errores de reglas de negocio
// ============================================================================

namespace Seven.Facturacion.Domain.Excepciones;

/// <summary>
/// Excepción lanzada cuando se viola una regla de negocio del dominio.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Código de error opcional para identificar el tipo de excepción.
    /// </summary>
    public string? CodigoError { get; }

    /// <summary>
    /// Crea una nueva instancia de DomainException con un mensaje.
    /// </summary>
    /// <param name="mensaje">Mensaje descriptivo del error.</param>
    public DomainException(string mensaje) : base(mensaje)
    {
    }

    /// <summary>
    /// Crea una nueva instancia de DomainException con mensaje y código de error.
    /// </summary>
    /// <param name="mensaje">Mensaje descriptivo del error.</param>
    /// <param name="codigoError">Código identificador del error.</param>
    public DomainException(string mensaje, string codigoError) : base(mensaje)
    {
        CodigoError = codigoError;
    }

    /// <summary>
    /// Crea una nueva instancia de DomainException con mensaje y excepción interna.
    /// </summary>
    /// <param name="mensaje">Mensaje descriptivo del error.</param>
    /// <param name="excepcionInterna">Excepción original que causó este error.</param>
    public DomainException(string mensaje, Exception excepcionInterna)
        : base(mensaje, excepcionInterna)
    {
    }

    /// <summary>
    /// Crea una nueva instancia de DomainException con todos los parámetros.
    /// </summary>
    /// <param name="mensaje">Mensaje descriptivo del error.</param>
    /// <param name="codigoError">Código identificador del error.</param>
    /// <param name="excepcionInterna">Excepción original que causó este error.</param>
    public DomainException(string mensaje, string codigoError, Exception excepcionInterna)
        : base(mensaje, excepcionInterna)
    {
        CodigoError = codigoError;
    }
}

/// <summary>
/// Excepción lanzada cuando no se encuentra una entidad solicitada.
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Nombre de la entidad no encontrada.
    /// </summary>
    public string NombreEntidad { get; }

    /// <summary>
    /// Identificador buscado.
    /// </summary>
    public object? Identificador { get; }

    /// <summary>
    /// Crea una nueva instancia de EntityNotFoundException.
    /// </summary>
    /// <param name="nombreEntidad">Nombre de la entidad.</param>
    /// <param name="identificador">ID buscado.</param>
    public EntityNotFoundException(string nombreEntidad, object? identificador)
        : base($"{nombreEntidad} con identificador '{identificador}' no fue encontrado", "ENTIDAD_NO_ENCONTRADA")
    {
        NombreEntidad = nombreEntidad;
        Identificador = identificador;
    }
}

/// <summary>
/// Excepción lanzada cuando hay un conflicto de validación.
/// </summary>
public class ValidationException : DomainException
{
    /// <summary>
    /// Diccionario de errores de validación por campo.
    /// </summary>
    public IDictionary<string, string[]> Errores { get; }

    /// <summary>
    /// Crea una nueva instancia de ValidationException.
    /// </summary>
    /// <param name="errores">Diccionario de errores por campo.</param>
    public ValidationException(IDictionary<string, string[]> errores)
        : base("Se encontraron errores de validación", "VALIDACION_FALLIDA")
    {
        Errores = errores;
    }

    /// <summary>
    /// Crea una nueva instancia de ValidationException con un solo error.
    /// </summary>
    /// <param name="campo">Nombre del campo con error.</param>
    /// <param name="mensaje">Mensaje de error.</param>
    public ValidationException(string campo, string mensaje)
        : base($"Error de validación en '{campo}': {mensaje}", "VALIDACION_FALLIDA")
    {
        Errores = new Dictionary<string, string[]>
        {
            { campo, [mensaje] }
        };
    }
}

