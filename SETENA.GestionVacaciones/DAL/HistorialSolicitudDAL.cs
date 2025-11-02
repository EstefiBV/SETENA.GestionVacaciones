using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class HistorialSolicitudDAL
    {
        private readonly ConexionDAL _conexion;

        public HistorialSolicitudDAL()
        {
            _conexion = new ConexionDAL();
        }

        // Inserta un registro en el historial
        public void RegistrarHistorial(int idSolicitud, int idUsuario, string accion, string observaciones)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string query = @"
                INSERT INTO HistorialSolicitudes (IdSolicitud, IdUsuario, Accion, Observaciones, FechaAccion)
                VALUES (@Sol, @Usu, @Acc, @Obs, GETDATE())";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Sol", idSolicitud);
            cmd.Parameters.AddWithValue("@Usu", idUsuario);
            cmd.Parameters.AddWithValue("@Acc", accion);
            cmd.Parameters.AddWithValue("@Obs", observaciones ?? string.Empty);

            cmd.ExecuteNonQuery();
        }

        // Devuelve todos los registros del historial de una solicitud
        public List<HistorialSolicitud> ObtenerPorSolicitud(int idSolicitud)
        {
            var lista = new List<HistorialSolicitud>();

            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string query = @"
                SELECT * FROM HistorialSolicitudes 
                WHERE IdSolicitud = @Id 
                ORDER BY FechaAccion DESC";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", idSolicitud);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new HistorialSolicitud
                {
                    IdHistorial = (int)reader["IdHistorial"],
                    IdSolicitud = (int)reader["IdSolicitud"],
                    IdUsuario = (int)reader["IdUsuario"],
                    Accion = reader["Accion"].ToString(),
                    FechaAccion = Convert.ToDateTime(reader["FechaAccion"]),
                    Observaciones = reader["Observaciones"]?.ToString()
                });
            }

            return lista;
        }
    }
}
