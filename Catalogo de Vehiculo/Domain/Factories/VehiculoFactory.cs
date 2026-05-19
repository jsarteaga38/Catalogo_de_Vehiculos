using System;
using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Domain.Factories
{
    /// <summary>
    /// Factory Method: centraliza la creación de vehículos.
    /// Aplica OCP: agregar un nuevo tipo solo requiere modificar esta clase.
    /// </summary>
    public static class VehiculoFactory
    {
        /// <summary>
        /// Crea un vehículo según el tipo indicado.
        /// </summary>
        /// <param name="tipo">Automovil | Camion | Motocicleta</param>
        /// <param name="marca">Marca del vehículo</param>
        /// <param name="modelo">Modelo del vehículo</param>
        /// <param name="año">Año de fabricación</param>
        /// <param name="precio">Precio base</param>
        /// <param name="color">Color</param>
        /// <param name="caracteristica">Puertas / Peso / TipoMoto según tipo</param>
        public static Vehiculo Crear(
            string tipo,
            string marca,
            string modelo,
            int año,
            double precio,
            string color,
            string caracteristica)
        {
            switch (tipo)
            {
                case "Automovil":
                    if (!int.TryParse(caracteristica, out int puertas))
                        throw new ArgumentException("La cantidad de puertas debe ser un número entero válido.");
                    return new Automovil(marca, modelo, año, precio, color, puertas);

                case "Camion":
                    if (!double.TryParse(caracteristica, out double peso))
                        throw new ArgumentException("El peso de carga debe ser un número válido.");
                    return new Camion(marca, modelo, año, precio, color, peso);

                case "Motocicleta":
                    if (string.IsNullOrWhiteSpace(caracteristica))
                        throw new ArgumentException("El tipo de motocicleta no puede estar vacío.");
                    return new Motocicleta(marca, modelo, año, precio, color, caracteristica);

                default:
                    throw new ArgumentException($"Tipo de vehículo '{tipo}' no reconocido.");
            }
        }
    }
}