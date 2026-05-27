# 🚗 Catálogo de Vehículos

![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![C#](https://img.shields.io/badge/C%23-WinForms-blue)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-red)
![Architecture](https://img.shields.io/badge/Architecture-Clean-green)

## 📋 Información del Proyecto

| Campo | Detalle |
|-------|---------|
| **Materia** | Herramientas de Programación |
| **Profesor** | Oscar Leonel Sánchez Conde |
| **Institución** | Institución Universitaria Pascual Bravo |
| **Integrantes** | Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya |

---

## 📌 Descripción

Sistema de escritorio desarrollado en C# con Windows Forms que permite a una empresa registrar, consultar, filtrar y gestionar su flota de vehículos. Incluye módulos de empleados, ventas, mantenimientos, dashboard estadístico y exportación a Excel.

---

## ✨ Funcionalidades

- Registro de Automóviles, Camiones y Motocicletas
- Cálculo automático de depreciación (máximo 15 años) y mantenimiento
- Estado automático del vehículo: Disponible, Vendido, En Mantenimiento
- Gestión de Empleados con CRUD completo
- Registro de Ventas con cliente y empleado vendedor
- Registro de Mantenimientos con próximo mantenimiento programado
- Filtros avanzados por tipo, marca y año
- Dashboard con estadísticas visuales y gráficas de barras
- Exportación a Excel con formato profesional
- Validaciones visuales en tiempo real

---

## 🗄️ Base de Datos

    CREATE TABLE Vehiculos (
        Id             INT IDENTITY PRIMARY KEY,
        Tipo           NVARCHAR(50),
        Marca          NVARCHAR(100),
        Modelo         NVARCHAR(100),
        Año            INT,
        Precio         FLOAT,
        Color          NVARCHAR(50),
        Caracteristica NVARCHAR(100),
        Estado         NVARCHAR(20) DEFAULT 'Disponible'
    );

    CREATE TABLE Empleados (
        Id       INT IDENTITY PRIMARY KEY,
        Nombre   NVARCHAR(100),
        Cedula   NVARCHAR(20),
        Cargo    NVARCHAR(50),
        Contacto NVARCHAR(100)
    );

    CREATE TABLE Clientes (
        Id       INT IDENTITY PRIMARY KEY,
        Nombre   NVARCHAR(100),
        Cedula   NVARCHAR(20),
        Contacto NVARCHAR(100)
    );

    CREATE TABLE Ventas (
        Id          INT IDENTITY PRIMARY KEY,
        VehiculoId  INT REFERENCES Vehiculos(Id),
        EmpleadoId  INT REFERENCES Empleados(Id),
        ClienteId   INT REFERENCES Clientes(Id),
        PrecioVenta FLOAT,
        Fecha       DATETIME DEFAULT GETDATE()
    );

    CREATE TABLE Mantenimientos (
        Id                   INT IDENTITY PRIMARY KEY,
        VehiculoId           INT REFERENCES Vehiculos(Id),
        TipoMantenimiento    NVARCHAR(100),
        Costo                FLOAT,
        Fecha                DATETIME DEFAULT GETDATE(),
        ProximoMantenimiento DATETIME
    );

---

## 🏗️ Arquitectura — Clean Architecture

    Domain/                  <- Entidades, interfaces, factories
    Application/             <- Servicios de negocio
    Infrastructure/          <- Repositorios SQL Server
    Presentation/            <- Formularios WinForms

---

## 🎨 Patrones de Diseño

| Patrón | Aplicación |
|--------|-----------|
| Factory Method | VehiculoFactory centraliza creación de vehículos |
| Repository + Unit of Work | Abstrae acceso a SQL Server |
| Observer | CatalogoService notifica cambios automáticamente |
| MVP | CatalogoPresenter separa UI de lógica |

---

## 🛠️ Tecnologías

| Tecnología | Uso |
|-----------|-----|
| C# / .NET 10 | Lenguaje y framework |
| Windows Forms | Interfaz gráfica |
| SQL Server LocalDB | Base de datos |
| Microsoft.Data.SqlClient | Acceso a datos |
| ClosedXML 0.105.0 | Exportación Excel |
| LINQ | Filtros en memoria |
| GDI+ | Gráficas del Dashboard |

---

## 🚀 Cómo ejecutar

    git clone https://github.com/jsarteaga38/Catalogo_de_Vehiculos.git
    cd "Catalogo de Vehiculo"
    dotnet run

---

## 📁 Documentación

Ver carpeta `/docs` para diagramas, patrones, SOLID, arquitectura y manual de usuario.

---

## 📜 Ramas

| Rama | Contenido |
|------|-----------|
| master | Código final integrado |
| 14-Correccion-depreciacion | Depreciación lineal máximo 15 años |
| 15-Nuevas-Entidades | Empleado, Cliente, Venta, Mantenimiento |
| 16-Nuevas-Interfaces | Interfaces y repositorios nuevos |
| 17-Formularios-UI | FormEmpleados, FormVentas, FormMantenimientos |
| 21-Dashboard-Actualizado | Dashboard con estadísticas de estado |
| 22-Documentacion-Actualizada | Documentación completa actualizada |
