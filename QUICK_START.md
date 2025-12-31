# ⚡ Inicio Rápido - Sistema de Facturación Seven

Esta guía te permitirá tener el proyecto funcionando en **menos de 5 minutos**.

## 🎯 Objetivo

Levantar el sistema completo de facturación con:
- ✅ Base de datos PostgreSQL
- ✅ Backend API .NET 10
- ✅ Frontend Angular 20
- ✅ Datos de prueba precargados
- ✅ Usuario admin configurado

## 📋 Requisitos

Solo necesitas:
- **Docker Desktop** instalado y corriendo
- **Git** para clonar el repositorio

## 🚀 Pasos

### 1️⃣ Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd digitalware
```

### 2️⃣ Verificar Requisitos (Opcional)

**Windows:**
```powershell
.\check-requirements.ps1
```

**Linux/Mac:**
```bash
chmod +x check-requirements.sh
./check-requirements.sh
```

### 3️⃣ Levantar el Proyecto

**Windows:**
```powershell
.\start.ps1
```

**Linux/Mac:**
```bash
chmod +x start.sh
./start.sh
```

### 4️⃣ Verificar que Todo Funcione (Opcional)

**Windows:**
```powershell
.\test-docker.ps1
```

**Linux/Mac:**
```bash
chmod +x test-docker.sh
./test-docker.sh
```

### 5️⃣ Acceder a la Aplicación

Abre tu navegador en: **http://localhost:4200**

**Credenciales:**
- **Usuario:** `admin`
- **Contraseña:** `admin123`

## 🎉 ¡Listo!

Ya tienes el sistema funcionando. Ahora puedes:

1. ✅ **Gestionar Clientes** - Crear, editar, eliminar clientes
2. ✅ **Gestionar Productos** - Administrar catálogo de productos
3. ✅ **Crear Facturas** - Generar facturas con múltiples productos
4. ✅ **Ver Reportes** - Consultar facturas y estadísticas

## 🔧 Comandos Útiles

### Ver Logs
```bash
docker-compose logs -f
```

### Reiniciar Servicios
```bash
docker-compose restart
```

### Detener Todo
```bash
docker-compose down
```

### Resetear Base de Datos
```bash
docker-compose down -v
docker-compose up -d
```

## 📊 URLs de Acceso

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **Frontend** | http://localhost:4200 | Aplicación web |
| **Backend** | http://localhost:5000/api | API REST |
| **Swagger** | http://localhost:5000/swagger | Documentación API |

## 🐛 Problemas Comunes

### "Cannot connect to Docker daemon"
**Solución:** Inicia Docker Desktop

### "Port already in use"
**Solución:** Detén el servicio que esté usando el puerto:
```bash
# Ver qué está usando el puerto
netstat -ano | findstr :4200  # Windows
lsof -i :4200                 # Linux/Mac

# Cambiar puerto en docker-compose.yml si es necesario
```

### "Credenciales inválidas"
**Solución:** Resetea la base de datos:
```bash
docker-compose down -v
docker-compose up -d
```

## 📚 Más Información

- **[README.md](README.md)** - Documentación completa
- **[DOCKER_SETUP.md](DOCKER_SETUP.md)** - Guía detallada de Docker
- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Guía para desarrolladores
- **[DOCKER_FILES_SUMMARY.md](DOCKER_FILES_SUMMARY.md)** - Resumen de archivos Docker

## 💡 Consejos

1. **Primera vez:** La primera ejecución puede tardar 5-10 minutos mientras descarga las imágenes base.

2. **Desarrollo:** Si vas a desarrollar, considera usar el modo local en lugar de Docker para tener hot reload.

3. **Producción:** Para producción, configura variables de entorno seguras en `.env`.

4. **Backup:** Haz backup de la base de datos regularmente:
   ```bash
   docker-compose exec -T postgres pg_dump -U postgres seven_facturacion_dev > backup.sql
   ```

## 🎓 Próximos Pasos

1. ✅ Explora la aplicación
2. ✅ Crea algunos clientes de prueba
3. ✅ Agrega productos al catálogo
4. ✅ Genera tu primera factura
5. ✅ Revisa la documentación de la API en Swagger
6. ✅ Lee CONTRIBUTING.md si quieres contribuir

## 🆘 Soporte

Si tienes problemas:

1. Revisa los logs: `docker-compose logs -f`
2. Consulta [DOCKER_SETUP.md](DOCKER_SETUP.md)
3. Verifica que Docker esté corriendo
4. Asegúrate de tener los puertos libres

---

**¡Disfruta usando el Sistema de Facturación Seven!** 🎉

