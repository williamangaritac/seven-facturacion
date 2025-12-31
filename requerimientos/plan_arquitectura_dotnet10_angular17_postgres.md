# Plan de arquitectura (2 estilos) — .NET 10 (C# 14) + Angular 17 + PostgreSQL

Este documento resume **dos propuestas de arquitectura** para implementar la prueba técnica de facturación (modelo de datos + API REST + frontend Angular).  
El alcance mínimo esperado incluye: **listado/creación/edición de facturas**, consumo de servicios REST y buenas prácticas (SOLID, calidad, arquitectura).  

---

## Objetivos y no‑objetivos

**Objetivos**
- Implementar una app de facturación simple pero con estructura profesional.
- Mantener **separación de responsabilidades**, testabilidad y código limpio.
- Diseñar el modelo relacional para soportar consultas solicitadas (precios, inventario, clientes por edad/fechas, totales por producto, última compra y próxima estimada).

**No‑objetivos**
- UI perfecta (se prioriza integración/estructura).
- Autenticación completa (se deja **planteada la estructura** como mejora opcional).

---

## Stack propuesto

- **Backend:** .NET 10, C# 14, ASP.NET Core Web API
- **DB:** PostgreSQL (EF Core o ADO.NET) + migraciones
- **Frontend:** Angular 17 + DevExtreme (DataGrid, Form, Button)
- **Calidad:** xUnit, FluentAssertions, FluentValidation, OpenAPI/Swagger, Serilog, HealthChecks

---

# Estilo 1 — Clean Architecture (Layered) + CQRS ligero (monolito modular)

## Cuándo usarlo
- Cuando se quiere una base “enterprise” clara por capas y fácil de mantener.
- Si el equipo valora límites explícitos entre Dominio, Aplicación e Infraestructura.

## Estructura de solución (recomendada)
```
/src
  Billing.Api               -> Controllers, DTOs, middleware, auth plumbing
  Billing.Application       -> Casos de uso (Commands/Queries), validaciones, contratos
  Billing.Domain            -> Entidades, ValueObjects, reglas, interfaces del dominio
  Billing.Infrastructure    -> EF Core, Repositorios, Migrations, Integraciones
/tests
  Billing.UnitTests
  Billing.IntegrationTests
```

## Flujo (CQRS simple)
- **Queries**: lectura (ej. `GetInvoices`, `GetProductPriceList`, `GetLowStockProducts`).
- **Commands**: escritura (ej. `CreateInvoice`, `UpdateInvoice`).
- Mediator (MediatR) opcional: ayuda a mantener handlers pequeños y testeables.

## Persistencia (PostgreSQL)
- EF Core con Migrations (ideal para la prueba).
- Alternativa ADO.NET (para demostrar performance), dejando repositorios como contratos.

## Manejo de errores
- Middleware global: mapear excepciones a ProblemDetails (`RFC7807`).
- Validación: `FluentValidation` antes de ejecutar casos de uso.

## Endpoints mínimos (REST)
- `GET /api/invoices` (listado con paginación opcional)
- `GET /api/invoices/{id}`
- `POST /api/invoices`
- `PUT /api/invoices/{id}`
- `GET /api/products/prices`
- `GET /api/products/low-stock?threshold=5`
- `GET /api/reports/clients-age?maxAge=35&from=2000-02-01&to=2000-05-25`
- `GET /api/reports/product-sales?year=2000`
- `GET /api/reports/client-next-purchase/{clientId}`

## Modelo de datos (mínimo 4 tablas)
- `clients`
- `products`
- `invoices`
- `invoice_items`

**Notas de diseño**
- `invoice_items` con (invoice_id, product_id, quantity, unit_price, line_total)
- Guardar `unit_price` en el detalle para “precio histórico”.
- Índices: `products(sku)`, `invoice_items(product_id)`, `invoices(client_id, invoice_date)`.

## Angular 17 (alto nivel)
- Módulos/Features:
  - `InvoicesModule` (list + editor)
  - `ProductsModule` (consulta de precios/stock)
- Servicios:
  - `InvoicesApiService`
  - `ProductsApiService`
- UI DevExtreme:
  - `dx-data-grid` para listado
  - `dx-form` para creación/edición con `dx-button`

## Ventajas / Riesgos
**Ventajas**
- Muy claro para evaluadores: capas + DTOs + casos de uso.
- Fácil de testear (handlers puros).
- Base sólida para crecer.

