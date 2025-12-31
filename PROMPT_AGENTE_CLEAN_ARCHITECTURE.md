# 🤖 PROMPT: Implementación Sistema Facturación - Clean Architecture

## CONTEXTO

Eres un ingeniero de software senior Microsoft MVP especializado en .NET. Implementarás un sistema de facturación usando Clean Architecture con las siguientes restricciones estrictas.

---

## 🎯 OBJETIVO

Implementar sistema de facturación fullstack:
- **Backend**: .NET 10, C# 14, Minimal APIs, EF Core 10, PostgreSQL
- **Frontend**: Angular 17 con DevExtreme
- **Arquitectura**: Clean Architecture + Repository Pattern

---

## ⚠️ RESTRICCIONES CRÍTICAS - MÍNIMAS DEPENDENCIAS

### SOLO estas dependencias NuGet permitidas:

```xml
<!-- OBLIGATORIAS -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />

<!-- OPCIONALES (solo si es estrictamente necesario) -->
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.0.0" /> <!-- Swagger UI -->
```

### PROHIBIDO usar:
- ❌ AutoMapper → Usar extension methods manuales
- ❌ MediatR → Inyección directa de servicios
- ❌ FluentValidation → Usar DataAnnotations + validación manual
- ❌ Newtonsoft.Json → Usar System.Text.Json nativo
- ❌ Serilog → Usar ILogger nativo de .NET
- ❌ Polly → Manejo de errores manual
- ❌ Cualquier otra librería no listada

### APROVECHAR características nativas C# 14 / .NET 10:
- Primary constructors
- Required members
- Collection expressions `[]`
- Records para DTOs
- Pattern matching avanzado
- File-scoped namespaces
- Global usings
- Minimal APIs con grupos
- Native AOT ready (opcional)

---

## 📁 ESTRUCTURA DE PROYECTOS

```
📁 src/
├── 📁 Seven.Facturacion.Domain/
│   ├── 📁 Entities/
│   │   ├── Cliente.cs
│   │   ├── Producto.cs
│   │   ├── Factura.cs
│   │   └── DetalleFactura.cs
│   ├── 📁 Interfaces/
│   │   ├── IClienteRepository.cs
│   │   ├── IProductoRepository.cs
│   │   ├── IFacturaRepository.cs
│   │   └── IUnitOfWork.cs
│   ├── 📁 Exceptions/
│   │   └── DomainException.cs
│   └── Seven.Facturacion.Domain.csproj
│
├── 📁 Seven.Facturacion.Application/
│   ├── 📁 DTOs/
│   │   ├── ClienteDto.cs
│   │   ├── ProductoDto.cs
│   │   ├── FacturaDto.cs
│   │   └── DetalleFacturaDto.cs
│   ├── 📁 Services/
│   │   ├── IClienteService.cs
│   │   ├── ClienteService.cs
│   │   ├── IProductoService.cs
│   │   ├── ProductoService.cs
│   │   ├── IFacturaService.cs
│   │   └── FacturaService.cs
│   ├── 📁 Extensions/
│   │   └── MappingExtensions.cs
│   ├── 📁 Common/
│   │   └── Result.cs
│   └── Seven.Facturacion.Application.csproj
│
├── 📁 Seven.Facturacion.Infrastructure/
│   ├── 📁 Persistence/
│   │   ├── AppDbContext.cs
│   │   └── 📁 Configurations/
│   │       ├── ClienteConfiguration.cs
│   │       ├── ProductoConfiguration.cs
│   │       ├── FacturaConfiguration.cs
│   │       └── DetalleFacturaConfiguration.cs
│   ├── 📁 Repositories/
│   │   ├── ClienteRepository.cs
│   │   ├── ProductoRepository.cs
│   │   ├── FacturaRepository.cs
│   │   └── UnitOfWork.cs
│   ├── DependencyInjection.cs
│   └── Seven.Facturacion.Infrastructure.csproj
│
└── 📁 Seven.Facturacion.API/
    ├── 📁 Endpoints/
    │   ├── ClienteEndpoints.cs
    │   ├── ProductoEndpoints.cs
    │   └── FacturaEndpoints.cs
    ├── 📁 Middleware/
    │   └── ExceptionMiddleware.cs
    ├── Program.cs
    ├── appsettings.json
    └── Seven.Facturacion.API.csproj
```

