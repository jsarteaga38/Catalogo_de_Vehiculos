using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Catalogo_de_Vehiculo.Modelos;

namespace Catalogo_de_Vehiculo.Repositorios.Implementaciones
{
    public class CamionRepository : VehiculoRepository, ICamionRepository
    {
        private readonly string _connectionString;

        public CamionRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Camion> BuscarPorPesoMaximo(double pesoMaximo)
        {
            List<Camion> lista = new List<Camion>();
            string sql = @"SELECT * FROM Vehiculos 
                           WHERE Tipo = 'Camion' 
                           AND CAST(Caracteristica AS FLOAT) <= @PesoMaximo";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PesoMaximo", pesoMaximo);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
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