using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class FeriadoBLL
    {
        private readonly FeriadoDAL _feriadoDAL;

        public FeriadoBLL()
        {
            _feriadoDAL = new FeriadoDAL();
        }

        public List<Feriado> ObtenerTodos() =>
            _feriadoDAL.ObtenerTodos();

        public bool Registrar(Feriado feriado) =>
            _feriadoDAL.Registrar(feriado);
    }
}
