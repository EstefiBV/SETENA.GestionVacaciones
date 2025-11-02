using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class FeriadoDAL
    {
        private readonly ConexionDAL _conexion;

        public FeriadoDAL()
        {
            _conexion = new ConexionDAL();
        }

        public List<Feriado> ObtenerTodos()
        {
            var lista = new List<Feriado>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "SELECT * FROM Feriados ORDER BY Fecha";
            using var cmd = new SqlCommand(query, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Feriado
                {
                    IdFeriado = (int)reader["IdFeriado"],
                    Fecha = Convert.ToDateTime(reader["Fecha"]),
                    Descripcion = reader["Descripcion"].ToString(),
                    Tipo = reader["Tipo"].ToString()
                });
            }

            return lista;
        }

        public bool Registrar(Feriado feriado)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "INSERT INTO Feriados (Fecha, Descripcion, Tipo) VALUES (@Fecha, @Descripcion, @Tipo)";
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Fecha", feriado.Fecha);
            cmd.Parameters.AddWithValue("@Descripcion", feriado.Descripcion);
            cmd.Parameters.AddWithValue("@Tipo", feriado.Tipo);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
