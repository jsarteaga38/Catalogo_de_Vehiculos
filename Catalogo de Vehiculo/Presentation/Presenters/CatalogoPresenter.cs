using System;
using Catalogo_de_Vehiculo.Application.Services;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Factories;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Presentation.Presenters
{
    /// <summary>
    /// Presenter (MVP): coordina la View y el Service.
    /// La View no conoce el Service; el Service no conoce la View.
    /// </summary>
    public class CatalogoPresenter
    {
        private readonly ICatalogoView _view;
        private readonly CatalogoService _service;

        public CatalogoPresenter(ICatalogoView view, CatalogoService service)
        {
            _view = view;
            _service = service;
        }

        public void CargarVehiculos()
        {
            _view.MostrarVehiculos(_service.ObtenerTodos());
        }

        public void RegistrarVehiculo()
        {
            try
            {
                if (!int.TryParse(_view.Anio, out int año))
                {
                    _view.MostrarMensaje("El año debe ser un número entero válido.", "Dato inválido", true);
                    return;
                }

                if (!double.TryParse(_view.Precio, out double precio))
                {
                    _view.MostrarMensaje("El precio debe ser un número válido.", "Dato inválido", true);
                    return;
                }

                Vehiculo vehiculo = VehiculoFactory.Crear(
                    _view.TipoSeleccionado,
                    _view.Marca,
                    _view.Modelo,
                    año,
                    precio,
                    _view.Color,
                    _view.Caracteristica
                );

                _service.AgregarVehiculo(vehiculo);
                _view.LimpiarFormulario();
                _view.MostrarMensaje("Vehículo registrado correctamente.", "Éxito");
            }
            catch (ArgumentException ex)
            {
                _view.MostrarMensaje(ex.Message, "Dato inválido", true);
            }
            catch (Exception ex)
            {
                _view.MostrarMensaje("Error inesperado: " + ex.Message, "Error", true);
            }
        }

        public void BuscarVehiculo()
        {
            Vehiculo? encontrado = _service.BuscarPorMarca(_view.MarcaBusqueda);
            if (encontrado != null)
                _view.MostrarVehiculos(new List<Vehiculo> { encontrado });
            else
                _view.MostrarMensaje("No se encontró ningún vehículo con esa marca.", "Sin resultados");
        }

        public void EliminarVehiculo()
        {
            Vehiculo? encontrado = _service.BuscarPorMarca(_view.MarcaBusqueda);
            if (encontrado == null)
            {
                _view.MostrarMensaje("No se encontró ningún vehículo con esa marca.", "Sin resultados");
                return;
            }
            _service.EliminarVehiculo(encontrado);
            _view.MostrarMensaje("Vehículo eliminado correctamente.", "Éxito");
        }
    }
}