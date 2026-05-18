using System;
using System.Collections.Generic;
using System.Text;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Application.Services
{
    internal class ServicioVehiculo
    {
        private List<Vehiculo> vehiculos;

        public ServicioVehiculo()
        {
            vehiculos = new List<Vehiculo>();
        }

        public void AgregarVehiculo(Vehiculo vehiculo)
        {
            // RN-01: No se puede agregar un vehículo nulo
            if (vehiculo != null)
                vehiculos.Add(vehiculo);
        }

        public void EliminarVehiculo(Vehiculo vehiculo)
        {
            if (vehiculo != null)
                vehiculos.Remove(vehiculo);
        }

        public Vehiculo? BuscarPorMarca(string marca)
        {
            if (string.IsNullOrWhiteSpace(marca))
                return null;

            foreach (Vehiculo v in vehiculos)
            {
                if (!string.IsNullOrEmpty(v.Marca) &&
                    v.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase))
                {
                    return v;
                }
            }

            return null;
        }

        public double CalcularTotalDepreciacion()
        {
            double total = 0;
            foreach (Vehiculo v in vehiculos)
                total += v.CalcularDepreciacion();
            return total;
        }

        public double CalcularTotalMantenimiento()
        {
            double total = 0;
            foreach (Vehiculo v in vehiculos)
                total += v.CalcularCostoMantenimiento();
            return total;
        }

        public string MostrarVehiculos()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Vehiculo v in vehiculos)
                sb.AppendLine(v.ToString());
            return sb.ToString();
        }

        public List<Vehiculo> ObtenerTodos()
        {
            return vehiculos;
        }

        public double CalcularValorActualTotal()
        {
            double total = 0;
            foreach (Vehiculo v in vehiculos)
            {
                // RN-02: Ningún vehículo puede aportar valor negativo al total
                double valorActual = v.Precio - v.CalcularDepreciacion();
                total += Math.Max(0, valorActual);
            }
            return total;
        }
    }
}