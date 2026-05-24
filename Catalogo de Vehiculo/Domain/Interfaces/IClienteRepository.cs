using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    public interface IClienteRepository
    {
        void Agregar(Cliente cliente);
        void Eliminar(int id);
        Cliente? BuscarPorId(int id);
        Cliente? BuscarPorCedula(string cedula);
        List<Cliente> ObtenerTodos();
    }
}