---

## 📋 ESPECIFICACIONES DE IMPLEMENTACIÓN

### 1. ENTIDADES DE DOMINIO

```csharp
// Domain/Entities/Cliente.cs
namespace Seven.Facturacion.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Email { get; set; }
    public string? Telefono { get; set; }
    public required DateOnly FechaNacimiento { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navegación
    public ICollection<Factura> Facturas { get; set; } = [];
    
    // Propiedad calculada
    public int Edad => DateOnly.FromDateTime(DateTime.Today).Year - FechaNacimiento.Year;
}

// Domain/Entities/Producto.cs
public class Producto
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public required decimal Precio { get; set; }
    public required int Stock { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<DetalleFactura> DetallesFactura { get; set; } = [];
}

// Domain/Entities/Factura.cs
public class Factura
{
    public int Id { get; set; }
    public required string NumeroFactura { get; set; }
    public required int ClienteId { get; set; }
    public required DateTime Fecha { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, PAGADA, ANULADA
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public Cliente Cliente { get; set; } = null!;
    public ICollection<DetalleFactura> Detalles { get; set; } = [];
    
    public void CalcularTotales()
    {
        Subtotal = Detalles.Sum(d => d.Subtotal);
        Impuesto = Subtotal * 0.19m; // IVA 19%
        Total = Subtotal + Impuesto;
    }
}

// Domain/Entities/DetalleFactura.cs
public class DetalleFactura
{
    public int Id { get; set; }
    public required int FacturaId { get; set; }
    public required int ProductoId { get; set; }
    public required int Cantidad { get; set; }
    public required decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
    
    public Factura Factura { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
```

### 2. INTERFACES DE REPOSITORIO (Domain)

```csharp
// Domain/Interfaces/IRepository.cs
namespace Seven.Facturacion.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
}

// Domain/Interfaces/IClienteRepository.cs
public interface IClienteRepository : IRepository<Cliente>
{
    Task<IEnumerable<Cliente>> GetClientesByEdadAndFechaCompraAsync(
        int edadMaxima, DateTime desde, DateTime hasta, CancellationToken ct = default);
}

// Domain/Interfaces/IProductoRepository.cs
public interface IProductoRepository : IRepository<Producto>
{
    Task<IEnumerable<Producto>> GetProductosBajoStockAsync(int stockMinimo, CancellationToken ct = default);
    Task<IEnumerable<object>> GetListaPreciosAsync(CancellationToken ct = default);
}

// Domain/Interfaces/IFacturaRepository.cs
public interface IFacturaRepository : IRepository<Factura>
{
    Task<Factura?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Factura>> GetAllWithDetailsAsync(CancellationToken ct = default);
    Task<IEnumerable<object>> GetVentasPorProductoAsync(int year, CancellationToken ct = default);
    Task<object?> GetProximaCompraClienteAsync(int clienteId, CancellationToken ct = default);
}

// Domain/Interfaces/IUnitOfWork.cs
public interface IUnitOfWork : IDisposable
{
    IClienteRepository Clientes { get; }
    IProductoRepository Productos { get; }
    IFacturaRepository Facturas { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
```

### 3. DTOs CON RECORDS (Application)

