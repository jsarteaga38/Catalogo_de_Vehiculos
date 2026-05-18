using Catalogo_de_Vehiculo.Modelos;

namespace Catalogo_de_Vehiculo.Repositorios
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