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
            int antiguedad = DateTime.Now.Year - Año;
            double depreciacion = Precio * (0.10 * antiguedad);

            // RN-02: La depreciación no puede superar el 100% del precio base
            return Math.Min(depreciacion, Precio);
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