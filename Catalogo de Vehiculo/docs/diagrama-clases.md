# 📊 Diagrama de Clases

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## Diagrama

    +--------------------------------------------------+
    |                   <<abstract>>                   |
    |                    Vehiculo                      |
    +--------------------------------------------------+
    | + Marca: string                                  |
    | + Modelo: string                                 |
    | + Año: int                                       |
    | + Precio: double                                 |
    | + Color: string                                  |
    +--------------------------------------------------+
    | + CalcularDepreciacion(): double <<abstract>>    |
    | + CalcularCostoMantenimiento(): double <<abstract>>|
    | + ToString(): string                             |
    +--------------------------------------------------+
              △               △               △
              |               |               |
    +---------+----+  +-------+------+  +-----+--------+
    |  Automovil   |  |    Camion    |  | Motocicleta  |
    +--------------+  +--------------+  +--------------+
    | +cantidadPla-|  | + peso:      |  | +tipoMoto-   |
    |  sas: int    |  |   double     |  |  sicleta:    |
    +--------------+  +--------------+  |  string      |
    | Depreciacion:|  | Depreciacion:|  +--------------+
    |  10% x año  |  |  12% x año  |  | Depreciacion:|
    | Mantenimiento|  | Mantenimiento|  |  8% x año   |
    |  5% precio  |  |  8% precio  |  | Mantenimiento|
    +--------------+  +--------------+  |  3% precio  |
                                        +--------------+

---

## Capa Domain - Interfaces

    +-------------------------+     +-------------------------+
    | <<interface>>           |     | <<interface>>           |
    | IVehiculoRepository<T>  |     | ICatalogoObserver       |
    +-------------------------+     +-------------------------+
    | + Agregar(T)            |     | + OnCatalogo-           |
    | + Eliminar(T)           |     |   Actualizado(string)   |
    | + BuscarPorMarca(string)|     +-------------------------+
    | + ObtenerTodos():List<T>|
    | + CalcularValorActual() |     +-------------------------+
    +-------------------------+     | <<interface>>           |
              △                     | ICatalogoObservable     |
              |                     +-------------------------+
    +---------+-----------+         | + Suscribir(observer)  |
    |  IAutomovilRepository|        | + Desuscribir(observer)|
    |  ICamionRepository   |        | + Notificar(string)    |
    |  IMotocicletaRepo... |        +-------------------------+
    +---------------------+
                                    +-------------------------+
                                    | <<interface>>           |
                                    | ICatalogoView           |
                                    +-------------------------+
                                    | + TipoSeleccionado      |
                                    | + Marca, Modelo, Anio  |
                                    | + Precio, Color        |
                                    | + Caracteristica       |
                                    | + MarcaBusqueda        |
                                    | + MostrarVehiculos()   |
                                    | + MostrarMensaje()     |
                                    | + LimpiarFormulario()  |
                                    +-------------------------+

---

## Capa Application

    +----------------------------------+
    |         CatalogoService          |
    +----------------------------------+
    | - _repositorio                   |
    | - _observadores: List            |
    +----------------------------------+
    | + AgregarVehiculo(Vehiculo)      |
    | + EliminarVehiculo(Vehiculo)     |
    | + BuscarPorMarca(string)         |
    | + ObtenerTodos(): List<Vehiculo> |
    | + CalcularValorActualTotal()     |
    | + Suscribir(observer)            |
    | + Desuscribir(observer)          |
    | + Notificar(string)              |
    +----------------------------------+
    implements: ICatalogoObservable

    +----------------------------------+
    |          ExportService           |
    +----------------------------------+
    | + ExportarAExcel(List<Vehiculo>) |
    |   : string                       |
    +----------------------------------+

---

## Capa Domain - Factory

    +----------------------------------+
    |       <<static>>                 |
    |       VehiculoFactory            |
    +----------------------------------+
    | + Crear(tipo, marca, modelo,     |
    |   año, precio, color,            |
    |   caracteristica): Vehiculo      |
    +----------------------------------+

---

## Capa Infrastructure

    +----------------------------------+
    |       VehiculoRepository         |
    +----------------------------------+
    | - _connectionString: string      |
    +----------------------------------+
    | + Agregar(Vehiculo)              |
    | + Eliminar(Vehiculo)             |
    | + BuscarPorMarca(string)         |
    | + ObtenerTodos()                 |
    | + CalcularValorActualTotal()     |
    | + MapearVehiculo(reader)         |
    +----------------------------------+
    implements: IVehiculoRepository<Vehiculo>
              △
              |
    +---------+-----------+-----------+
    |                     |           |
    +-------------+ +----------+ +----------+
    |Automovil-   | |Camion-   | |Motocicle-|
    |Repository   | |Repository| |taRepo... |
    +-------------+ +----------+ +----------+
    |+BuscarPor-  | |+BuscarPor| |+BuscarPor|
    | CantidadPu- | | PesoMaxi-| | Tipo()   |
    | ertas()     | | mo()     | |          |
    +-------------+ +----------+ +----------+

    +----------------------------------+
    |           UnitOfWork             |
    +----------------------------------+
    | - _connectionString: string      |
    | - _vehiculos                     |
    | - _automoviles                   |
    | - _camiones                      |
    | - _motocicletas                  |
    +----------------------------------+
    | + Vehiculos                      |
    | + Automoviles                    |
    | + Camiones                       |
    | + Motocicletas                   |
    | + Commit()                       |
    | + Dispose()                      |
    +----------------------------------+
    implements: IUnitOfWork

---

## Capa Presentation

    +----------------------------------+
    |        CatalogoPresenter         |
    +----------------------------------+
    | - _view: ICatalogoView           |
    | - _service: CatalogoService      |
    +----------------------------------+
    | + CargarVehiculos()              |
    | + RegistrarVehiculo()            |
    | + BuscarVehiculo()               |
    | + EliminarVehiculo()             |
    +----------------------------------+

    +----------------------------------+
    |         FormPrincipal            |
    +----------------------------------+
    | - _presenter: CatalogoPresenter  |
    | - _servicio: CatalogoService     |
    +----------------------------------+
    | + OnCatalogoActualizado()        |
    | + MostrarVehiculos()             |
    | + MostrarMensaje()               |
    | + LimpiarFormulario()            |
    +----------------------------------+
    implements: ICatalogoView, ICatalogoObserver

    +----------------------------------+
    |          FormDashboard           |
    +----------------------------------+
    | - _vehiculos: List<Vehiculo>     |
    +----------------------------------+
    | + DibujarGrafica()               |
    | + CrearTarjeta()                 |
    | + CrearTarjetaFinanciera()       |
    +----------------------------------+

---

## Relaciones entre capas

    Presentation  →  depende de  →  Application
    Application   →  depende de  →  Domain
    Infrastructure→  depende de  →  Domain
    Presentation  →  NO depende  →  Infrastructure (directamente)