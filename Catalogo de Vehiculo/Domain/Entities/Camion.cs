using System;
using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Camion : Vehiculo
    {
        public double peso { get; set; }

        public Camion(string marca, string modelo, int año, double precio, string color, double peso)
            : base(marca, modelo, año, precio, color)
        {
            this.peso = peso;
        }

        public override double CalcularDepreciacion()
        {
            int antiguedad = Math.Min(DateTime.Now.Year - Año, 15);
            return Precio * (0.12 * antiguedad);
        }

        public override double CalcularCostoMantenimiento()
        {
            return Precio * 0.08;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Peso de carga: {peso} T";
        }
    }
}