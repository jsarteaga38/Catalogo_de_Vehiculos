using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Agregar(Cliente cliente)
        {
            string sql = @"INSERT INTO Clientes (Nombre, Cedula, Contacto)
                           VALUES (@Nombre, @Cedula, @Contacto)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                cmd.Parameters.AddWithValue("@Contacto", cliente.Contacto);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al agregar cliente: " + ex.Message);
            }
        }

        public void Eliminar(int id)
        {
            string sql = "DELETE FROM Clientes WHERE Id = @Id";
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
                throw new Exception("Error al eliminar cliente: " + ex.Message);
            }
        }

        public Cliente? BuscarPorId(int id)
        {
            string sql = "SELECT * FROM Clientes WHERE Id = @Id";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return MapearCliente(reader);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar cliente: " + ex.Message);
            }
            return null;
        }

        public Cliente? BuscarPorCedula(string cedula)
        {
            string sql = "SELECT * FROM Clientes WHERE Cedula = @Cedula";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Cedula", cedula);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read()) return MapearCliente(reader);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar cliente: " + ex.Message);
            }
            return null;
        }

        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> lista = new List<Cliente>();
            string sql = "SELECT * FROM Clientes ORDER BY Nombre";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    lista.Add(MapearCliente(reader));
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener clientes: " + ex.Message);
            }
            return lista;
        }

        private Cliente MapearCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString()!,
                Cedula = reader["Cedula"].ToString()!,
                Contacto = reader["Contacto"].ToString()!
            };
        }
    }
}