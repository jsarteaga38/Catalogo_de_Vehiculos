using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly string _connectionString;

        public EmpleadoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Agregar(Empleado empleado)
        {
            string sql = @"INSERT INTO Empleados (Nombre, Cedula, Cargo, Contacto)
                           VALUES (@Nombre, @Cedula, @Cargo, @Contacto)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Cedula", empleado.Cedula);
                cmd.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                cmd.Parameters.AddWithValue("@Contacto", empleado.Contacto);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al agregar empleado: " + ex.Message);
            }
        }

        public void Eliminar(int id)
        {
            string sql = "DELETE FROM Empleados WHERE Id = @Id";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar empleado: " + ex.Message);
            }
        }

        public Empleado? BuscarPorId(int id)
        {
            string sql = "SELECT * FROM Empleados WHERE Id = @Id";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return MapearEmpleado(reader);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar empleado: " + ex.Message);
            }
            return null;
        }

        public Empleado? BuscarPorCedula(string cedula)
        {
            string sql = "SELECT * FROM Empleados WHERE Cedula = @Cedula";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Cedula", cedula);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return MapearEmpleado(reader);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar empleado: " + ex.Message);
            }
            return null;
        }

        public List<Empleado> ObtenerTodos()
        {
            List<Empleado> lista = new List<Empleado>();
            string sql = "SELECT * FROM Empleados ORDER BY Nombre";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearEmpleado(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener empleados: " + ex.Message);
            }
            return lista;
        }

        private Empleado MapearEmpleado(SqlDataReader reader)
        {
            return new Empleado
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString()!,
                Cedula = reader["Cedula"].ToString()!,
                Cargo = reader["Cargo"].ToString()!,
                Contacto = reader["Contacto"].ToString()!
            };
        }
    }
}