```csharp
// Application/DTOs/ClienteDto.cs
namespace Seven.Facturacion.Application.DTOs;

public record ClienteDto(
    int Id,
    string Nombre,
    string Apellido,
    string NombreCompleto,
    string Email,
    string? Telefono,
    DateOnly FechaNacimiento,
    int Edad,
    string? Direccion,
    bool Activo
);

public record CreateClienteDto(
    [property: Required, StringLength(100)] string Nombre,
    [property: Required, StringLength(100)] string Apellido,
    [property: Required, EmailAddress] string Email,
    [property: Phone] string? Telefono,
    [property: Required] DateOnly FechaNacimiento,
    string? Direccion
);

public record UpdateClienteDto(
    [property: Required, StringLength(100)] string Nombre,
    [property: Required, StringLength(100)] string Apellido,
    [property: Required, EmailAddress] string Email,
    [property: Phone] string? Telefono,
    string? Direccion
);

// Application/DTOs/ProductoDto.cs
public record ProductoDto(
    int Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock,
    bool Activo
);

public record CreateProductoDto(
    [property: Required, StringLength(50)] string Codigo,
    [property: Required, StringLength(200)] string Nombre,
    string? Descripcion,
    [property: Required, Range(0.01, double.MaxValue)] decimal Precio,
    [property: Required, Range(0, int.MaxValue)] int Stock
);

public record ListaPrecioDto(int Id, string Codigo, string Nombre, decimal Precio);

public record ProductoBajoStockDto(int Id, string Codigo, string Nombre, int Stock);

// Application/DTOs/FacturaDto.cs
public record FacturaDto(
    int Id,
    string NumeroFactura,
    int ClienteId,
    string ClienteNombre,
    DateTime Fecha,
    decimal Subtotal,
    decimal Impuesto,
    decimal Total,
    string Estado,
    IEnumerable<DetalleFacturaDto> Detalles
);

public record CreateFacturaDto(
    [property: Required] int ClienteId,
    [property: Required, MinLength(1)] IEnumerable<CreateDetalleFacturaDto> Detalles
);

public record DetalleFacturaDto(
    int Id,
    int ProductoId,
    string ProductoNombre,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal
);

public record CreateDetalleFacturaDto(
    [property: Required] int ProductoId,
    [property: Required, Range(1, int.MaxValue)] int Cantidad
);

// DTOs para consultas específicas
public record VentasPorProductoDto(int ProductoId, string ProductoNombre, decimal TotalVendido, int CantidadVendida);

public record ProximaCompraClienteDto(
    int ClienteId,
    string ClienteNombre,
    DateTime UltimaCompra,
    int DiasPromedio,
    DateTime ProximaCompraEstimada
);
```

### 4. RESULT PATTERN (Application/Common)

```csharp
// Application/Common/Result.cs
namespace Seven.Facturacion.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public int StatusCode { get; }

    private Result(bool isSuccess, T? value, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value) => new(true, value, null, 200);
    public static Result<T> Created(T value) => new(true, value, null, 201);
    public static Result<T> Failure(string error, int statusCode = 400) => new(false, default, error, statusCode);
    public static Result<T> NotFound(string error) => new(false, default, error, 404);
}
```

### 5. MAPPING EXTENSIONS (Sin AutoMapper)

```csharp
// Application/Extensions/MappingExtensions.cs
namespace Seven.Facturacion.Application.Extensions;

public static class MappingExtensions
{
    // Cliente mappings
    public static ClienteDto ToDto(this Cliente c) => new(
        c.Id, c.Nombre, c.Apellido, $"{c.Nombre} {c.Apellido}",
        c.Email, c.Telefono, c.FechaNacimiento, c.Edad, c.Direccion, c.Activo
    );

    public static Cliente ToEntity(this CreateClienteDto dto) => new()
    {
        Nombre = dto.Nombre,
        Apellido = dto.Apellido,
        Email = dto.Email,
        Telefono = dto.Telefono,
        FechaNacimiento = dto.FechaNacimiento,
        Direccion = dto.Direccion
    };

    public static IEnumerable<ClienteDto> ToDto(this IEnumerable<Cliente> clientes) =>
        clientes.Select(c => c.ToDto());

    // Producto mappings
    public static ProductoDto ToDto(this Producto p) => new(
        p.Id, p.Codigo, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Activo
    );

    public static Producto ToEntity(this CreateProductoDto dto) => new()
    {
        Codigo = dto.Codigo,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        Precio = dto.Precio,
        Stock = dto.Stock
    };

    public static IEnumerable<ProductoDto> ToDto(this IEnumerable<Producto> productos) =>
        productos.Select(p => p.ToDto());

    // Factura mappings
    public static FacturaDto ToDto(this Factura f) => new(
        f.Id, f.NumeroFactura, f.ClienteId,
        $"{f.Cliente.Nombre} {f.Cliente.Apellido}",
        f.Fecha, f.Subtotal, f.Impuesto, f.Total, f.Estado,
        f.Detalles.Select(d => d.ToDto())
    );

    public static DetalleFacturaDto ToDto(this DetalleFactura d) => new(
        d.Id, d.ProductoId, d.Producto.Nombre,
        d.Cantidad, d.PrecioUnitario, d.Subtotal
    );

    public static IEnumerable<FacturaDto> ToDto(this IEnumerable<Factura> facturas) =>
        facturas.Select(f => f.ToDto());
}
```

