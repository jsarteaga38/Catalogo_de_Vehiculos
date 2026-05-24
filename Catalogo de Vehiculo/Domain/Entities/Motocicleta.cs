using System;
using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Motocicleta : Vehiculo
    {
        public string tipoMotosicleta { get; set; }

        public Motocicleta(string marca, string modelo, int año, double precio, string color, string tipoMotosicleta)
            : base(marca, modelo, año, precio, color)
        {
            this.tipoMotosicleta = tipoMotosicleta;
        }

        public override double CalcularDepreciacion()
        {
            int antiguedad = Math.Min(DateTime.Now.Year - Año, 15);
            return Precio * (0.08 * antiguedad);
        }

        public override double CalcularCostoMantenimiento()
        {
            return Precio * 0.03;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Tipo de motocicleta: {tipoMotosicleta}";
        }
    }
}