namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public double PrecioVenta { get; set; }
        public DateTime Fecha { get; set; }

        // Propiedades de navegación para mostrar en UI
        public string VehiculoDescripcion { get; set; } = "";
        public string EmpleadoNombre { get; set; } = "";
        public string ClienteNombre { get; set; } = "";

        public Venta() { }

        public Venta(int vehiculoId, int empleadoId, int clienteId, double precioVenta)
        {
            if (precioVenta <= 0)
                throw new ArgumentException("El precio de venta debe ser mayor a cero.");

            VehiculoId = vehiculoId;
            EmpleadoId = empleadoId;
            ClienteId = clienteId;
            PrecioVenta = precioVenta;
            Fecha = DateTime.Now;
        }

        public override string ToString() =>
            $"Venta #{Id} — {VehiculoDescripcion} a {ClienteNombre} por ${PrecioVenta:F2} ({Fecha:dd/MM/yyyy})";
    }
}