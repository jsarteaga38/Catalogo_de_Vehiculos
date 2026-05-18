using Catalogo_de_Vehiculo.Domain.Entities;
namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Repositorio específico para Motocicleta.
    /// </summary>
    public interface IMotocicletaRepository : IVehiculoRepository<Motocicleta>
    {
        /// <summary>Busca motocicletas por tipo (deportiva, scooter, etc.).</summary>
        List<Motocicleta> BuscarPorTipo(string tipo);
    }
}