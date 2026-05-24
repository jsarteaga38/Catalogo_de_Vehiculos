# ✅ Mapeo de Principios SOLID

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## S — Single Responsibility Principle (Responsabilidad Única)

> *"Una clase debe tener una sola razón para cambiar."*

### Aplicación en el proyecto

| Clase | Única responsabilidad |
|-------|----------------------|
| `Vehiculo` | Modelar los datos base de un vehículo |
| `Automovil` | Modelar específicamente un automóvil con sus reglas de depreciación |
| `VehiculoFactory` | Solo crear instancias de vehículos |
| `VehiculoRepository` | Solo persistir y recuperar vehículos de SQL Server |
| `CatalogoService` | Solo coordinar operaciones del catálogo y notificar observadores |
| `ExportService` | Solo exportar datos a Excel |
| `CatalogoPresenter` | Solo coordinar la comunicación entre View y Service |
| `FormPrincipal` | Solo manejar la interfaz gráfica |
| `FormDashboard` | Solo mostrar estadísticas visuales |

### Evidencia
ExportService es un ejemplo claro: se creó como clase separada en lugar de agregar el método de exportación directamente en CatalogoService. Si mañana cambia el formato de exportación, solo se modifica ExportService.

---

## O — Open/Closed Principle (Abierto/Cerrado)

> *"Una clase debe estar abierta para extensión pero cerrada para modificación."*

### Aplicación en el proyecto

El diseño permite agregar un nuevo tipo de vehículo sin modificar código existente.

Paso 1 - Crear la nueva clase extendiendo Vehiculo:

    public class Bicicleta : Vehiculo
    {
        public int NumeroVelocidades { get; set; }
        public override double CalcularDepreciacion() => Precio * (0.05 * (DateTime.Now.Year - Año));
        public override double CalcularCostoMantenimiento() => Precio * 0.02;
    }

Paso 2 - Agregar el caso en VehiculoFactory:

    case "Bicicleta":
        return new Bicicleta(...);

Nada más se toca. CatalogoService, VehiculoRepository, FormPrincipal permanecen sin cambios.

---

## L — Liskov Substitution Principle (Sustitución de Liskov)

> *"Los objetos de una clase derivada deben poder sustituir a los de la clase base sin alterar el comportamiento del programa."*

### Aplicación en el proyecto

Automovil, Camion y Motocicleta heredan de Vehiculo y pueden usarse en cualquier lugar donde se espera un Vehiculo:

    // VehiculoRepository trabaja con Vehiculo base
    public void Agregar(Vehiculo vehiculo)  // acepta cualquier subtipo

    // CatalogoService opera sobre List<Vehiculo>
    public List<Vehiculo> ObtenerTodos()   // retorna cualquier combinación

    // FormPrincipal itera sobre Vehiculo
    foreach (Vehiculo v in lista)
        total += v.CalcularDepreciacion();  // polimorfismo correcto

Cada subclase sobreescribe CalcularDepreciacion() y CalcularCostoMantenimiento() con tasas propias (10%, 12%, 8%) sin romper el contrato de Vehiculo.

---

## I — Interface Segregation Principle (Segregación de Interfaces)

> *"Los clientes no deben verse obligados a depender de interfaces que no usan."*

### Aplicación en el proyecto

En lugar de una única interfaz con todos los métodos, se crearon interfaces específicas:

    // Interfaz genérica base — operaciones comunes
    public interface IVehiculoRepository<T>
    {
        void Agregar(T vehiculo);
        void Eliminar(T vehiculo);
        T? BuscarPorMarca(string marca);
        List<T> ObtenerTodos();
        double CalcularValorActualTotal();
    }

    // Interfaz específica — solo lo que necesita Automovil
    public interface IAutomovilRepository : IVehiculoRepository<Automovil>
    {
        List<Automovil> BuscarPorCantidadPuertas(int cantidadPuertas);
    }

    // Interfaz específica — solo lo que necesita Camion
    public interface ICamionRepository : IVehiculoRepository<Camion>
    {
        List<Camion> BuscarPorPesoMaximo(double pesoMaximo);
    }

ICamionRepository no obliga a implementar BuscarPorCantidadPuertas porque eso no aplica para camiones.

---

## D — Dependency Inversion Principle (Inversión de Dependencias)

> *"Los módulos de alto nivel no deben depender de módulos de bajo nivel. Ambos deben depender de abstracciones."*

### Aplicación en el proyecto

    CatalogoPresenter  →  depende de  →  ICatalogoView (interfaz)
                                              ↑
                                        FormPrincipal (implementación)

    CatalogoService    →  depende de  →  IVehiculoRepository<Vehiculo> (interfaz)
                                              ↑
                                        VehiculoRepository (implementación)

En código:

    // CatalogoPresenter depende de la interfaz, NO del Form concreto
    public class CatalogoPresenter
    {
        private readonly ICatalogoView _view;
        private readonly CatalogoService _service;

        public CatalogoPresenter(ICatalogoView view, CatalogoService service)
        {
            _view = view;
            _service = service;
        }
    }

Esto permite cambiar FormPrincipal por cualquier otra implementación de ICatalogoView sin modificar el CatalogoPresenter.

---

## Resumen Visual

    S → Cada clase tiene una sola razón para cambiar
        VehiculoFactory ≠ CatalogoService ≠ ExportService ≠ FormPrincipal

    O → Nuevo tipo de vehículo = solo agregar clase + 1 línea en Factory
        Sin tocar: Service, Repository, Form, Presenter

    L → Automovil, Camion, Motocicleta sustituyen a Vehiculo sin romper nada
        foreach(Vehiculo v) funciona con cualquier subtipo

    I → IAutomovilRepository ≠ ICamionRepository ≠ IMotocicletaRepository
        Cada interfaz solo tiene lo que necesita

    D → Presenter → ICatalogoView ← FormPrincipal
        Service → IVehiculoRepository ← VehiculoRepository