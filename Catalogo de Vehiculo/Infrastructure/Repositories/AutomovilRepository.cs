using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class AutomovilRepository : VehiculoRepository, IAutomovilRepository
    {
        private readonly string _connectionString;

        public AutomovilRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public new void Agregar(Automovil vehiculo)
        {
            base.Agregar(vehiculo);
        }

        public new void Eliminar(Automovil vehiculo)
        {
            base.Eliminar(vehiculo);
        }

        public new Automovil? BuscarPorMarca(string marca)
        {
            Vehiculo? v = base.BuscarPorMarca(marca);
            return v as Automovil;
        }

        public new List<Automovil> ObtenerTodos()
        {
            List<Automovil> lista = new List<Automovil>();
            string sql = "SELECT * FROM Vehiculos WHERE Tipo = 'Automovil'";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int puertas = int.TryParse(reader["Caracteristica"].ToString(), out int p) ? p : 0;
                    lista.Add(new Automovil(
                        reader["Marca"].ToString()!,
                        reader["Modelo"].ToString()!,
                        Convert.ToInt32(reader["Año"]),
                        Convert.ToDouble(reader["Precio"]),
                        reader["Color"].ToString()!,
                        puertas
                    ));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener automóviles: " + ex.Message);
            }
            return lista;
        }

        public List<Automovil> BuscarPorCantidadPuertas(int cantidadPuertas)
        {
            List<Automovil> lista = new List<Automovil>();
            string sql = "SELECT * FROM Vehiculos WHERE Tipo = 'Automovil' AND Caracteristica = @Puertas";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Puertas", cantidadPuertas.ToString());
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Automovil(
                        reader["Marca"].ToString()!,
                        reader["Modelo"].ToString()!,
                        Convert.ToInt32(reader["Año"]),
                        Convert.ToDouble(reader["Precio"]),
                        reader["Color"].ToString()!,
                        cantidadPuertas
                    ));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar por puertas: " + ex.Message);
            }
            return lista;
        }

        public double CalcularValorActualTotal()
        {
            double total = 0;
            foreach (Automovil a in ObtenerTodos())
                total += Math.Max(0, a.Precio - a.CalcularDepreciacion());
            return total;
        }
    }
}