namespace Catalogo_de_Vehiculo.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Contacto { get; set; } = "";

        public Cliente() { }

        public Cliente(string nombre, string cedula, string contacto)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(cedula))
                throw new ArgumentException("La cédula no puede estar vacía.");

            Nombre = nombre;
            Cedula = cedula;
            Contacto = contacto;
        }

        public override string ToString() => $"{Nombre} (Cédula: {Cedula})";
    }
}