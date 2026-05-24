using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    public interface IVentaRepository
    {
        void Agregar(Venta venta);
        List<Venta> ObtenerTodos();
        List<Venta> ObtenerPorEmpleado(int empleadoId);
        List<Venta> ObtenerPorVehiculo(int vehiculoId);
    }
}
