using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class AuditoriaDAL
    {
        private readonly ConexionDAL _conexion;

        public AuditoriaDAL()
        {
            _conexion = new ConexionDAL();
        }

        public void RegistrarAccion(int idUsuario, string modulo, string descripcion)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"INSERT INTO Auditoria (IdUsuario, ModuloAfectado, DescripcionAccion, FechaAccion)
                             VALUES (@Usu, @Mod, @Desc, GETDATE())";
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Usu", idUsuario);
            cmd.Parameters.AddWithValue("@Mod", modulo);
            cmd.Parameters.AddWithValue("@Desc", descripcion);
            cmd.ExecuteNonQuery();
        }

        public List<Auditoria> ObtenerUltimosRegistros()
        {
            var lista = new List<Auditoria>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "SELECT TOP 50 * FROM Auditoria ORDER BY FechaAccion DESC";
            using var cmd = new SqlCommand(query, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Auditoria
                {
                    IdAuditoria = (int)reader["IdAuditoria"],
                    IdUsuario = (int)reader["IdUsuario"],
                    ModuloAfectado = reader["ModuloAfectado"].ToString(),
                    DescripcionAccion = reader["DescripcionAccion"].ToString(),
                    FechaAccion = Convert.ToDateTime(reader["FechaAccion"])
                });
            }

            return lista;
        }
    }
}
