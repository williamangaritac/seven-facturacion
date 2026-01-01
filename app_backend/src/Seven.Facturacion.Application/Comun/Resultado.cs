// ============================================================================
// CLASE: Resultado<T> (Result Pattern)
// Descripción: Encapsula resultados de operaciones con éxito o error
// ============================================================================

namespace Seven.Facturacion.Application.Comun;

/// <summary>
/// Representa el resultado de una operación que puede ser exitosa o fallida.
/// Implementa el patrón Result para evitar excepciones en flujo normal.
/// </summary>
/// <typeparam name="T">Tipo del valor retornado en caso de éxito.</typeparam>
public class Resultado<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool EsExitoso { get; }

    /// <summary>
    /// Valor retornado en caso de éxito.
    /// </summary>
    public T? Valor { get; }

    /// <summary>
    /// Mensaje de error en caso de falla.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Código de estado HTTP sugerido.
    /// </summary>
    public int CodigoEstado { get; }

    /// <summary>
    /// Errores de validación detallados (opcional).
    /// </summary>
    public IDictionary<string, string[]>? ErroresValidacion { get; }

    /// <summary>
    /// Constructor protegido para permitir herencia.
    /// </summary>
    protected Resultado(bool esExitoso, T? valor, string? error, int codigoEstado, IDictionary<string, string[]>? erroresValidacion = null)
    {
        EsExitoso = esExitoso;
        Valor = valor;
        Error = error;
        CodigoEstado = codigoEstado;
        ErroresValidacion = erroresValidacion;
    }

    // ========================================================================
    // MÉTODOS DE FÁBRICA - ÉXITO
    // ========================================================================

    /// <summary>
    /// Crea un resultado exitoso con código 200.
    /// </summary>
    public static Resultado<T> Exito(T valor) => 
        new(true, valor, null, 200);

    /// <summary>
    /// Crea un resultado exitoso con código 201 (Created).
    /// </summary>
    public static Resultado<T> Creado(T valor) => 
        new(true, valor, null, 201);

    /// <summary>
    /// Crea un resultado exitoso sin contenido con código 204.
    /// </summary>
    public static Resultado<T> SinContenido() => 
        new(true, default, null, 204);

    // ========================================================================
    // MÉTODOS DE FÁBRICA - ERROR
    // ========================================================================

    /// <summary>
    /// Crea un resultado de error con código personalizado.
    /// </summary>
    public static Resultado<T> Falla(string error, int codigoEstado = 400) => 
        new(false, default, error, codigoEstado);

    /// <summary>
    /// Crea un resultado de error 404 (Not Found).
    /// </summary>
    public static Resultado<T> NoEncontrado(string error) => 
        new(false, default, error, 404);

    /// <summary>
    /// Crea un resultado de error 400 con errores de validación.
    /// </summary>
    public static Resultado<T> ErrorValidacion(IDictionary<string, string[]> errores) => 
        new(false, default, "Error de validación", 400, errores);

    /// <summary>
    /// Crea un resultado de error 400 con un solo error de validación.
    /// </summary>
    public static Resultado<T> ErrorValidacion(string campo, string mensaje) => 
        new(false, default, mensaje, 400, new Dictionary<string, string[]> { { campo, [mensaje] } });

    /// <summary>
    /// Crea un resultado de error 409 (Conflict).
    /// </summary>
    public static Resultado<T> Conflicto(string error) => 
        new(false, default, error, 409);

    /// <summary>
    /// Crea un resultado de error 500 (Internal Server Error).
    /// </summary>
    public static Resultado<T> ErrorInterno(string error) => 
        new(false, default, error, 500);
}

/// <summary>
/// Resultado sin valor de retorno (para operaciones void).
/// </summary>
public class Resultado : Resultado<bool>
{
    private Resultado(bool esExitoso, string? error, int codigoEstado)
        : base(esExitoso, esExitoso, error, codigoEstado)
    {
    }

    /// <summary>
    /// Crea un resultado exitoso sin valor.
    /// </summary>
    public static Resultado Ok() =>
        new(true, null, 200);

    /// <summary>
    /// Crea un resultado exitoso sin contenido (204).
    /// </summary>
    public static Resultado OkSinContenido() =>
        new(true, null, 204);

    /// <summary>
    /// Crea un resultado de error.
    /// </summary>
    public static new Resultado Error(string error, int codigoEstado = 400) =>
        new(false, error, codigoEstado);

    /// <summary>
    /// Crea un resultado de error 404.
    /// </summary>
    public static Resultado ErrorNoEncontrado(string error) =>
        new(false, error, 404);
}

