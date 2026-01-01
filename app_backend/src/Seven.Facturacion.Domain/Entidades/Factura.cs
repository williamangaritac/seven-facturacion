// ============================================================================
// ENTIDAD: Factura
// Descripción: Representa el encabezado de una factura con cálculo de totales
// ============================================================================

namespace Seven.Facturacion.Domain.Entidades;

/// <summary>
/// Entidad que representa una factura en el sistema.
/// </summary>
public class Factura
{
    /// <summary>
    /// Porcentaje de IVA aplicado a las facturas.
    /// </summary>
    public const decimal PorcentajeIva = 0.19m;

    /// <summary>
    /// Identificador único de la factura.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Número único de factura (formato: FAC-YYYYMMDDHHMMSS).
    /// </summary>
    [Required]
    [StringLength(20)]
    public required string NumeroFactura { get; set; }

    /// <summary>
    /// Identificador del cliente asociado.
    /// </summary>
    [Required]
    public required int ClienteId { get; set; }

    /// <summary>
    /// Fecha de emisión de la factura.
    /// </summary>
    [Required]
    public required DateTime Fecha { get; set; }

    /// <summary>
    /// Subtotal sin impuestos (suma de subtotales de detalles).
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Valor del impuesto (IVA 19%).
    /// </summary>
    public decimal Impuesto { get; set; }

    /// <summary>
    /// Total de la factura (Subtotal + Impuesto).
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Estado de la factura: PENDIENTE, PAGADA, ANULADA.
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = EstadoFactura.Pendiente;

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización del registro.
    /// </summary>
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    // ========================================================================
    // PROPIEDADES DE NAVEGACIÓN
    // ========================================================================

    /// <summary>
    /// Cliente asociado a la factura.
    /// </summary>
    public Cliente Cliente { get; set; } = null!;

    /// <summary>
    /// Colección de detalles (líneas) de la factura.
    /// Inicializada con collection expression de C# 14.
    /// </summary>
    public ICollection<DetalleFactura> Detalles { get; set; } = [];

    // ========================================================================
    // MÉTODOS DE DOMINIO
    // ========================================================================

    /// <summary>
    /// Calcula los totales de la factura basándose en los detalles.
    /// </summary>
    public void CalcularTotales()
    {
        Subtotal = Detalles.Sum(d => d.Subtotal);
        Impuesto = Math.Round(Subtotal * PorcentajeIva, 2);
        Total = Subtotal + Impuesto;
        FechaActualizacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Agrega un detalle a la factura y recalcula totales.
    /// </summary>
    /// <param name="detalle">Detalle a agregar.</param>
    public void AgregarDetalle(DetalleFactura detalle)
    {
        detalle.FacturaId = Id;
        Detalles.Add(detalle);
        CalcularTotales();
    }

    /// <summary>
    /// Marca la factura como pagada.
    /// </summary>
    public void MarcarComoPagada()
    {
        if (Estado == EstadoFactura.Anulada)
        {
            throw new DomainException("No se puede pagar una factura anulada");
        }

        Estado = EstadoFactura.Pagada;
        FechaActualizacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Anula la factura.
    /// </summary>
    public void Anular()
    {
        if (Estado == EstadoFactura.Pagada)
        {
            throw new DomainException("No se puede anular una factura ya pagada");
        }

        Estado = EstadoFactura.Anulada;
        FechaActualizacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Genera un número de factura único basado en la fecha actual.
    /// </summary>
    /// <returns>Número de factura con formato FAC-YYYYMMDDHHMMSS.</returns>
    public static string GenerarNumeroFactura() =>
        $"FAC-{DateTime.UtcNow:yyyyMMddHHmmss}";
}

/// <summary>
/// Constantes para los estados válidos de una factura.
/// </summary>
public static class EstadoFactura
{
    public const string Pendiente = "PENDIENTE";
    public const string Pagada = "PAGADA";
    public const string Anulada = "ANULADA";
}

