using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class MantenimientoRepository : IMantenimientoRepository
    {
        private readonly string _connectionString;

        public MantenimientoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Agregar(Mantenimiento mantenimiento)
        {
            string sql = @"INSERT INTO Mantenimientos 
                           (VehiculoId, TipoMantenimiento, Costo, Fecha, ProximoMantenimiento)
                           VALUES (@VehiculoId, @Tipo, @Costo, @Fecha, @Proximo);
                           UPDATE Vehiculos SET Estado = 'En Mantenimiento' WHERE Id = @VehiculoId;";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@VehiculoId", mantenimiento.VehiculoId);
                cmd.Parameters.AddWithValue("@Tipo", mantenimiento.TipoMantenimiento);
                cmd.Parameters.AddWithValue("@Costo", mantenimiento.Costo);
                cmd.Parameters.AddWithValue("@Fecha", mantenimiento.Fecha);
                cmd.Parameters.AddWithValue("@Proximo", mantenimiento.ProximoMantenimiento);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar mantenimiento: " + ex.Message);
            }
        }

        public List<Mantenimiento> ObtenerTodos()
        {
            List<Mantenimiento> lista = new List<Mantenimiento>();
            string sql = @"SELECT m.*,
                           CONCAT(v.Marca, ' ', v.Modelo, ' (', v.Año, ')') AS VehiculoDesc
                           FROM Mantenimientos m
                           JOIN Vehiculos v ON m.VehiculoId = v.Id
                           ORDER BY m.Fecha DESC";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearMantenimiento(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener mantenimientos: " + ex.Message);
            }
            return lista;
        }

        public List<Mantenimiento> ObtenerPorVehiculo(int vehiculoId)
        {
            List<Mantenimiento> lista = new List<Mantenimiento>();
            string sql = @"SELECT m.*,
                           CONCAT(v.Marca, ' ', v.Modelo, ' (', v.Año, ')') AS VehiculoDesc
                           FROM Mantenimientos m
                           JOIN Vehiculos v ON m.VehiculoId = v.Id
                           WHERE m.VehiculoId = @VehiculoId
                           ORDER BY m.Fecha DESC";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@VehiculoId", vehiculoId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearMantenimiento(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener mantenimientos por vehiculo: " + ex.Message);
            }
            return lista;
        }

        public List<Mantenimiento> ObtenerProximosMantenimientos()
        {
            List<Mantenimiento> lista = new List<Mantenimiento>();
            string sql = @"SELECT m.*,
                           CONCAT(v.Marca, ' ', v.Modelo, ' (', v.Año, ')') AS VehiculoDesc
                           FROM Mantenimientos m
                           JOIN Vehiculos v ON m.VehiculoId = v.Id
                           WHERE m.ProximoMantenimiento >= GETDATE()
                           ORDER BY m.ProximoMantenimiento ASC";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearMantenimiento(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener proximos mantenimientos: " + ex.Message);
            }
            return lista;
        }

        private Mantenimiento MapearMantenimiento(SqlDataReader reader)
        {
            return new Mantenimiento
            {
                Id = Convert.ToInt32(reader["Id"]),
                VehiculoId = Convert.ToInt32(reader["VehiculoId"]),
                TipoMantenimiento = reader["TipoMantenimiento"].ToString()!,
                Costo = Convert.ToDouble(reader["Costo"]),
                Fecha = Convert.ToDateTime(reader["Fecha"]),
                ProximoMantenimiento = Convert.ToDateTime(reader["ProximoMantenimiento"]),
                VehiculoDescripcion = reader["VehiculoDesc"].ToString()!
            };
        }
    }
}