### 6. SERVICIOS (Application Layer)

```csharp
// Application/Services/IFacturaService.cs
namespace Seven.Facturacion.Application.Services;

public interface IFacturaService
{
    Task<Result<IEnumerable<FacturaDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<FacturaDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<FacturaDto>> CreateAsync(CreateFacturaDto dto, CancellationToken ct = default);
    Task<Result<FacturaDto>> UpdateAsync(int id, CreateFacturaDto dto, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result<IEnumerable<VentasPorProductoDto>>> GetVentasPorProductoAsync(int year, CancellationToken ct = default);
    Task<Result<ProximaCompraClienteDto>> GetProximaCompraClienteAsync(int clienteId, CancellationToken ct = default);
}

// Application/Services/FacturaService.cs
public class FacturaService(IUnitOfWork uow, ILogger<FacturaService> logger) : IFacturaService
{
    public async Task<Result<IEnumerable<FacturaDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var facturas = await uow.Facturas.GetAllWithDetailsAsync(ct);
        return Result<IEnumerable<FacturaDto>>.Success(facturas.ToDto());
    }

    public async Task<Result<FacturaDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var factura = await uow.Facturas.GetByIdWithDetailsAsync(id, ct);
        return factura is null
            ? Result<FacturaDto>.NotFound($"Factura {id} no encontrada")
            : Result<FacturaDto>.Success(factura.ToDto());
    }

    public async Task<Result<FacturaDto>> CreateAsync(CreateFacturaDto dto, CancellationToken ct = default)
    {
        // Validar cliente existe
        var cliente = await uow.Clientes.GetByIdAsync(dto.ClienteId, ct);
        if (cliente is null)
            return Result<FacturaDto>.NotFound($"Cliente {dto.ClienteId} no encontrado");

        // Crear factura
        var factura = new Factura
        {
            NumeroFactura = $"FAC-{DateTime.UtcNow:yyyyMMddHHmmss}",
            ClienteId = dto.ClienteId,
            Fecha = DateTime.UtcNow,
            Detalles = []
        };

        // Agregar detalles y validar productos
        foreach (var detalle in dto.Detalles)
        {
            var producto = await uow.Productos.GetByIdAsync(detalle.ProductoId, ct);
            if (producto is null)
                return Result<FacturaDto>.NotFound($"Producto {detalle.ProductoId} no encontrado");

            if (producto.Stock < detalle.Cantidad)
                return Result<FacturaDto>.Failure($"Stock insuficiente para {producto.Nombre}");

            factura.Detalles.Add(new DetalleFactura
            {
                ProductoId = detalle.ProductoId,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = producto.Precio
            });

            // Actualizar stock
            producto.Stock -= detalle.Cantidad;
            uow.Productos.Update(producto);
        }

        factura.CalcularTotales();
        await uow.Facturas.AddAsync(factura, ct);
        await uow.SaveChangesAsync(ct);

        logger.LogInformation("Factura {Numero} creada", factura.NumeroFactura);

        var created = await uow.Facturas.GetByIdWithDetailsAsync(factura.Id, ct);
        return Result<FacturaDto>.Created(created!.ToDto());
    }

    // ... implementar otros métodos
}
```

