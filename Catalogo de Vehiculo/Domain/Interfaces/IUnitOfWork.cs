using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work: agrupa todos los repositorios bajo una sola unidad de trabajo.
    /// Aplica DIP: la capa Application depende de esta interfaz, no de clases concretas.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IVehiculoRepository<Domain.Entities.Vehiculo> Vehiculos { get; }
        IAutomovilRepository Automoviles { get; }
        ICamionRepository Camiones { get; }
        IMotocicletaRepository Motocicletas { get; }
        void Commit();
    }
}