# 🔐 Instrucciones de Autenticación - Seven Facturación

## Paso 1: Crear tabla de usuarios en PostgreSQL

### Opción A: Usando pgAdmin o cualquier cliente PostgreSQL

1. Abre pgAdmin o tu cliente PostgreSQL favorito
2. Conéctate a la base de datos `facturacion_db`
3. Ejecuta el siguiente script:

```sql
CREATE TABLE IF NOT EXISTS facturacion.usuarios (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

INSERT INTO facturacion.usuarios (username, password_hash, activo)
VALUES ('admin', '$2a$11$8K1p/a0dL3LHR/nHkfuBiOCEZZ8QeKhQkrXfzIU4OqgnE0.jjKZ6e', TRUE)
ON CONFLICT (username) DO NOTHING;

CREATE INDEX IF NOT EXISTS idx_usuarios_username ON facturacion.usuarios(username);
```

### Opción B: Usando PowerShell (si tienes psql en el PATH)

```powershell
cd Scripts
.\ejecutar_sql_usuarios.ps1
```

## Paso 2: Levantar el Backend (.NET)

```powershell
cd src\Seven.Facturacion.Api
dotnet run
```

El backend estará disponible en: **https://localhost:7001**

## Paso 3: Levantar el Frontend (Angular)

```powershell
cd frontend_angular
npm run dev
```

El frontend estará disponible en: **http://localhost:4200**

## Paso 4: Probar la autenticación

1. Abre el navegador en **http://localhost:4200**
2. Deberías ver la pantalla de login
3. Ingresa las credenciales:
   - **Usuario:** `admin`
   - **Contraseña:** `admin123`
4. Haz clic en "Iniciar Sesión"
5. Si todo está correcto, serás redirigido a la aplicación

## Verificación

### Verificar que la tabla se creó correctamente

```sql
SELECT * FROM facturacion.usuarios;
```

Deberías ver el usuario `admin` con su password hasheado.

### Verificar el endpoint de login

Puedes probar el endpoint directamente con curl:

```powershell
curl -X POST https://localhost:7001/api/auth/login `
  -H "Content-Type: application/json" `
  -d '{"username":"admin","password":"admin123"}' `
  -k
```

Deberías recibir una respuesta con el token:

```json
{
  "token": "guid-generado",
  "username": "admin"
}
```

## Rutas Protegidas

Las siguientes rutas están protegidas y requieren autenticación:
- `/api/clientes/*`
- `/api/productos/*`
- `/api/facturas/*`

Las siguientes rutas son públicas:
- `/api/auth/login` - Login
- `/api/auth/validate` - Validar token
- `/swagger` - Documentación
- `/health` - Health check
- `/` - Endpoint raíz

## Troubleshooting

### Error: "No se puede conectar con el servidor"

- Verifica que el backend esté corriendo en https://localhost:7001
- Verifica que PostgreSQL esté corriendo

### Error: "Credenciales inválidas"

- Verifica que ejecutaste el script SQL correctamente
- Verifica que el usuario `admin` existe en la tabla `usuarios`

### Error: "No autorizado" en las peticiones

- Verifica que el token se esté guardando en localStorage
- Abre las DevTools del navegador → Application → Local Storage
- Deberías ver `auth_token` y `auth_username`

### El frontend no carga

- Verifica que Angular esté corriendo en http://localhost:4200
- Revisa la consola del navegador para ver errores

## Notas Técnicas

- **Hash de contraseña:** Se usa BCrypt con factor de trabajo 11
- **Tokens:** Se generan GUIDs y se almacenan en memoria (para producción usar Redis/DB)
- **Middleware:** El `AuthMiddleware` valida el token en todas las peticiones a `/api/*` excepto `/api/auth/*`
- **Guard:** El `authGuard` protege las rutas del frontend
- **Interceptor:** El `apiInterceptor` agrega automáticamente el token a todas las peticiones

