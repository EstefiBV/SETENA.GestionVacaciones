using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class RebajoMasivoDAL
    {
        private readonly ConexionDAL _conexion;

        public RebajoMasivoDAL()
        {
            _conexion = new ConexionDAL();
        }

        public bool RegistrarRebajo(RebajoMasivo rebajo)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"INSERT INTO RebajosMasivos (Descripcion, FechaInicio, FechaFin, IdAdministrador)
                             VALUES (@Desc, @Inicio, @Fin, @Admin)";
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Desc", rebajo.Descripcion);
            cmd.Parameters.AddWithValue("@Inicio", rebajo.FechaInicio);
            cmd.Parameters.AddWithValue("@Fin", rebajo.FechaFin);
            cmd.Parameters.AddWithValue("@Admin", rebajo.IdAdministrador);

            return cmd.ExecuteNonQuery() > 0;
        }

        public List<RebajoMasivo> ObtenerHistorial()
        {
            var lista = new List<RebajoMasivo>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "SELECT * FROM RebajosMasivos ORDER BY FechaRegistro DESC";
            using var cmd = new SqlCommand(query, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new RebajoMasivo
                {
                    IdRebajo = (int)reader["IdRebajo"],
                    Descripcion = reader["Descripcion"].ToString(),
                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                    FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                    FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                    IdAdministrador = (int)reader["IdAdministrador"]
                });
            }

            return lista;
        }
    }
}
