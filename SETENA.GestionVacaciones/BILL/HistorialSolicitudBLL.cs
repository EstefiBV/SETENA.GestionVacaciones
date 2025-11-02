using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class HistorialSolicitudBLL
    {
        private readonly HistorialSolicitudDAL _historialDAL;

        public HistorialSolicitudBLL()
        {
            _historialDAL = new HistorialSolicitudDAL();
        }

        public void Registrar(int idSolicitud, int idUsuario, string accion, string observaciones)
        {
            _historialDAL.RegistrarHistorial(idSolicitud, idUsuario, accion, observaciones);
        }

        public List<HistorialSolicitud> ObtenerPorSolicitud(int idSolicitud)
        {
            return _historialDAL.ObtenerPorSolicitud(idSolicitud);
        }
    }
}
