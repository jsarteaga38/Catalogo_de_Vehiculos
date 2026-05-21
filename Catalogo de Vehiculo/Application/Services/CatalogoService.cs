using System;
using System.Collections.Generic;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Application.Services
{
    /// <summary>
    /// Servicio principal del catálogo.
    /// Implementa ICatalogoObservable para notificar cambios (patrón Observer).
    /// </summary>
    public class CatalogoService : ICatalogoObservable
    {
        private readonly IVehiculoRepository<Vehiculo> _repositorio;
        private readonly List<ICatalogoObserver> _observadores = new();

        public CatalogoService(IVehiculoRepository<Vehiculo> repositorio)
        {
            _repositorio = repositorio;
        }

        // ── Observer ────────────────────────────────────────────
        public void Suscribir(ICatalogoObserver observer)
        {
            if (!_observadores.Contains(observer))
                _observadores.Add(observer);
        }

        public void Desuscribir(ICatalogoObserver observer)
        {
            _observadores.Remove(observer);
        }

        public void Notificar(string mensaje)
        {
            foreach (ICatalogoObserver obs in _observadores)
                obs.OnCatalogoActualizado(mensaje);
        }

        // ── Operaciones del catálogo ────────────────────────────
        public void AgregarVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null) return;
            _repositorio.Agregar(vehiculo);
            Notificar($"Vehículo agregado: {vehiculo.Marca} {vehiculo.Modelo}");
        }

        public void EliminarVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo == null) return;
            _repositorio.Eliminar(vehiculo);
            Notificar($"Vehículo eliminado: {vehiculo.Marca} {vehiculo.Modelo}");
        }

        public Vehiculo? BuscarPorMarca(string marca)
        {
            if (string.IsNullOrWhiteSpace(marca)) return null;
            return _repositorio.BuscarPorMarca(marca);
        }

        public List<Vehiculo> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }

        public double CalcularValorActualTotal()
        {
            return _repositorio.CalcularValorActualTotal();
        }

        public double CalcularTotalDepreciacion()
        {
            double total = 0;
            foreach (Vehiculo v in ObtenerTodos())
                total += v.CalcularDepreciacion();
            return total;
        }

        public double CalcularTotalMantenimiento()
        {
            double total = 0;
            foreach (Vehiculo v in ObtenerTodos())
                total += v.CalcularCostoMantenimiento();
            return total;
        }
    }
}