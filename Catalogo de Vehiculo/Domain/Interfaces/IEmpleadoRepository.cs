using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        void Agregar(Empleado empleado);
        void Eliminar(int id);
        Empleado? BuscarPorId(int id);
        Empleado? BuscarPorCedula(string cedula);
        List<Empleado> ObtenerTodos();
    }
}