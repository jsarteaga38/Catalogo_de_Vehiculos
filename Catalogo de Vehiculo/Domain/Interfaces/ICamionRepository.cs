using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Repositorio específico para Camion.
    /// </summary>
    public interface ICamionRepository : IVehiculoRepository<Camion>
    {
        /// <summary>Busca camiones con peso menor o igual al máximo indicado.</summary>
        List<Camion> BuscarPorPesoMaximo(double pesoMaximo);
    }
}