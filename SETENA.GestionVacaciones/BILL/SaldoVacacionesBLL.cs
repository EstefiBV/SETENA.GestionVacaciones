using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;

namespace SETENA.GestionVacaciones.BILL
{
    public class SaldoVacacionesBLL
    {
        private readonly SaldoVacacionesDAL _saldoDAL;

        public SaldoVacacionesBLL()
        {
            _saldoDAL = new SaldoVacacionesDAL();
        }

        public SaldoVacaciones ObtenerSaldo(int idUsuario) =>
            _saldoDAL.ObtenerPorUsuario(idUsuario);

        public void RecalcularAnual() =>
            _saldoDAL.RecalcularAnual();

        public void DescontarDias(int idUsuario, decimal dias) =>
            _saldoDAL.ActualizarSaldo(idUsuario, dias);
    }
}
