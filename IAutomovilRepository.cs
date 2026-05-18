using Catalogo_de_Vehiculo.Modelos;

namespace Catalogo_de_Vehiculo.Repositorios
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