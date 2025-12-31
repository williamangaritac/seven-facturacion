# 🚀 Guía para Subir el Proyecto a GitHub

Esta guía te ayudará a subir el Sistema de Facturación Seven a GitHub paso a paso.

## 📋 Requisitos Previos

- ✅ Cuenta de GitHub creada
- ✅ Git instalado en tu máquina
- ✅ Proyecto funcionando localmente

## 🔧 Paso 1: Configurar Git (Si es la primera vez)

```bash
# Configurar tu nombre
git config --global user.name "Tu Nombre"

# Configurar tu email (el mismo de GitHub)
git config --global user.email "tu-email@ejemplo.com"

# Verificar configuración
git config --list
```

## 📦 Paso 2: Inicializar Repositorio Local (Si no está inicializado)

```bash
# Navegar a la carpeta del proyecto
cd c:\Users\willi\OneDrive\Escritorio\digitalware

# Verificar si ya está inicializado
git status

# Si no está inicializado, ejecutar:
git init
```

## 🌐 Paso 3: Crear Repositorio en GitHub

### Opción A: Desde la Web de GitHub

1. Ve a https://github.com
2. Haz clic en el botón **"+"** (arriba a la derecha)
3. Selecciona **"New repository"**
4. Completa los datos:
   - **Repository name:** `seven-facturacion-system` (o el nombre que prefieras)
   - **Description:** `Sistema de Facturación con .NET 10, Angular 20 y PostgreSQL`
   - **Visibility:** 
     - ✅ **Public** (si quieres que sea público)
     - ✅ **Private** (si quieres que sea privado)
   - **NO marques** "Initialize this repository with a README" (ya tenemos uno)
   - **NO agregues** .gitignore ni licencia (ya los tenemos)
5. Haz clic en **"Create repository"**

### Opción B: Desde GitHub CLI (si tienes gh instalado)

```bash
gh repo create seven-facturacion-system --public --source=. --remote=origin
```

## 🔗 Paso 4: Conectar Repositorio Local con GitHub

Después de crear el repositorio en GitHub, verás una URL como:
```
https://github.com/tu-usuario/seven-facturacion-system.git
```

Ejecuta:

```bash
# Agregar el repositorio remoto
git remote add origin https://github.com/tu-usuario/seven-facturacion-system.git

# Verificar que se agregó correctamente
git remote -v
```

## 📝 Paso 5: Preparar Archivos para el Commit

```bash
# Ver estado de los archivos
git status

# Agregar todos los archivos
git add .

# Verificar qué se va a commitear
git status
```

## ✅ Paso 6: Hacer el Primer Commit

```bash
# Crear el commit inicial
git commit -m "Initial commit: Sistema de Facturación Seven con Docker"

# Verificar el commit
git log
```

## 🚀 Paso 7: Subir a GitHub

```bash
# Subir a la rama main
git push -u origin main

# Si tu rama se llama master en lugar de main:
git push -u origin master

# Si tienes error porque la rama no existe, crear y cambiar a main:
git branch -M main
git push -u origin main
```

## 🔐 Autenticación

### Si te pide usuario y contraseña:

GitHub ya no acepta contraseñas. Necesitas usar un **Personal Access Token (PAT)**.

#### Crear un Personal Access Token:

1. Ve a GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Clic en **"Generate new token"** → **"Generate new token (classic)"**
3. Dale un nombre: `Seven Facturacion Token`
4. Selecciona los permisos:
   - ✅ `repo` (todos los sub-permisos)
5. Clic en **"Generate token"**
6. **COPIA EL TOKEN** (solo se muestra una vez)

#### Usar el token:

Cuando Git te pida la contraseña, pega el token en lugar de tu contraseña.

### Alternativa: Usar SSH

```bash
# Generar clave SSH (si no tienes una)
ssh-keygen -t ed25519 -C "tu-email@ejemplo.com"

# Copiar la clave pública
cat ~/.ssh/id_ed25519.pub

# Agregar la clave a GitHub:
# GitHub → Settings → SSH and GPG keys → New SSH key
# Pega la clave pública

# Cambiar la URL del repositorio a SSH
git remote set-url origin git@github.com:tu-usuario/seven-facturacion-system.git

# Subir
git push -u origin main
```

## 📋 Paso 8: Verificar en GitHub

1. Ve a tu repositorio en GitHub
2. Verifica que todos los archivos estén ahí
3. Verifica que el README.md se vea correctamente

## 🏷️ Paso 9: Crear un Release (Opcional)

```bash
# Crear un tag
git tag -a v1.0.0 -m "Release v1.0.0 - Sistema de Facturación Seven"

# Subir el tag
git push origin v1.0.0
```

Luego en GitHub:
1. Ve a tu repositorio
2. Clic en **"Releases"**
3. Clic en **"Create a new release"**
4. Selecciona el tag `v1.0.0`
5. Título: `v1.0.0 - Sistema de Facturación Seven`
6. Descripción: Copia el contenido de EXECUTIVE_SUMMARY.md
7. Clic en **"Publish release"**

## 📝 Paso 10: Actualizar el README con la URL del Repositorio

Edita el README.md y reemplaza `<url-del-repositorio>` con la URL real:

```bash
# Ejemplo:
git clone https://github.com/tu-usuario/seven-facturacion-system.git
```

Luego:

```bash
git add README.md
git commit -m "docs: actualizar URL del repositorio en README"
git push
```

## 🎯 Comandos Útiles para el Futuro

### Hacer cambios y subirlos

```bash
# Ver cambios
git status

# Agregar archivos modificados
git add .

# Hacer commit
git commit -m "descripción de los cambios"

# Subir a GitHub
git push
```

### Actualizar desde GitHub

```bash
# Descargar cambios
git pull
```

### Ver historial

```bash
# Ver commits
git log

# Ver commits en una línea
git log --oneline

# Ver cambios de un archivo
git log -p archivo.txt
```

## ✅ Checklist Final

- [ ] Repositorio creado en GitHub
- [ ] Repositorio local conectado con GitHub
- [ ] Primer commit realizado
- [ ] Código subido a GitHub
- [ ] README.md se ve correctamente
- [ ] .gitignore funciona (no se subieron archivos innecesarios)
- [ ] URL del repositorio actualizada en README.md
- [ ] Release creado (opcional)

## 🎉 ¡Listo!

Tu proyecto ya está en GitHub y cualquier persona puede clonarlo con:

```bash
git clone https://github.com/tu-usuario/seven-facturacion-system.git
cd seven-facturacion-system
.\start.ps1  # Windows
./start.sh   # Linux/Mac
```

## 📞 Problemas Comunes

### "fatal: remote origin already exists"

```bash
git remote remove origin
git remote add origin https://github.com/tu-usuario/seven-facturacion-system.git
```

### "error: failed to push some refs"

```bash
# Forzar push (solo si estás seguro)
git push -f origin main
```

### "Permission denied (publickey)"

Necesitas configurar SSH. Sigue la sección "Alternativa: Usar SSH" arriba.

---

**¡Tu proyecto ya está en GitHub!** 🎉

