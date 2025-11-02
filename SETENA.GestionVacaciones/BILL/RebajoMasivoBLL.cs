using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class RebajoMasivoBLL
    {
        private readonly RebajoMasivoDAL _rebajoDAL;
        private readonly SaldoVacacionesDAL _saldoDAL;
        private readonly AuditoriaDAL _auditoriaDAL;

        public RebajoMasivoBLL()
        {
            _rebajoDAL = new RebajoMasivoDAL();
            _saldoDAL = new SaldoVacacionesDAL();
            _auditoriaDAL = new AuditoriaDAL();
        }

        public bool AplicarRebajo(RebajoMasivo rebajo)
        {
            bool resultado = _rebajoDAL.RegistrarRebajo(rebajo);
            if (resultado)
            {
                _auditoriaDAL.RegistrarAccion(rebajo.IdAdministrador, "Rebajos", $"Rebajo masivo aplicado: {rebajo.Descripcion}");
            }
            return resultado;
        }

        public List<RebajoMasivo> ObtenerHistorial() =>
            _rebajoDAL.ObtenerHistorial();
    }
}
