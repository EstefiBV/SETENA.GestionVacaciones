using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class AuditoriaBLL
    {
        private readonly AuditoriaDAL _auditoriaDAL;

        public AuditoriaBLL()
        {
            _auditoriaDAL = new AuditoriaDAL();
        }

        public void Registrar(int idUsuario, string modulo, string descripcion) =>
            _auditoriaDAL.RegistrarAccion(idUsuario, modulo, descripcion);

        public List<Auditoria> ObtenerUltimos() =>
            _auditoriaDAL.ObtenerUltimosRegistros();
    }
}
