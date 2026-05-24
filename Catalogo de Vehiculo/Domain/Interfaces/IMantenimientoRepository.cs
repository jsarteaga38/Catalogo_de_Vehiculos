using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    public interface IMantenimientoRepository
    {
        void Agregar(Mantenimiento mantenimiento);
        List<Mantenimiento> ObtenerTodos();
        List<Mantenimiento> ObtenerPorVehiculo(int vehiculoId);
        List<Mantenimiento> ObtenerProximosMantenimientos();
    }
}