using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Catalogo_de_Vehiculo.Modelos;
using Catalogo_de_Vehiculo.Repositorios;
using Catalogo_de_Vehiculo.Repositorios.Implementaciones;

namespace Catalogo_de_Vehiculo.Implementaciones
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

        public List<Automovil> BuscarPorCantidadPuertas(int cantidadPuertas)
        {
            List<Automovil> lista = new List<Automovil>();
            string sql = @"SELECT * FROM Vehiculos 
                           WHERE Tipo = 'Automovil' 
                           AND Caracteristica = @Puertas";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Puertas", cantidadPuertas.ToString());
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
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
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar por puertas: " + ex.Message);
            }
            return lista;
        }

        public void Eliminar(Automovil vehiculo)
        {
            throw new NotImplementedException();
        }

        Automovil? IVehiculoRepository<Automovil>.BuscarPorMarca(string marca)
        {
            throw new NotImplementedException();
        }

        List<Automovil> IVehiculoRepository<Automovil>.ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}