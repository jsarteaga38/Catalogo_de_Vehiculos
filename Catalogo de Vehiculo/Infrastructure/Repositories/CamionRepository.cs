using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class CamionRepository : VehiculoRepository, ICamionRepository
    {
        private readonly string _connectionString;

        public CamionRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public new void Agregar(Camion vehiculo)
        {
            base.Agregar(vehiculo);
        }

        public new void Eliminar(Camion vehiculo)
        {
            base.Eliminar(vehiculo);
        }

        public new Camion? BuscarPorMarca(string marca)
        {
            Vehiculo? v = base.BuscarPorMarca(marca);
            return v as Camion;
        }

        public new List<Camion> ObtenerTodos()
        {
            List<Camion> lista = new List<Camion>();
            string sql = "SELECT * FROM Vehiculos WHERE Tipo = 'Camion'";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    double peso = double.TryParse(reader["Caracteristica"].ToString(), out double p) ? p : 0;
                    lista.Add(new Camion(
                        reader["Marca"].ToString()!,
                        reader["Modelo"].ToString()!,
                        Convert.ToInt32(reader["Año"]),
                        Convert.ToDouble(reader["Precio"]),
                        reader["Color"].ToString()!,
                        peso
                    ));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener camiones: " + ex.Message);
            }
            return lista;
        }

        public List<Camion> BuscarPorPesoMaximo(double pesoMaximo)
        {
            List<Camion> lista = new List<Camion>();
            string sql = @"SELECT * FROM Vehiculos 
                           WHERE Tipo = 'Camion' 
                           AND CAST(Caracteristica AS FLOAT) <= @PesoMaximo";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PesoMaximo", pesoMaximo);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Camion(
                        reader["Marca"].ToString()!,
                        reader["Modelo"].ToString()!,
                        Convert.ToInt32(reader["Año"]),
                        Convert.ToDouble(reader["Precio"]),
                        reader["Color"].ToString()!,
                        Convert.ToDouble(reader["Caracteristica"])
                    ));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar por peso: " + ex.Message);
            }
            return lista;
        }

        public double CalcularValorActualTotal()
        {
            double total = 0;
            foreach (Camion c in ObtenerTodos())
                total += Math.Max(0, c.Precio - c.CalcularDepreciacion());
            return total;
        }
    }
}