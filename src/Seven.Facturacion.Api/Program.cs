// ============================================================================
// PROGRAM: Punto de entrada de la API
// Descripción: Configuración del host, servicios y middleware
// ============================================================================

using Seven.Facturacion.Api.Middlewares;
using Seven.Facturacion.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================================

// Agregar controladores con opciones de JSON
builder.Services.AddControllers(opciones =>
    {
        // Deshabilitar la validación estricta de records para evitar errores con atributos [property:]
        opciones.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(opciones =>
    {
        opciones.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
        opciones.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("PermitirTodo", politica =>
    {
        politica.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

// Agregar servicios de infraestructura (DbContext, Repositorios, Servicios)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

builder.Services.AgregarInfraestructura(connectionString);

// Configurar Health Checks
builder.Services.AddHealthChecks();

// ============================================================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// ============================================================================

var app = builder.Build();

// ============================================================================
// CONFIGURACIÓN DEL PIPELINE DE MIDDLEWARE
// ============================================================================

// Middleware de manejo global de excepciones (siempre primero)
app.UseMiddleware<ManejadorExcepcionesMiddleware>();

// Configuración de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "Seven Facturación API v1");
        opciones.RoutePrefix = "swagger";
    });
}

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS
app.UseCors("PermitirTodo");

// Autorización (preparado para futuras implementaciones)
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Health Check endpoint
app.MapHealthChecks("/health");

// Endpoint raíz para verificar que la API está corriendo
app.MapGet("/", () => Results.Ok(new
{
    Aplicacion = "Seven Facturación API",
    Version = "1.0.0",
    Estado = "Ejecutándose",
    Documentacion = "/swagger"
}));

// ============================================================================
// EJECUCIÓN DE LA APLICACIÓN
// ============================================================================

app.Run();

// ============================================================================
// CLASE PARCIAL PARA TESTING
// ============================================================================

/// <summary>
/// Clase parcial para permitir el uso en pruebas de integración.
/// </summary>
public partial class Program { }

