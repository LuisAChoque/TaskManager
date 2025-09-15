# Proyecto NOTESAPP

Este proyecto incluye un backend en .NET haciendo uso de Entity Framework como ORM y un frontend servido con `http-server`. También usa SQL Server en un contenedor de Docker.

## Requisitos

En el script adjunto se van a instalar las siguientes herramientas para el correcto funcionamiento del proyecto:

### 📌 **Docker**
- **Versión:** Docker 20.10
- Se instala automáticamente si no está presente.
- Se utilizara para poder ejecutar SQL Server un container.

### 📌 **SQL Server en Docker**
- Imagen utilizada: `mcr.microsoft.com/mssql/server:2022-latest`

### 📌 **.NET SDK**
- **Versión:** .NET SDK 8.0
- En Linux, se instala desde los repositorios de Microsoft.
- En macOS, se instala usando `brew`.

### 📌 **Entity Framework CLI**
- **Versión:** Última versión disponible
- Se instala con:
  ```sh
  dotnet tool install --global dotnet-ef
  ```

### 📌 **Node.js y npm**
- **Versión:** Node.js 18.x y npm 8+
- En Linux, se instala desde `nodesource`.
- En macOS, se instala con `brew`.

### 📌 **http-server**
- **Versión:** Última disponible
- Se instala globalmente con:
  ```sh
  npm install -g http-server
  ```

## Instalación y Ejecución

1. **Clonar el repositorio:**
   ```sh
   URL: -Link a este repositorio-
   ```

2. **Ejecutar el script de configuración:**
   ```sh
   chmod +x run.sh
   ./run.sh
   ```

3. **Acceder a la aplicación:**
   - **Backend:** [http://localhost:5000](http://localhost:5000)
   - **Frontend:** [http://localhost:3000](http://localhost:3000)

## Credenciales por defecto
- **Username:** `Admin`
- **Password:** `Admin`

## Notas
- Si `docker` no está en ejecución, el script intentará iniciarlo automáticamente.
- Si `.NET SDK` o `npm` no están instalados, se instalarán de forma automática.
- El proyecto fue testeado exitosamente en ubuntu 24.04 con el navegador Google Chrome