### 7. INFRASTRUCTURE - DbContext

```csharp
// Infrastructure/Persistence/AppDbContext.cs
namespace Seven.Facturacion.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<DetalleFactura> DetallesFactura => Set<DetalleFactura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<Cliente>()
            .Concat<EntityEntry>(ChangeTracker.Entries<Producto>())
            .Concat(ChangeTracker.Entries<Factura>())
            .Where(e => e.State is EntityState.Modified))
        {
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(ct);
    }
}

// Infrastructure/Persistence/Configurations/FacturaConfiguration.cs
public class FacturaConfiguration : IEntityTypeConfiguration<Factura>
{
    public void Configure(EntityTypeBuilder<Factura> builder)
    {
        builder.ToTable("facturas");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id");
        builder.Property(f => f.NumeroFactura).HasColumnName("numero_factura").HasMaxLength(20).IsRequired();
        builder.Property(f => f.ClienteId).HasColumnName("cliente_id");
        builder.Property(f => f.Fecha).HasColumnName("fecha");
        builder.Property(f => f.Subtotal).HasColumnName("subtotal").HasPrecision(18, 2);
        builder.Property(f => f.Impuesto).HasColumnName("impuesto").HasPrecision(18, 2);
        builder.Property(f => f.Total).HasColumnName("total").HasPrecision(18, 2);
        builder.Property(f => f.Estado).HasColumnName("estado").HasMaxLength(20);
        builder.Property(f => f.CreatedAt).HasColumnName("created_at");
        builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(f => f.NumeroFactura).IsUnique();
        builder.HasIndex(f => f.ClienteId);
        builder.HasIndex(f => f.Fecha);

        builder.HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId);

        builder.HasMany(f => f.Detalles)
            .WithOne(d => d.Factura)
            .HasForeignKey(d => d.FacturaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### 8. REPOSITORY IMPLEMENTATIONS

```csharp
// Infrastructure/Repositories/FacturaRepository.cs
namespace Seven.Facturacion.Infrastructure.Repositories;

public class FacturaRepository(AppDbContext context) : IFacturaRepository
{
    public async Task<Factura?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Facturas.FindAsync([id], ct);

    public async Task<Factura?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
        await context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<IEnumerable<Factura>> GetAllAsync(CancellationToken ct = default) =>
        await context.Facturas.ToListAsync(ct);

    public async Task<IEnumerable<Factura>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
        await context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync(ct);

    public async Task<Factura> AddAsync(Factura entity, CancellationToken ct = default)
    {
        await context.Facturas.AddAsync(entity, ct);
        return entity;
    }

    public void Update(Factura entity) => context.Facturas.Update(entity);

    public void Delete(Factura entity) => context.Facturas.Remove(entity);

    // Consulta específica: Ventas por producto durante un año
    public async Task<IEnumerable<object>> GetVentasPorProductoAsync(int year, CancellationToken ct = default) =>
        await context.DetallesFactura
            .Include(d => d.Factura)
            .Include(d => d.Producto)
            .Where(d => d.Factura.Fecha.Year == year)
            .GroupBy(d => new { d.ProductoId, d.Producto.Nombre })
            .Select(g => new VentasPorProductoDto(
                g.Key.ProductoId,
                g.Key.Nombre,
                g.Sum(d => d.Subtotal),
                g.Sum(d => d.Cantidad)
            ))
            .ToListAsync(ct);

