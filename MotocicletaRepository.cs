using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Catalogo_de_Vehiculo.Modelos;

namespace Catalogo_de_Vehiculo.Repositorios.Implementaciones
{
    public class MotocicletaRepository : VehiculoRepository, IMotocicletaRepository
    {
        private readonly string _connectionString;

        public MotocicletaRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Motocicleta> BuscarPorTipo(string tipo)
        {
            List<Motocicleta> lista = new List<Motocicleta>();
            string sql = @"SELECT * FROM Vehiculos 
                           WHERE Tipo = 'Motocicleta' 
                           AND LOWER(Caracteristica) = LOWER(@Tipo)";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Tipo", tipo);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
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
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar por tipo: " + ex.Message);
            }
            return lista;
        }
    }
}
