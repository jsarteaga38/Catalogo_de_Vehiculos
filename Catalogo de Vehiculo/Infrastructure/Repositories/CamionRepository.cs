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

        public void Agregar(Camion vehiculo)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(Camion vehiculo)
        {
            throw new NotImplementedException();
        }

        public Camion? BuscarPorMarca(string marca)
        {
            throw new NotImplementedException();
        }

        public List<Camion> ObtenerTodos()
        {
            throw new NotImplementedException();
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
    }
}