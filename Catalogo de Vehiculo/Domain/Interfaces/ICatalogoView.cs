using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Contrato que debe cumplir cualquier View del catálogo.
    /// El Presenter solo conoce esta interfaz, nunca el Form concreto.
    /// </summary>
    public interface ICatalogoView
    {
        string TipoSeleccionado { get; }
        string Marca { get; }
        string Modelo { get; }
        string Anio { get; }
        string Precio { get; }
        string Color { get; }
        string Caracteristica { get; }
        string MarcaBusqueda { get; }

        void MostrarVehiculos(List<Vehiculo> vehiculos);
        void MostrarMensaje(string mensaje, string titulo, bool esError = false);
        void LimpiarFormulario();
    }
}