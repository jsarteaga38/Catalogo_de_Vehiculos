namespace Catalogo_de_Vehiculo.Domain.Entities

{
    public abstract class Vehiculo
    {
        public string Marca { get; set; } = "";
        public string Modelo { get; set; } = "";
        public int Año { get; set; }
        public double Precio { get; set; }
        public string Color { get; set; } = "";
        public string Estado { get; set; } = "Disponible";
        protected Vehiculo(string marca, string modelo, int año, double precio, string color)
        {
            // RN-05: Precio mayor a cero
            if (precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            // RN-04: Año entre 1900 y el año actual
            if (año < 1900 || año > DateTime.Now.Year)
                throw new ArgumentException($"El año debe estar entre 1900 y {DateTime.Now.Year}.");

            // RN-06: Marca y Modelo no pueden ser vacíos o nulos
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("La marca no puede estar vacía.");

            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("El modelo no puede estar vacío.");

            Marca = marca;
            Modelo = modelo;
            Año = año;
            Precio = precio;
            Color = color;

        }

        public abstract double CalcularDepreciacion();
        public abstract double CalcularCostoMantenimiento();

        public virtual string ObtenerCaracteristica()
        {
            return "";
        }

        public override string ToString()
        {
            return $"{GetType().Name} - {Marca} {Modelo} ({Año}) - {Color} - $" +
                   $"{Precio} | Depreciación: {CalcularDepreciacion():F2} | " +
                   $"Mantenimiento: {CalcularCostoMantenimiento():F2} {ObtenerCaracteristica()}";
        }
    }
}