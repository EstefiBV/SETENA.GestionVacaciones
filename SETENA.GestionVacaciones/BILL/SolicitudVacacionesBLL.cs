using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class SolicitudVacacionesBLL
    {
        private readonly SolicitudVacacionesDAL _solicitudDAL;

        public SolicitudVacacionesBLL()
        {
            _solicitudDAL = new SolicitudVacacionesDAL();
        }

        // ==========================
        // Crear nueva solicitud
        // ==========================
        public bool Crear(SolicitudVacaciones solicitud)
        {
            return _solicitudDAL.Crear(solicitud);
        }

        // ==========================
        // Obtener solicitudes por usuario
        // ==========================
        public List<SolicitudVacaciones> ObtenerPorUsuario(int usuarioId)
        {
            return _solicitudDAL.ObtenerPorUsuario(usuarioId);
        }

        // ==========================
        // Obtener todas las solicitudes (para administrador)
        // ==========================
        public List<SolicitudVacaciones> ObtenerTodas()
        {
            return _solicitudDAL.ObtenerTodas();
        }

        // ==========================
        // Obtener solicitudes pendientes (para jefatura)
        // ==========================
        public List<SolicitudVacaciones> ObtenerPendientes()
        {
            var todas = _solicitudDAL.ObtenerTodas();
            return todas.Where(s => s.Estado == "Pendiente").ToList();
        }

        // ==========================
        // Cambiar estado (Aprobada / Rechazada / Revisión)
        // ==========================
        public void CambiarEstado(int id, string nuevoEstado, string? comentario = "", int? idJefatura = null)
        {
            _solicitudDAL.CambiarEstado(id, nuevoEstado, comentario, idJefatura);
        }

        // ==========================
        // Obtener solicitud por ID
        // ==========================
        public SolicitudVacaciones ObtenerPorId(int id)
        {
            var lista = _solicitudDAL.ObtenerTodas();
            return lista.FirstOrDefault(s => s.Id == id);
        }
    }
}
