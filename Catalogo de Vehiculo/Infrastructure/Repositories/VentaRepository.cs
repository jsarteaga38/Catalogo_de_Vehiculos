using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly string _connectionString;

        public VentaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Agregar(Venta venta)
        {
            string sql = @"INSERT INTO Ventas (VehiculoId, EmpleadoId, ClienteId, PrecioVenta, Fecha)
                           VALUES (@VehiculoId, @EmpleadoId, @ClienteId, @PrecioVenta, @Fecha);
                           UPDATE Vehiculos SET Estado = 'Vendido' WHERE Id = @VehiculoId;";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@VehiculoId", venta.VehiculoId);
                cmd.Parameters.AddWithValue("@EmpleadoId", venta.EmpleadoId);
                cmd.Parameters.AddWithValue("@ClienteId", venta.ClienteId);
                cmd.Parameters.AddWithValue("@PrecioVenta", venta.PrecioVenta);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar venta: " + ex.Message);
            }
        }

        public List<Venta> ObtenerTodos()
        {
            List<Venta> lista = new List<Venta>();
            string sql = @"SELECT v.*, 
                           CONCAT(ve.Marca, ' ', ve.Modelo, ' (', ve.Año, ')') AS VehiculoDesc,
                           e.Nombre AS EmpleadoNombre,
                           c.Nombre AS ClienteNombre
                           FROM Ventas v
                           JOIN Vehiculos ve ON v.VehiculoId = ve.Id
                           JOIN Empleados e ON v.EmpleadoId = e.Id
                           JOIN Clientes c ON v.ClienteId = c.Id
                           ORDER BY v.Fecha DESC";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearVenta(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener ventas: " + ex.Message);
            }
            return lista;
        }

        public List<Venta> ObtenerPorEmpleado(int empleadoId)
        {
            List<Venta> lista = new List<Venta>();
            string sql = @"SELECT v.*,
                           CONCAT(ve.Marca, ' ', ve.Modelo, ' (', ve.Año, ')') AS VehiculoDesc,
                           e.Nombre AS EmpleadoNombre,
                           c.Nombre AS ClienteNombre
                           FROM Ventas v
                           JOIN Vehiculos ve ON v.VehiculoId = ve.Id
                           JOIN Empleados e ON v.EmpleadoId = e.Id
                           JOIN Clientes c ON v.ClienteId = c.Id
                           WHERE v.EmpleadoId = @EmpleadoId
                           ORDER BY v.Fecha DESC";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmpleadoId", empleadoId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearVenta(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener ventas por empleado: " + ex.Message);
            }
            return lista;
        }

        public List<Venta> ObtenerPorVehiculo(int vehiculoId)
        {
            List<Venta> lista = new List<Venta>();
            string sql = @"SELECT v.*,
                           CONCAT(ve.Marca, ' ', ve.Modelo, ' (', ve.Año, ')') AS VehiculoDesc,
                           e.Nombre AS EmpleadoNombre,
                           c.Nombre AS ClienteNombre
                           FROM Ventas v
                           JOIN Vehiculos ve ON v.VehiculoId = ve.Id
                           JOIN Empleados e ON v.EmpleadoId = e.Id
                           JOIN Clientes c ON v.ClienteId = c.Id
                           WHERE v.VehiculoId = @VehiculoId
                           ORDER BY v.Fecha DESC";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@VehiculoId", vehiculoId);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearVenta(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener ventas por vehiculo: " + ex.Message);
            }
            return lista;
        }

        private Venta MapearVenta(SqlDataReader reader)
        {
            return new Venta
            {
                Id = Convert.ToInt32(reader["Id"]),
                VehiculoId = Convert.ToInt32(reader["VehiculoId"]),
                EmpleadoId = Convert.ToInt32(reader["EmpleadoId"]),
                ClienteId = Convert.ToInt32(reader["ClienteId"]),
                PrecioVenta = Convert.ToDouble(reader["PrecioVenta"]),
                Fecha = Convert.ToDateTime(reader["Fecha"]),
                VehiculoDescripcion = reader["VehiculoDesc"].ToString()!,
                EmpleadoNombre = reader["EmpleadoNombre"].ToString()!,
                ClienteNombre = reader["ClienteNombre"].ToString()!
            };
        }
    }
}