    // Consulta específica: Próxima compra estimada
    public async Task<object?> GetProximaCompraClienteAsync(int clienteId, CancellationToken ct = default)
    {
        var facturas = await context.Facturas
            .Include(f => f.Cliente)
            .Where(f => f.ClienteId == clienteId)
            .OrderBy(f => f.Fecha)
            .ToListAsync(ct);

        if (facturas.Count < 2)
            return null;

        var cliente = facturas.First().Cliente;
        var ultimaCompra = facturas.Last().Fecha;
        
        // Calcular promedio de días entre compras
        var diferencias = new List<int>();
        for (int i = 1; i < facturas.Count; i++)
        {
            diferencias.Add((facturas[i].Fecha - facturas[i - 1].Fecha).Days);
        }
        var promedioDias = (int)diferencias.Average();

        return new ProximaCompraClienteDto(
            clienteId,
            $"{cliente.Nombre} {cliente.Apellido}",
            ultimaCompra,
            promedioDias,
            ultimaCompra.AddDays(promedioDias)
        );
    }
}
```

### 9. MINIMAL API ENDPOINTS

```csharp
// API/Endpoints/FacturaEndpoints.cs
namespace Seven.Facturacion.API.Endpoints;

public static class FacturaEndpoints
{
    public static void MapFacturaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/facturas")
            .WithTags("Facturas")
            .WithOpenApi();

        group.MapGet("/", async (IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.GetAllAsync(ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Error);
        })
        .WithName("GetFacturas")
        .Produces<IEnumerable<FacturaDto>>();

        group.MapGet("/{id:int}", async (int id, IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.GetByIdAsync(id, ct);
            return result.IsSuccess 
                ? Results.Ok(result.Value) 
                : Results.NotFound(result.Error);
        })
        .WithName("GetFacturaById")
        .Produces<FacturaDto>()
        .ProducesProblem(404);

        group.MapPost("/", async (CreateFacturaDto dto, IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.CreateAsync(dto, ct);
            return result.IsSuccess
                ? Results.Created($"/api/facturas/{result.Value!.Id}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateFactura")
        .Produces<FacturaDto>(201)
        .ProducesValidationProblem();

        group.MapPut("/{id:int}", async (int id, CreateFacturaDto dto, IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(id, dto, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("UpdateFactura");

        group.MapDelete("/{id:int}", async (int id, IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.DeleteAsync(id, ct);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
        })
        .WithName("DeleteFactura");

        // Consultas específicas
        group.MapGet("/ventas-por-producto/{year:int}", async (int year, IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.GetVentasPorProductoAsync(year, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Error);
        })
        .WithName("GetVentasPorProducto");

        group.MapGet("/proxima-compra/{clienteId:int}", async (int clienteId, IFacturaService service, CancellationToken ct) =>
        {
            var result = await service.GetProximaCompraClienteAsync(clienteId, ct);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        })
        .WithName("GetProximaCompraCliente");
    }
}
```

### 10. PROGRAM.CS (Composición)

```csharp
// API/Program.cs
using Seven.Facturacion.Infrastructure;
using Seven.Facturacion.API.Endpoints;
using Seven.Facturacion.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");

// Endpoints
app.MapClienteEndpoints();
app.MapProductoEndpoints();
app.MapFacturaEndpoints();

app.Run();
```

### 11. EXCEPTION MIDDLEWARE

```csharp
// API/Middleware/ExceptionMiddleware.cs
namespace Seven.Facturacion.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no manejado: {Message}", ex.Message);
            
            context.Response.StatusCode = ex switch
            {
                ArgumentException => 400,
                KeyNotFoundException => 404,
                _ => 500
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message,
                statusCode = context.Response.StatusCode
            });
        }
    }
}
```

---

## 📊 CONSULTAS SQL REQUERIDAS

Implementar estas consultas en los repositorios correspondientes:

```sql
-- 1. Lista de precios de todos los productos
SELECT id, codigo, nombre, precio FROM productos WHERE activo = true ORDER BY nombre;

-- 2. Productos con stock <= 5
SELECT id, codigo, nombre, stock FROM productos WHERE stock <= 5 AND activo = true;

