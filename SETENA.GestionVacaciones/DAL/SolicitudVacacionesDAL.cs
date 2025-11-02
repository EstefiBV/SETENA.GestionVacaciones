using SETENA.GestionVacaciones.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class SolicitudVacacionesDAL
    {
        private readonly ConexionDAL _conexion;

        public SolicitudVacacionesDAL()
        {
            _conexion = new ConexionDAL();
        }

        // ==========================================
        // 1️⃣ Crear nueva solicitud de vacaciones
        // ==========================================
        public bool Crear(SolicitudVacaciones s)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string sql = @"
INSERT INTO SolicitudesVacaciones
    (IdUsuario, FechaInicio, FechaFin, DiasSolicitados, Observaciones, Estado, FechaSolicitud)
VALUES
    (@IdUsuario, @FechaInicio, @FechaFin, @DiasSolicitados, @Observaciones, 'Pendiente', GETDATE());";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@IdUsuario", s.UsuarioId);
            cmd.Parameters.AddWithValue("@FechaInicio", s.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", s.FechaFin);
            cmd.Parameters.AddWithValue("@DiasSolicitados", (object?)s.DiasSolicitados ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)s.Observaciones ?? DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ==========================================
        // 2️⃣ Obtener solicitudes por usuario
        // ==========================================
        public List<SolicitudVacaciones> ObtenerPorUsuario(int usuarioId)
        {
            var lista = new List<SolicitudVacaciones>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string sql = @"
SELECT 
    sv.IdSolicitud AS Id,
    sv.IdUsuario,
    sv.FechaInicio,
    sv.FechaFin,
    sv.DiasSolicitados,
    sv.Observaciones,
    sv.Estado,
    sv.FechaSolicitud,
    sv.FechaDecision,
    sv.IdJefatura,
    sv.ComentarioJefatura
FROM SolicitudesVacaciones sv
WHERE sv.IdUsuario = @IdUsuario
ORDER BY sv.FechaSolicitud DESC;";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@IdUsuario", usuarioId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(LeerSolicitud(rd));
            }

            return lista;
        }

        // ==========================================
        // 3️⃣ Obtener todas las solicitudes (para admin)
        // ==========================================
        public List<SolicitudVacaciones> ObtenerTodas()
        {
            var lista = new List<SolicitudVacaciones>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string sql = @"
SELECT 
    sv.IdSolicitud AS Id,
    sv.IdUsuario,
    u.NombreCompleto AS NombreUsuario,
    sv.FechaInicio,
    sv.FechaFin,
    sv.DiasSolicitados,
    sv.Observaciones,
    sv.Estado,
    sv.FechaSolicitud,
    sv.FechaDecision,
    sv.IdJefatura,
    sv.ComentarioJefatura
FROM SolicitudesVacaciones sv
INNER JOIN Usuarios u ON u.IdUsuario = sv.IdUsuario
ORDER BY sv.FechaSolicitud DESC;";

            using var cmd = new SqlCommand(sql, con);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(LeerSolicitud(rd));
            }

            return lista;
        }

        // ==========================================
        // 4️⃣ Obtener solicitudes pendientes (para jefatura)
        // ==========================================
        public List<SolicitudVacaciones> ObtenerPendientes()
        {
            var lista = new List<SolicitudVacaciones>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string sql = @"
SELECT 
    sv.IdSolicitud AS Id,
    sv.IdUsuario,
    u.NombreCompleto AS NombreUsuario,
    sv.FechaInicio,
    sv.FechaFin,
    sv.DiasSolicitados,
    sv.Observaciones,
    sv.Estado,
    sv.FechaSolicitud
FROM SolicitudesVacaciones sv
INNER JOIN Usuarios u ON u.IdUsuario = sv.IdUsuario
WHERE sv.Estado = 'Pendiente'
ORDER BY sv.FechaSolicitud DESC;";

            using var cmd = new SqlCommand(sql, con);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(LeerSolicitud(rd));
            }

            return lista;
        }

        // ==========================================
        // 5️⃣ Actualizar estado (aprobada / rechazada)
        // ==========================================
        public void CambiarEstado(int idSolicitud, string nuevoEstado, string? comentarioJefatura, int? idJefatura = null)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            const string sql = @"
UPDATE SolicitudesVacaciones
SET Estado = @Estado,
    FechaDecision = GETDATE(),
    ComentarioJefatura = @Comentario,
    IdJefatura = @IdJefatura
WHERE IdSolicitud = @IdSolicitud;";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
            cmd.Parameters.AddWithValue("@Comentario", (object?)comentarioJefatura ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdJefatura", (object?)idJefatura ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdSolicitud", idSolicitud);

            cmd.ExecuteNonQuery();
        }

        // ==========================================
        // 🔹 Método auxiliar para mapear datos
        // ==========================================
        private SolicitudVacaciones LeerSolicitud(SqlDataReader rd)
        {
            return new SolicitudVacaciones
            {
                Id = (int)rd["Id"],
                UsuarioId = (int)rd["IdUsuario"],
                NombreUsuario = rd["NombreUsuario"] == DBNull.Value ? null : rd["NombreUsuario"].ToString(),
                FechaInicio = (DateTime)rd["FechaInicio"],
                FechaFin = (DateTime)rd["FechaFin"],
                DiasSolicitados = rd["DiasSolicitados"] == DBNull.Value ? (decimal?)null : (decimal)rd["DiasSolicitados"],
                Observaciones = rd["Observaciones"] == DBNull.Value ? null : rd["Observaciones"].ToString(),
                Estado = rd["Estado"]?.ToString(),
                FechaSolicitud = rd["FechaSolicitud"] == DBNull.Value ? (DateTime?)null : (DateTime)rd["FechaSolicitud"],
                FechaDecision = rd["FechaDecision"] == DBNull.Value ? (DateTime?)null : (DateTime)rd["FechaDecision"],
                IdJefatura = rd["IdJefatura"] == DBNull.Value ? (int?)null : (int)rd["IdJefatura"],
                ComentarioJefatura = rd["ComentarioJefatura"] == DBNull.Value ? null : rd["ComentarioJefatura"].ToString()
            };
        }
    }
}
