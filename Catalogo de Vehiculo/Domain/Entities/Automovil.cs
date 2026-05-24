using System;
using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Automovil : Vehiculo
    {
        public int cantidadPlasas { get; set; }

        public Automovil(string marca, string modelo, int año, double precio, string color, int cantidadPlasas)
            : base(marca, modelo, año, precio, color)
        {
            this.cantidadPlasas = cantidadPlasas;
        }

        public override double CalcularDepreciacion()
        {
            int antiguedad = Math.Min(DateTime.Now.Year - Año, 15);
            return Precio * (0.10 * antiguedad);
        }

        public override double CalcularCostoMantenimiento()
        {
            return Precio * 0.05;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Cantidad de puertas: {cantidadPlasas}";
        }
    }
}