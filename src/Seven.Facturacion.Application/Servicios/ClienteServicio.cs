// ============================================================================
// SERVICIO: ClienteServicio
// Descripción: Implementación de lógica de negocio para clientes
// ============================================================================

namespace Seven.Facturacion.Application.Servicios;

/// <summary>
/// Servicio que implementa los casos de uso para la gestión de clientes.
/// Utiliza primary constructor de C# 14.
/// </summary>
/// <param name="unidadDeTrabajo">Unidad de trabajo para acceso a repositorios.</param>
/// <param name="logger">Logger para registro de eventos.</param>
public class ClienteServicio(
    IUnidadDeTrabajo unidadDeTrabajo,
    ILogger<ClienteServicio> logger) : IClienteServicio
{
    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<ClienteDto>>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Obteniendo todos los clientes");
        
        var clientes = await unidadDeTrabajo.Clientes.ObtenerTodosAsync(ct);
        return Resultado<IEnumerable<ClienteDto>>.Exito(clientes.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<ClienteDto>> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        logger.LogInformation("Buscando cliente con ID: {ClienteId}", id);
        
        var cliente = await unidadDeTrabajo.Clientes.ObtenerPorIdAsync(id, ct);
        
        if (cliente is null)
        {
            logger.LogWarning("Cliente con ID {ClienteId} no encontrado", id);
            return Resultado<ClienteDto>.NoEncontrado($"Cliente con ID {id} no encontrado");
        }

        return Resultado<ClienteDto>.Exito(cliente.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<ClienteDto>> CrearAsync(CrearClienteDto dto, CancellationToken ct = default)
    {
        logger.LogInformation("Creando nuevo cliente: {Correo}", dto.CorreoElectronico);

        // Validar que el correo no esté duplicado
        var clienteExistente = await unidadDeTrabajo.Clientes.ObtenerPorCorreoAsync(dto.CorreoElectronico, ct);
        if (clienteExistente is not null)
        {
            logger.LogWarning("El correo {Correo} ya está registrado", dto.CorreoElectronico);
            return Resultado<ClienteDto>.Conflicto($"El correo electrónico '{dto.CorreoElectronico}' ya está registrado");
        }

        // Validar fecha de nacimiento
        if (dto.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
        {
            return Resultado<ClienteDto>.ErrorValidacion("FechaNacimiento", "La fecha de nacimiento no puede ser futura");
        }

        var cliente = dto.ToEntity();
        await unidadDeTrabajo.Clientes.AgregarAsync(cliente, ct);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Cliente creado exitosamente con ID: {ClienteId}", cliente.Id);
        return Resultado<ClienteDto>.Creado(cliente.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<ClienteDto>> ActualizarAsync(int id, ActualizarClienteDto dto, CancellationToken ct = default)
    {
        logger.LogInformation("Actualizando cliente con ID: {ClienteId}", id);

        var cliente = await unidadDeTrabajo.Clientes.ObtenerPorIdAsync(id, ct);
        if (cliente is null)
        {
            logger.LogWarning("Cliente con ID {ClienteId} no encontrado para actualizar", id);
            return Resultado<ClienteDto>.NoEncontrado($"Cliente con ID {id} no encontrado");
        }

        // Validar correo duplicado (excepto el mismo cliente)
        var clienteConCorreo = await unidadDeTrabajo.Clientes.ObtenerPorCorreoAsync(dto.CorreoElectronico, ct);
        if (clienteConCorreo is not null && clienteConCorreo.Id != id)
        {
            logger.LogWarning("El correo {Correo} ya está en uso por otro cliente", dto.CorreoElectronico);
            return Resultado<ClienteDto>.Conflicto($"El correo electrónico '{dto.CorreoElectronico}' ya está en uso");
        }

        cliente.ActualizarDesde(dto);
        unidadDeTrabajo.Clientes.Actualizar(cliente);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Cliente con ID {ClienteId} actualizado exitosamente", id);
        return Resultado<ClienteDto>.Exito(cliente.ToDto());
    }

    /// <inheritdoc />
    public async Task<Resultado<bool>> EliminarAsync(int id, CancellationToken ct = default)
    {
        logger.LogInformation("Eliminando cliente con ID: {ClienteId}", id);

        var cliente = await unidadDeTrabajo.Clientes.ObtenerConFacturasAsync(id, ct);
        if (cliente is null)
        {
            logger.LogWarning("Cliente con ID {ClienteId} no encontrado para eliminar", id);
            return Resultado<bool>.NoEncontrado($"Cliente con ID {id} no encontrado");
        }

        // Validar que no tenga facturas asociadas
        if (cliente.Facturas.Count > 0)
        {
            logger.LogWarning("No se puede eliminar cliente {ClienteId} porque tiene facturas asociadas", id);
            return Resultado<bool>.Falla($"No se puede eliminar el cliente porque tiene {cliente.Facturas.Count} factura(s) asociada(s)");
        }

        unidadDeTrabajo.Clientes.Eliminar(cliente);
        await unidadDeTrabajo.GuardarCambiosAsync(ct);

        logger.LogInformation("Cliente con ID {ClienteId} eliminado exitosamente", id);
        return Resultado<bool>.Exito(true);
    }

    /// <inheritdoc />
    public async Task<Resultado<IEnumerable<ClientePorEdadYCompraDto>>> ObtenerPorEdadYFechaCompraAsync(
        int edadMaxima,
        DateTime fechaDesde,
        DateTime fechaHasta,
        CancellationToken ct = default)
    {
        // PostgreSQL requiere DateTimeKind específico (UTC)
        fechaDesde = DateTime.SpecifyKind(fechaDesde, DateTimeKind.Utc);
        fechaHasta = DateTime.SpecifyKind(fechaHasta, DateTimeKind.Utc);

        logger.LogInformation(
            "Buscando clientes con edad <= {Edad} y compras entre {FechaDesde} y {FechaHasta}",
            edadMaxima, fechaDesde, fechaHasta);

        var clientes = await unidadDeTrabajo.Clientes
            .ObtenerPorEdadYFechaCompraAsync(edadMaxima, fechaDesde, fechaHasta, ct);

        var resultado = clientes.Select(c => new ClientePorEdadYCompraDto(
            Id: c.Id,
            NombreCompleto: c.NombreCompleto,
            CorreoElectronico: c.CorreoElectronico,
            Edad: c.Edad,
            TotalComprasPeriodo: c.Facturas.Count
        ));

        logger.LogInformation("Se encontraron {Cantidad} clientes", resultado.Count());
        return Resultado<IEnumerable<ClientePorEdadYCompraDto>>.Exito(resultado);
    }
}

