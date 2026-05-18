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
            int antiguedad = DateTime.Now.Year - Año;
            double depreciacion = Precio * (0.12 * antiguedad);

            // RN-02: La depreciación no puede superar el 100% del precio base
            return Math.Min(depreciacion, Precio);
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