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

        public void Agregar(Automovil vehiculo)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(Automovil vehiculo)
        {
            throw new NotImplementedException();
        }

        public Automovil? BuscarPorMarca(string marca)
        {
            throw new NotImplementedException();
        }

        public List<Automovil> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public List<Automovil> BuscarPorCantidadPuertas(int cantidadPuertas)
        {
            List<Automovil> lista = new List<Automovil>();
            string sql = @"SELECT * FROM Vehiculos 
                           WHERE Tipo = 'Automovil' 
                           AND Caracteristica = @Puertas";
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
    }
}