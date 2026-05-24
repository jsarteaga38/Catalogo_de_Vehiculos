# 📁 Diagrama de Estructura del Proyecto

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## Estructura de Carpetas

    Catalogo_de_Vehiculos/                    ← Raíz del repositorio
    │
    ├── README.md                             ← Descripción general del proyecto
    │
    ├── docs/                                 ← Documentación del proyecto
    │   ├── diagrama-clases.md
    │   ├── estructura-proyecto.md
    │   ├── flujo-interfaz.md
    │   ├── patrones-de-diseno.md
    │   ├── principios-solid.md
    │   └── manual-usuario.md
    │
    └── Catalogo de Vehiculo/                 ← Proyecto C# principal
        │
        ├── Domain/                           ← Capa de dominio (núcleo)
        │   ├── Entities/                     ← Modelos del negocio
        │   │   ├── Vehiculo.cs               ← Clase base abstracta
        │   │   ├── Automovil.cs              ← Depreciación 10%, mantenimiento 5%
        │   │   ├── Camion.cs                 ← Depreciación 12%, mantenimiento 8%
        │   │   └── Motocicleta.cs            ← Depreciación 8%, mantenimiento 3%
        │   │
        │   ├── Interfaces/                   ← Contratos del sistema
        │   │   ├── IVehiculoRepository.cs    ← CRUD genérico
        │   │   ├── IAutomovilRepository.cs   ← Búsqueda por puertas
        │   │   ├── ICamionRepository.cs      ← Búsqueda por peso
        │   │   ├── IMotocicletaRepository.cs ← Búsqueda por tipo
        │   │   ├── IUnitOfWork.cs            ← Unidad de trabajo
        │   │   ├── ICatalogoObserver.cs      ← Patrón Observer
        │   │   ├── ICatalogoObservable.cs    ← Patrón Observer
        │   │   └── ICatalogoView.cs          ← Patrón MVP
        │   │
        │   └── Factories/                    ← Patrón Factory Method
        │       └── VehiculoFactory.cs        ← Crea Automovil/Camion/Motocicleta
        │
        ├── Application/                      ← Capa de aplicación (lógica de negocio)
        │   └── Services/
        │       ├── CatalogoService.cs        ← Coordinador + Observable (Observer)
        │       └── ExportService.cs          ← Exportación a Excel (ClosedXML)
        │
        ├── Infrastructure/                   ← Capa de infraestructura (datos)
        │   └── Repositories/
        │       ├── VehiculoRepository.cs     ← CRUD SQL Server base
        │       ├── AutomovilRepository.cs    ← Repositorio específico
        │       ├── CamionRepository.cs       ← Repositorio específico
        │       ├── MotocicletaRepository.cs  ← Repositorio específico
        │       └── UnitOfWork.cs             ← Unidad de trabajo
        │
        ├── Presentation/                     ← Capa de presentación (UI)
        │   ├── Forms/
        │   │   ├── FormPrincipal.cs          ← Ventana principal (View + Observer)
        │   │   └── FormDashboard.cs          ← Dashboard estadísticas
        │   └── Presenters/
        │       └── CatalogoPresenter.cs      ← Presenter (MVP)
        │
        ├── Program.cs                        ← Punto de entrada
        ├── App.config                        ← Configuración
        └── Catalogo de Vehiculo.csproj       ← Archivo de proyecto

---

## Flujo de Dependencias entre Capas

    +----------------+
    |  Presentation  |  ← FormPrincipal, FormDashboard, CatalogoPresenter
    +----------------+
           |
           | depende de
           ↓
    +----------------+
    |  Application   |  ← CatalogoService, ExportService
    +----------------+
           |
           | depende de
           ↓
    +----------------+
    |    Domain      |  ← Entities, Interfaces, Factories
    +----------------+
           △
           |
           | implementa
           |
    +----------------+
    | Infrastructure |  ← Repositories, UnitOfWork
    +----------------+

    REGLA: Las capas externas dependen de las internas.
           Las capas internas NO conocen las externas.

---

## Tecnologías por Capa

    Domain          → C# puro, sin dependencias externas
    Application     → C#, ClosedXML (solo ExportService)
    Infrastructure  → Microsoft.Data.SqlClient, SQL Server LocalDB
    Presentation    → Windows Forms, System.Drawing

---

## Archivos de Configuración

    App.config              ← Configuración de la aplicación
    .gitignore              ← Archivos ignorados por Git
    .gitattributes          ← Configuración de líneas de texto Git
    Catalogo de Vehiculo.csproj ← Dependencias NuGet:
                                   - Microsoft.Data.SqlClient
                                   - ClosedXML 0.105.0