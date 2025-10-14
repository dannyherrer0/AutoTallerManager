# 🚗 AutoTallerManager

Sistema backend RESTful para la gestión integral de un taller automotriz moderno, desarrollado con ASP.NET Core y arquitectura hexagonal.

## 📋 Descripción

AutoTallerManager centraliza y automatiza procesos clave como:
- Gestión de clientes y vehículos
- Órdenes de servicio y seguimiento
- Control de inventario de repuestos
- Facturación automática
- Sistema de roles y permisos (Admin, Mecánico, Recepcionista)

## 🏗️ Arquitectura

El proyecto sigue el patrón de **Arquitectura Hexagonal** (Ports & Adapters) con 4 capas:

```
AutoTallerManager/
├── Domain/          # Entidades y lógica de negocio
├── Application/     # DTOs, casos de uso y servicios
├── Infrastructure/  # EF Core, Repositories, Unit of Work
└── Api/            # Controladores REST y configuración
```

## 🛠️ Tecnologías

- **Framework:** ASP.NET Core 8.0
- **ORM:** Entity Framework Core
- **Base de datos:** MySQL / PostgreSQL / SQL Server
- **Autenticación:** JWT (JSON Web Tokens)
- **Documentación:** Swagger / OpenAPI
- **Mapeo:** AutoMapper
- **Rate Limiting:** AspNetCoreRateLimit

## 📦 Requisitos Previos

- .NET 8.0 SDK
- MySQL 8.0+ (o PostgreSQL/SQL Server)
- Visual Studio 2022 / VS Code / Rider

## 🚀 Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/TU_USUARIO/AutoTallerManager.git
cd AutoTallerManager
```

### 2. Restaurar paquetes NuGet

```bash
dotnet restore
```

### 3. Configurar la cadena de conexión

Edita `Api/appsettings.json` y configura tu conexión a la base de datos:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=autotaller;User=root;Password=tu_password;"
  }
}
```

### 4. Aplicar migraciones

```bash
cd Api
dotnet ef database update
```

### 5. Ejecutar el proyecto

```bash
dotnet run
```

La API estará disponible en: `https://localhost:7XXX` (el puerto se muestra en consola)

## 📚 Documentación API

Accede a la documentación interactiva Swagger en:
```
https://localhost:7XXX/swagger
```

## 🔐 Roles y Permisos

- **Admin:** Acceso total al sistema
- **Mecánico:** Actualización de órdenes y generación de facturas
- **Recepcionista:** Creación de órdenes y consulta de clientes

## 📝 Endpoints Principales

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar usuario

### Clientes
- `GET /api/clientes` - Listar clientes (paginado)
- `POST /api/clientes` - Crear cliente
- `GET /api/clientes/{id}` - Obtener cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente

### Órdenes de Servicio
- `GET /api/ordenes` - Listar órdenes
- `POST /api/ordenes` - Crear orden
- `PUT /api/ordenes/{id}` - Actualizar orden
- `POST /api/ordenes/{id}/cerrar` - Cerrar orden y generar factura

### Repuestos
- `GET /api/repuestos` - Listar repuestos
- `POST /api/repuestos` - Agregar repuesto
- `PUT /api/repuestos/{id}/stock` - Actualizar stock

## 👥 Contribuidores

- DANIELA SOFIA HERRERA ROJAS
- SANTIAGO VALDERRAMA LAITON
- DARWIN FELIPE ARENAS CARVAJAL

⚙️ Desarrollado con ASP.NET Core