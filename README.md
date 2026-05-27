# 📊 Diagrama de Clases

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación
**Institución:** Institución Universitaria Pascual Bravo
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## Entidades principales

    +--------------------------------------------------+
    |                   <<abstract>>                   |
    |                    Vehiculo                      |
    +--------------------------------------------------+
    | + Id: int                                        |
    | + Marca: string                                  |
    | + Modelo: string                                 |
    | + Año: int                                       |
    | + Precio: double                                 |
    | + Color: string                                  |
    | + Estado: string = "Disponible"                  |
    +--------------------------------------------------+
    | + CalcularDepreciacion(): double <<abstract>>    |
    | + CalcularCostoMantenimiento(): double <<abstract>>|
    +--------------------------------------------------+
              △           △           △
              |           |           |
    +---------+--+ +------+----+ +----+--------+
    | Automovil  | |  Camion   | | Motocicleta |
    +------------+ +-----------+ +-------------+
    |+cantidadPla| | + peso:   | |+tipoMoto-   |
    | sas: int   | |   double  | | sicleta:    |
    +------------+ +-----------+ |  string     |
    | Dep: 10%   | | Dep: 12%  | +-------------+
    | Mant: 5%   | | Mant: 8%  | | Dep: 8%     |
    +------------+ +-----------+ | Mant: 3%    |
                                 +-------------+
    REGLA: Depreciacion maxima = min(antiguedad, 15) años

---

## Nuevas entidades

    +-----------------------+     +----------------------+
    |       Empleado        |     |       Cliente        |
    +-----------------------+     +----------------------+
    | + Id: int             |     | + Id: int            |
    | + Nombre: string      |     | + Nombre: string     |
    | + Cedula: string      |     | + Cedula: string     |
    | + Cargo: string       |     | + Contacto: string   |
    | + Contacto: string    |     +----------------------+
    +-----------------------+

    +------------------------------------------+
    |                  Venta                   |
    +------------------------------------------+
    | + Id: int                                |
    | + VehiculoId: int                        |
    | + EmpleadoId: int                        |
    | + ClienteId: int                         |
    | + PrecioVenta: double                    |
    | + Fecha: DateTime                        |
    | + VehiculoDescripcion: string            |
    | + EmpleadoNombre: string                 |
    | + ClienteNombre: string                  |
    +------------------------------------------+
    Al registrar venta → Vehiculo.Estado = "Vendido"

    +------------------------------------------+
    |              Mantenimiento               |
    +------------------------------------------+
    | + Id: int                                |
    | + VehiculoId: int                        |
    | + TipoMantenimiento: string              |
    | + Costo: double                          |
    | + Fecha: DateTime                        |
    | + ProximoMantenimiento: DateTime         |
    | + VehiculoDescripcion: string            |
    +------------------------------------------+
    Al registrar mant. → Vehiculo.Estado = "En Mantenimiento"
    Al liberar         → Vehiculo.Estado = "Disponible"

---

## Interfaces Domain

    IVehiculoRepository<T>          ICatalogoObserver
    IAutomovilRepository            ICatalogoObservable
    ICamionRepository               ICatalogoView
    IMotocicletaRepository          IUnitOfWork
    IEmpleadoRepository
    IClienteRepository
    IVentaRepository
    IMantenimientoRepository

---

## Capa Application

    CatalogoService                 ExportService
    - implementa ICatalogoObservable - ExportarAExcel()
    - AgregarVehiculo()             - Genera .xlsx con ClosedXML
    - EliminarVehiculo()
    - BuscarPorMarca()
    - ObtenerTodos()
    - GetConnectionString()

---

## Capa Infrastructure

    VehiculoRepository              EmpleadoRepository
    AutomovilRepository             ClienteRepository
    CamionRepository                VentaRepository
    MotocicletaRepository           MantenimientoRepository
    UnitOfWork

---

## Capa Presentation

    FormPrincipal                   CatalogoPresenter
    FormDashboard                   (implementa MVP)
    FormEmpleados
    FormVentas
    FormMantenimientos
