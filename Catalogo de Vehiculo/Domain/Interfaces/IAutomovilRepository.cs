using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Repositorio específico para Automovil.
    /// </summary>
    public interface IAutomovilRepository : IVehiculoRepository<Automovil>
    {
        /// <summary>Busca automóviles por cantidad de puertas.</summary>
        List<Automovil> BuscarPorCantidadPuertas(int cantidadPuertas);
    }
}