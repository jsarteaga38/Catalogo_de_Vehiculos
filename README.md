# Catalogo de Vehiculos

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

Sistema de escritorio desarrollado en **C# con Windows Forms** que permite registrar, consultar, filtrar y eliminar vehículos de distintas categorías (Automóvil, Motocicleta, Camión), con cálculo automático de depreciación y costo de mantenimiento. Integra base de datos SQL Server y exportación a Excel.

---

## 🏗️ Arquitectura

El proyecto implementa **Clean Architecture** dividida en 4 capas:

    Catalogo_de_Vehiculos/
    │
    ├── Domain/                  ← Entidades e interfaces (núcleo del sistema)
    │   ├── Entities/            ← Vehiculo, Automovil, Camion, Motocicleta
    │   ├── Interfaces/          ← IVehiculoRepository, ICatalogoObserver, ICatalogoView
    │   └── Factories/           ← VehiculoFactory (Factory Method)
    │
    ├── Application/             ← Lógica de negocio
    │   └── Services/            ← CatalogoService (Observer), ExportService
    │
    ├── Infrastructure/          ← Acceso a datos
    │   └── Repositories/        ← VehiculoRepository, UnitOfWork, repos específicos
    │
    └── Presentation/            ← Interfaz gráfica
        ├── Forms/               ← FormPrincipal, FormDashboard
        └── Presenters/          ← CatalogoPresenter (MVP)

---

## 🎨 Patrones de Diseño Implementados

### 1. Factory Method
Centraliza la creación de vehículos en `VehiculoFactory`. El formulario no instancia directamente ningún objeto, solo llama a la fábrica con el tipo deseado.

### 2. Repository + Unit of Work
Abstrae el acceso a SQL Server. Cada tipo de vehículo tiene su repositorio específico que hereda de `VehiculoRepository`. `UnitOfWork` centraliza todos los repositorios bajo una sola unidad de trabajo.

### 3. Observer
`CatalogoService` implementa `ICatalogoObservable`. `FormPrincipal` se suscribe como observador — cada vez que se agrega o elimina un vehículo, la UI se actualiza automáticamente sin llamadas manuales.

---

## ✅ Principios SOLID Aplicados

| Principio | Aplicación en el proyecto |
|-----------|--------------------------|
| **S** — Single Responsibility | `VehiculoFactory` solo crea, `ExportService` solo exporta, `CatalogoService` solo coordina |
| **O** — Open/Closed | Agregar un nuevo tipo de vehículo solo requiere extender `VehiculoFactory`, sin tocar código existente |
| **L** — Liskov Substitution | `Automovil`, `Camion` y `Motocicleta` son intercambiables donde se espera `Vehiculo` |
| **I** — Interface Segregation | `IAutomovilRepository`, `ICamionRepository` e `IMotocicletaRepository` no obligan a implementar métodos ajenos |
| **D** — Dependency Inversion | `CatalogoPresenter` depende de `ICatalogoView`, no de `FormPrincipal` concreto |

---

## ✨ Funcionalidades

- ✅ Registro de Automóviles, Camiones y Motocicletas
- ✅ Cálculo automático de depreciación y mantenimiento
- ✅ Persistencia en SQL Server LocalDB
- ✅ Filtros avanzados por tipo, marca y año
- ✅ Búsqueda por marca
- ✅ Eliminación con confirmación
- ✅ Dashboard con estadísticas y gráfica de barras
- ✅ Exportación a Excel con formato profesional
- ✅ Validaciones visuales en tiempo real

---

## 🛠️ Tecnologías

| Tecnología | Uso |
|-----------|-----|
| C# / .NET 10 | Lenguaje y framework principal |
| Windows Forms | Interfaz gráfica |
| SQL Server LocalDB | Base de datos |
| Microsoft.Data.SqlClient | Acceso a datos |
| ClosedXML 0.105.0 | Exportación a Excel |

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
        Caracteristica NVARCHAR(100)
    );

---

## 🚀 Cómo ejecutar

1. Clonar el repositorio:

        git clone https://github.com/jsarteaga38/Catalogo_de_Vehiculos.git

2. Abrir en Visual Studio 2022 o superior

3. Crear la base de datos ejecutando el script SQL anterior en SQL Server LocalDB

4. Ejecutar con `dotnet run` o presionar ▶ en Visual Studio

---

## 📁 Documentación

La carpeta `/docs` contiene:
- Diagrama de clases
- Diagrama de estructura del proyecto
- Diagrama de flujo de la interfaz
- Justificación de patrones de diseño
- Mapeo de principios SOLID
- Manual de usuario

---

## 📜 Historial de ramas

| Rama | Contenido |
|------|-----------|
| `master` | Código principal integrado |
| `3-Repository-Unit-of-Work` | Patrón Repository y Unit of Work |
| `4-Patrón-Observer` | Patrón Observer |
| `5-CatalogoService-Refactor` | Refactorización del servicio |
| `6-Patrón-MVP` | Patrón Model-View-Presenter |
| `7-UI-Filtros` | UI mejorada con filtros |
| `8-Dashboard` | Dashboard con estadísticas |
| `9-Exportar-Excel` | Exportación a Excel |
| `10-Validaciones` | Validaciones visuales |
| `11-Documentacion` | Documentación del proyecto |
