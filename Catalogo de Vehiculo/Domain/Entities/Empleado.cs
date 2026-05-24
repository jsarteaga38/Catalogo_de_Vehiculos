namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Cargo { get; set; } = "";
        public string Contacto { get; set; } = "";

        public Empleado() { }

        public Empleado(string nombre, string cedula, string cargo, string contacto)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(cedula))
                throw new ArgumentException("La cédula no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(cargo))
                throw new ArgumentException("El cargo no puede estar vacío.");

            Nombre = nombre;
            Cedula = cedula;
            Cargo = cargo;
            Contacto = contacto;
        }

        public override string ToString() => $"{Nombre} — {Cargo} (Cédula: {Cedula})";
    }
}