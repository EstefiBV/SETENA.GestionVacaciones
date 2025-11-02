using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class RolBLL
    {
        private readonly RolDAL _rolDAL;

        public RolBLL()
        {
            _rolDAL = new RolDAL();
        }

        public List<Rol> ObtenerRoles() =>
            _rolDAL.ObtenerRoles();
    }
}