-- 3. Clientes <= 35 años con compras entre fechas
SELECT DISTINCT c.* 
FROM clientes c
INNER JOIN facturas f ON c.id = f.cliente_id
WHERE EXTRACT(YEAR FROM AGE(c.fecha_nacimiento)) <= 35
  AND f.fecha BETWEEN '2000-02-01' AND '2000-05-25';

-- 4. Total vendido por producto en año 2000
SELECT p.id, p.nombre, SUM(df.subtotal) as total_vendido, SUM(df.cantidad) as cantidad
FROM detalles_factura df
INNER JOIN productos p ON df.producto_id = p.id
INNER JOIN facturas f ON df.factura_id = f.id
WHERE EXTRACT(YEAR FROM f.fecha) = 2000
GROUP BY p.id, p.nombre;

-- 5. Última compra y próxima estimada (lógica en código)
SELECT cliente_id, MAX(fecha) as ultima_compra
FROM facturas
GROUP BY cliente_id;
```

---

## 🎨 FRONTEND ANGULAR 17 (Estructura mínima)

```
📁 src/app/
├── 📁 core/
│   ├── services/
│   │   └── api.service.ts          # HttpClient centralizado
│   └── interceptors/
│       └── error.interceptor.ts
├── 📁 features/
│   └── 📁 facturas/
│       ├── factura-list.component.ts
│       ├── factura-form.component.ts
│       ├── factura.service.ts
│       └── factura.model.ts
├── app.config.ts
├── app.routes.ts
└── app.component.ts
```

**Componente con DevExtreme:**
```typescript
// features/facturas/factura-list.component.ts
import { Component, inject, signal } from '@angular/core';
import { DxDataGridModule, DxButtonModule } from 'devextreme-angular';
import { FacturaService } from './factura.service';

@Component({
  selector: 'app-factura-list',
  standalone: true,
  imports: [DxDataGridModule, DxButtonModule],
  template: `
    <dx-data-grid
      [dataSource]="facturas()"
      [showBorders]="true"
      [columnAutoWidth]="true">
      <dxi-column dataField="numeroFactura" caption="N° Factura"></dxi-column>
      <dxi-column dataField="clienteNombre" caption="Cliente"></dxi-column>
      <dxi-column dataField="fecha" dataType="date"></dxi-column>
      <dxi-column dataField="total" format="currency"></dxi-column>
      <dxi-column dataField="estado"></dxi-column>
      <dxi-column type="buttons">
        <dxi-button name="edit" (onClick)="onEdit($event)"></dxi-button>
      </dxi-column>
    </dx-data-grid>
  `
})
export class FacturaListComponent {
  private facturaService = inject(FacturaService);
  facturas = signal<Factura[]>([]);

  constructor() {
    this.loadFacturas();
  }

  async loadFacturas() {
    const data = await this.facturaService.getAll();
    this.facturas.set(data);
  }

  onEdit(e: any) {
    // Navegar a edición
  }
}
```

---

## ✅ ENTREGABLES

1. **📁 Scripts SQL/**
   - `01_create_tables.sql`
   - `02_insert_data.sql`
   - `03_queries.sql`

2. **📁 Backend/** - Solución .NET con proyectos Domain, Application, Infrastructure, API

3. **📁 Frontend/** - Proyecto Angular 17 con DevExtreme

4. **📄 README.md** - Instrucciones de ejecución

---

## 🚀 COMANDOS DE EJECUCIÓN

```bash
# Backend
cd src/Seven.Facturacion.API
dotnet run

# Frontend
cd frontend
npm install
ng serve
```

---

## ⚡ PRIORIDADES

1. **PRIMERO**: Scripts SQL completos y funcionales
2. **SEGUNDO**: Backend con CRUD de Facturas funcionando
3. **TERCERO**: Las 5 consultas específicas implementadas
4. **CUARTO**: Frontend con listado y formulario básico
5. **QUINTO**: Documentación

**IMPORTANTE**: Código limpio, sin dependencias innecesarias, aprovechando C# 14 y .NET 10 al máximo.
