using Catalogo_de_Vehiculo.Modelos;

namespace Catalogo_de_Vehiculo.Repositorios
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