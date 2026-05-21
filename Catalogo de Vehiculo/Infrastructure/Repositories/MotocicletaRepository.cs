using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class MotocicletaRepository : VehiculoRepository, IMotocicletaRepository
    {
        private readonly string _connectionString;

        public MotocicletaRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public new void Agregar(Motocicleta vehiculo)
        {
            base.Agregar(vehiculo);
        }

        public new void Eliminar(Motocicleta vehiculo)
        {
            base.Eliminar(vehiculo);
        }

        public new Motocicleta? BuscarPorMarca(string marca)
        {
            Vehiculo? v = base.BuscarPorMarca(marca);
            return v as Motocicleta;
        }

        public new List<Motocicleta> ObtenerTodos()
        {
            List<Motocicleta> lista = new List<Motocicleta>();
            string sql = "SELECT * FROM Vehiculos WHERE Tipo = 'Motocicleta'";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Motocicleta(
                        reader["Marca"].ToString()!,
                        reader["Modelo"].ToString()!,
                        Convert.ToInt32(reader["Año"]),
                        Convert.ToDouble(reader["Precio"]),
                        reader["Color"].ToString()!,
                        reader["Caracteristica"].ToString()!
                    ));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener motocicletas: " + ex.Message);
            }
            return lista;
        }

        public List<Motocicleta> BuscarPorTipo(string tipo)
        {
            List<Motocicleta> lista = new List<Motocicleta>();
            string sql = @"SELECT * FROM Vehiculos 
                           WHERE Tipo = 'Motocicleta' 
                           AND LOWER(Caracteristica) = LOWER(@Tipo)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Motocicleta(
                        reader["Marca"].ToString()!,
                        reader["Modelo"].ToString()!,
                        Convert.ToInt32(reader["Año"]),
                        Convert.ToDouble(reader["Precio"]),
                        reader["Color"].ToString()!,
                        reader["Caracteristica"].ToString()!
                    ));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar por tipo: " + ex.Message);
            }
            return lista;
        }

        public double CalcularValorActualTotal()
        {
            double total = 0;
            foreach (Motocicleta m in ObtenerTodos())
                total += Math.Max(0, m.Precio - m.CalcularDepreciacion());
            return total;
        }
    }
}