namespace Catalogo_de_Vehiculo.Repositorios
{
    /// <summary>
    /// Interfaz genérica base para operaciones CRUD sobre vehículos.
    /// </summary>
    public interface IVehiculoRepository<T>
    {
        /// <summary>Agrega un vehículo al repositorio.</summary>
        void Agregar(T vehiculo);

        /// <summary>Elimina un vehículo del repositorio.</summary>
        void Eliminar(T vehiculo);

        /// <summary>Busca un vehículo por marca ignorando mayúsculas/minúsculas.</summary>
        T? BuscarPorMarca(string marca);

        /// <summary>Retorna todos los vehículos registrados.</summary>
        List<T> ObtenerTodos();

        /// <summary>Calcula el valor actual total de la flota sin valores negativos.</summary>
        double CalcularValorActualTotal();
    }
}