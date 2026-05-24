using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class VehiculoRepository : IVehiculoRepository<Vehiculo>
    {
        private readonly string _connectionString;

        public VehiculoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Agregar(Vehiculo vehiculo)
        {
            string tipo = vehiculo.GetType().Name;
            string caracteristica = ObtenerCaracteristica(vehiculo);

            string sql = @"INSERT INTO Vehiculos 
                           (Tipo, Marca, Modelo, Año, Precio, Color, Caracteristica)
                           VALUES 
                           (@Tipo,@Marca,@Modelo,@Año,@Precio,@Color,@Caracteristica)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cmd.Parameters.AddWithValue("@Marca", vehiculo.Marca);
                cmd.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
                cmd.Parameters.AddWithValue("@Año", vehiculo.Año);
                cmd.Parameters.AddWithValue("@Precio", vehiculo.Precio);
                cmd.Parameters.AddWithValue("@Color", vehiculo.Color);
                cmd.Parameters.AddWithValue("@Caracteristica", caracteristica);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al agregar: " + ex.Message);
            }
        }

        public void Eliminar(Vehiculo vehiculo)
        {
            string sql = "DELETE FROM Vehiculos WHERE Marca = @Marca AND Modelo = @Modelo";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Marca", vehiculo.Marca);
                cmd.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar: " + ex.Message);
            }
        }

        public Vehiculo? BuscarPorMarca(string marca)
        {
            string sql = "SELECT * FROM Vehiculos WHERE LOWER(Marca) = LOWER(@Marca)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Marca", marca);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                    return MapearVehiculo(reader);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar: " + ex.Message);
            }
            return null;
        }

        public List<Vehiculo> ObtenerTodos()
        {
            List<Vehiculo> lista = new List<Vehiculo>();
            string sql = "SELECT * FROM Vehiculos";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Vehiculo v = MapearVehiculo(reader);
                    if (v != null)
                        lista.Add(v);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener todos: " + ex.Message);
            }
            return lista;
        }

        public double CalcularValorActualTotal()
        {
            List<Vehiculo> todos = ObtenerTodos();
            double total = 0;
            foreach (Vehiculo v in todos)
                total += Math.Max(0, v.Precio - v.CalcularDepreciacion());
            return total;
        }

        private Vehiculo MapearVehiculo(SqlDataReader reader)
        {
            string tipo = reader["Tipo"].ToString()!;
            string marca = reader["Marca"].ToString()!;
            string modelo = reader["Modelo"].ToString()!;
            int año = Convert.ToInt32(reader["Año"]);
            double precio = Convert.ToDouble(reader["Precio"]);
            string color = reader["Color"].ToString()!;
            string caracteristica = reader["Caracteristica"].ToString()!;
            string estado = reader["Estado"].ToString()!;
            int id = Convert.ToInt32(reader["Id"]);

            Vehiculo? v = null;
            switch (tipo)
            {
                case "Automovil":
                    int puertas = int.TryParse(caracteristica, out int p) ? p : 0;
                    v = new Automovil(marca, modelo, año, precio, color, puertas);
                    break;
                case "Motocicleta":
                    v = new Motocicleta(marca, modelo, año, precio, color, caracteristica);
                    break;
                case "Camion":
                    double peso = double.TryParse(caracteristica, out double ps) ? ps : 0;
                    v = new Camion(marca, modelo, año, precio, color, peso);
                    break;
                default:
                    return null!;
            }

            v.Id = id;
            v.Estado = estado;
            return v;
        }

        private string ObtenerCaracteristica(Vehiculo vehiculo)
        {
            if (vehiculo is Camion camion) return camion.peso.ToString();
            if (vehiculo is Automovil auto) return auto.cantidadPlasas.ToString();
            if (vehiculo is Motocicleta moto) return moto.tipoMotosicleta;
            return "";
        }
    }
}