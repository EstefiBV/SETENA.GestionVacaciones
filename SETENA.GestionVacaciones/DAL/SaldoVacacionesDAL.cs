using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class SaldoVacacionesDAL
    {
        private readonly ConexionDAL _conexion;

        public SaldoVacacionesDAL()
        {
            _conexion = new ConexionDAL();
        }

        public SaldoVacaciones ObtenerPorUsuario(int idUsuario)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "SELECT * FROM SaldosVacaciones WHERE IdUsuario = @IdUsuario";
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new SaldoVacaciones
                {
                    IdSaldo = (int)reader["IdSaldo"],
                    IdUsuario = (int)reader["IdUsuario"],
                    Periodo = (int)reader["Periodo"],
                    DiasDisponibles = Convert.ToDecimal(reader["DiasDisponibles"]),
                    DiasAcumulados = Convert.ToDecimal(reader["DiasAcumulados"]),
                    UltimaActualizacion = Convert.ToDateTime(reader["UltimaActualizacion"])
                };
            }

            return null;
        }

        public void ActualizarSaldo(int idUsuario, decimal diasRestados)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"UPDATE SaldosVacaciones 
                             SET DiasDisponibles = DiasDisponibles - @DiasRestados,
                                 UltimaActualizacion = GETDATE()
                             WHERE IdUsuario = @IdUsuario";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@DiasRestados", diasRestados);
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

            cmd.ExecuteNonQuery();
        }

        public void RecalcularAnual()
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"UPDATE SaldosVacaciones 
                             SET DiasDisponibles = 
                                 CASE 
                                     WHEN DiasDisponibles + 20 > 40 THEN 40
                                     ELSE DiasDisponibles + 20
                                 END,
                                 UltimaActualizacion = GETDATE()";
            using var cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
    }
}
