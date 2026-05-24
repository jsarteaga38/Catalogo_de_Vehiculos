namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Mantenimiento
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public string TipoMantenimiento { get; set; } = "";
        public double Costo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime ProximoMantenimiento { get; set; }

        // Propiedad de navegación para mostrar en UI
        public string VehiculoDescripcion { get; set; } = "";

        public Mantenimiento() { }

        public Mantenimiento(int vehiculoId, string tipoMantenimiento, double costo, DateTime proximoMantenimiento)
        {
            if (string.IsNullOrWhiteSpace(tipoMantenimiento))
                throw new ArgumentException("El tipo de mantenimiento no puede estar vacío.");
            if (costo < 0)
                throw new ArgumentException("El costo no puede ser negativo.");

            VehiculoId = vehiculoId;
            TipoMantenimiento = tipoMantenimiento;
            Costo = costo;
            Fecha = DateTime.Now;
            ProximoMantenimiento = proximoMantenimiento;
        }

        public override string ToString() =>
            $"{TipoMantenimiento} — ${Costo:F2} ({Fecha:dd/MM/yyyy}) — Próximo: {ProximoMantenimiento:dd/MM/yyyy}";
    }
}