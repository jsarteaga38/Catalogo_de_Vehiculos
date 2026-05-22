# 🎨 Justificación de Patrones de Diseño

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## 1. Factory Method

### ¿Qué es?
El patrón Factory Method define una interfaz para crear objetos, pero delega a una clase específica la decisión de qué objeto instanciar. En lugar de usar `new` directamente, se llama a un método fábrica que decide qué clase concreta crear.

### ¿Dónde se implementó?
**Archivo:** `Domain/Factories/VehiculoFactory.cs`

```csharp
public static class VehiculoFactory
{
    public static Vehiculo Crear(string tipo, string marca, string modelo,
        int año, double precio, string color, string caracteristica)
    {
        switch (tipo)
        {
            case "Automovil": return new Automovil(...);
            case "Camion":    return new Camion(...);
            case "Motocicleta": return new Motocicleta(...);
            default: throw new ArgumentException($"Tipo '{tipo}' no reconocido.");
        }
    }
}
```

### ¿Por qué se eligió?
Antes de implementar este patrón, el `FormPrincipal` contenía bloques `if/else` con `new Automovil(...)`, `new Camion(...)` y `new Motocicleta(...)` dispersos. Esto violaba el principio **Open/Closed** — cada vez que se agregaba un tipo nuevo había que modificar el formulario.

Con Factory Method:
- La creación está centralizada en un solo lugar
- El formulario solo llama `VehiculoFactory.Crear(tipo, ...)` sin conocer los detalles
- Agregar un nuevo tipo (`Bicicleta`) solo requiere modificar la fábrica

### Beneficio medible
| Antes | Después |
|-------|---------|
| 3 bloques if/else en FormPrincipal | 1 línea: `VehiculoFactory.Crear(...)` |
| Modificar Form al agregar tipo nuevo | Solo modificar VehiculoFactory |
| Lógica de construcción mezclada con UI | Separación completa |

---

## 2. Repository + Unit of Work

### ¿Qué es?
**Repository** abstrae el acceso a la base de datos detrás de una interfaz. La capa de negocio no sabe si los datos vienen de SQL Server, un archivo o memoria.

**Unit of Work** agrupa todos los repositorios bajo una sola unidad coordinada, garantizando consistencia en las operaciones.

### ¿Dónde se implementó?
**Archivos:**
- `Domain/Interfaces/IVehiculoRepository.cs` — contrato genérico
- `Domain/Interfaces/IUnitOfWork.cs` — contrato del UoW
- `Infrastructure/Repositories/VehiculoRepository.cs` — implementación SQL
- `Infrastructure/Repositories/UnitOfWork.cs` — implementación UoW

```csharp
public interface IVehiculoRepository<T>
{
    void Agregar(T vehiculo);
    void Eliminar(T vehiculo);
    T? BuscarPorMarca(string marca);
    List<T> ObtenerTodos();
    double CalcularValorActualTotal();
}

public interface IUnitOfWork : IDisposable
{
    IVehiculoRepository<Vehiculo> Vehiculos { get; }
    IAutomovilRepository Automoviles { get; }
    ICamionRepository Camiones { get; }
    IMotocicletaRepository Motocicletas { get; }
    void Commit();
}
```

### ¿Por qué se eligió?
El proyecto requiere acceso a SQL Server para persistir vehículos. Sin este patrón, el formulario tendría queries SQL directamente mezclados con la lógica de presentación. Con Repository:
- La capa de presentación nunca conoce los detalles de SQL
- Si se cambia la base de datos, solo se modifica la capa Infrastructure
- Se pueden hacer pruebas unitarias con repositorios en memoria

### Beneficio medible
| Sin patrón | Con patrón |
|-----------|-----------|
| SQL en el formulario | SQL solo en Infrastructure |
| Difícil cambiar BD | Cambio transparente |
| No testeable | Testeable con mocks |

---

## 3. Observer

### ¿Qué es?
El patrón Observer define una relación de dependencia uno-a-muchos entre objetos. Cuando el sujeto cambia de estado, todos sus observadores son notificados automáticamente.

### ¿Dónde se implementó?
**Archivos:**
- `Domain/Interfaces/ICatalogoObserver.cs` — interfaz del observador
- `Domain/Interfaces/ICatalogoObservable.cs` — interfaz del sujeto
- `Application/Services/CatalogoService.cs` — sujeto observable
- `Presentation/Forms/FormPrincipal.cs` — observador concreto

```csharp
// Sujeto
public class CatalogoService : ICatalogoObservable
{
    public void AgregarVehiculo(Vehiculo v)
    {
        _repositorio.Agregar(v);
        Notificar($"Vehículo agregado: {v.Marca}");  // ← notifica automáticamente
    }
}

// Observador
public class FormPrincipal : Form, ICatalogoObserver
{
    public void OnCatalogoActualizado(string mensaje)
    {
        _presenter.CargarVehiculos();  // ← se actualiza automáticamente
    }
}
```

### ¿Por qué se eligió?
Antes de este patrón, cada botón del formulario llamaba manualmente a `MostrarEnGrid()` después de cada operación. Esto generaba código repetido y acoplamiento directo entre la UI y la lógica. Con Observer:
- El formulario se suscribe una sola vez en el constructor
- Cualquier cambio en el catálogo actualiza la UI automáticamente
- Se pueden agregar más observadores sin modificar el servicio

### Beneficio medible
| Antes | Después |
|-------|---------|
| `MostrarEnGrid()` en cada botón | Solo en `OnCatalogoActualizado()` |
| Acoplamiento directo Form-Servicio | Desacoplamiento total |
| Fácil olvidar actualizar la UI | Actualización garantizada |

---

## Conclusión

Los tres patrones se complementan formando una arquitectura cohesiva:

FormPrincipal (Observer)
→ CatalogoPresenter (MVP)
→ CatalogoService (Observable)
→ VehiculoFactory (Factory Method)
→ VehiculoRepository (Repository)
→ UnitOfWork (Unit of Work)


Cada patrón resuelve un problema específico sin solaparse con los demás, demostrando una aplicación práctica y justificada de los patrones de diseño en un proyecto de mediana escala.