**Riesgos**
- Puede sentirse “pesado” si el problema es pequeño (mitigable manteniendo CQRS ligero).

---

# Estilo 2 — Vertical Slice Architecture (Feature‑First) + Minimal APIs + BFF (monolito preparado para escalar)

## Cuándo usarlo
- Cuando se prioriza velocidad y cohesión por funcionalidad.
- Cuando se desea un diseño **por features** que puede evolucionar a microservicios.

## Estructura (por funcionalidades / slices)
```
/src
  Billing.Api
    /Features
      /Invoices
        GetInvoices.cs
        GetInvoiceById.cs
        CreateInvoice.cs
        UpdateInvoice.cs
        InvoiceDtos.cs
      /Products
        GetPriceList.cs
        GetLowStock.cs
      /Reports
        ClientPurchases.cs
        ProductSales.cs
        NextPurchaseEstimate.cs
    /Infrastructure
      DbContext.cs
      Migrations/
      Repositories/
    Program.cs   -> Minimal APIs + endpoints por feature
/tests
  Billing.Tests
```

## Patrón de endpoints
- Cada “slice” contiene:
  - Request/Response DTO
  - Validator
  - Handler/Service
  - Endpoint map (Minimal API)
- Se reduce boilerplate y se maximiza cohesión.

## BFF (Backend for Frontend) opcional (recomendado)
- El backend expone endpoints adaptados a las pantallas Angular.
- Evita “chatty APIs” y simplifica el frontend.

## Persistencia
- EF Core con repositorios mínimos **solo donde agregue valor**.
- Acceso directo con `DbContext` en handlers (válido y común en VSA) para reducir capas.

## Estimación de próxima compra (lógica simple)
- Calcular frecuencia con:
  - Últimas N compras (ej. 3) -> promedio de días entre compras
  - Próxima = última_compra + promedio_días  
- Documentar la lógica en el endpoint o README.

## Angular 17 (feature-first)
- `features/invoices/`:
  - `invoices-list.component`
  - `invoice-editor.component`
- `shared/`:
  - interceptores HTTP, manejo de errores, loaders
- Estado:
  - Simple (signals + services) o NgRx si se justifica (probablemente no).

## Ventajas / Riesgos
**Ventajas**
- Menos fricción, alto foco en entregar rápido.
- Muy fácil de navegar por “casos de uso”.
- Se presta para escalar a microservicios (extraer un feature como servicio).

**Riesgos**
- Si el equipo está acostumbrado a capas, requiere disciplina para no mezclar responsabilidades.
- Definir convenciones desde el inicio (naming, validación, errores, logging).

---

# Plan de implementación (aplicable a ambos estilos)

## 1) Base de datos (PostgreSQL)
1. Definir entidades y relaciones (clients, products, invoices, invoice_items).
2. Script de creación + script de datos de prueba.
3. Script independiente con consultas solicitadas.
4. (Si se usa EF) crear migrations equivalentes al script.

## 2) Backend (.NET 10)
1. Estructura del proyecto (Estilo 1 o 2).
2. Configurar EF Core + Npgsql, migraciones, logging.
3. Implementar endpoints mínimos (CRUD facturas + reportes/consultas).
4. Manejo de errores (ProblemDetails) y validación.
5. Tests:
   - Unit tests de lógica (estimación próxima compra).
   - Integration tests básicos (endpoint + DB test).

## 3) Frontend (Angular 17 + DevExtreme)
1. Proyecto Angular 17 (standalone o módulos; preferible standalone + feature folders).
2. Servicios HTTP tipados.
3. Pantallas:
   - Listado de facturas (dx-data-grid)
   - Crear/editar factura (dx-form + grid de items)
4. Manejo de errores y loading.

## 4) Entregables
- Scripts SQL (creación + inserts + consultas)
- Proyecto backend
- Proyecto frontend
- Documento breve de arquitectura + capturas (puede ser este .md convertido a PDF)

---

# Recomendación final

- Si quieres **maximizar claridad para evaluadores** y “arquitectura clásica”: **Estilo 1 (Clean Architecture + CQRS ligero)**.
- Si quieres **entregar rápido** y demostrar enfoque moderno por features: **Estilo 2 (Vertical Slice + Minimal APIs + BFF)**.
