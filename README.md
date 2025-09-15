# Proyecto TaskManager

## 📖 Descripción
**TaskManager** es una aplicación web para gestionar tareas.  
Permite crear, listar, actualizar y eliminar tareas, asociándolas a usuarios y categorías (To Do, In Progress, Done).  

Este proyecto está preparado para correr en Linux y se desarrolló demostrando el uso de un **backend en .NET** y un **frontend en React con TailwindCSS**.

---

## 🛠️ Tecnologías utilizadas

### 🔹 Backend
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server (montado en un contenedor docker)
- Swagger (se pueden testear los distintos endpoints)

### 🔹 Frontend
- React 18 + Vite
- TailwindCSS 3
- Axios (para consumir la API)

---

## ⚡ Requisitos previos
Para ejecutar el proyecto necesitás tener instalado:

- [Node.js 18+](https://nodejs.org/) (incluye `npm`)
- [.NET 8 SDK](https://dotnet.microsoft.com/download)


---

## ▶️ Ejecución del proyecto

Gracias al script de configuración, solo tenés que clonar el repo y dos veces ejecutar el script run.sh:

```bash
git clone https://github.com/tu-usuario/TaskManager.git
cd TaskManager
./run.sh

