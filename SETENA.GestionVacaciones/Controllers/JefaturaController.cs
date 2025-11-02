using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;
using System.Security.Claims;

namespace SETENA.GestionVacaciones.Controllers
{
    [Authorize(Roles = "Jefatura,Administrador")]
    public class JefaturaController : Controller
    {
        private readonly SolicitudVacacionesBLL _solicitudBLL;
        private readonly UsuarioBLL _usuarioBLL;
        private readonly HistorialSolicitudBLL _historialBLL;

        public JefaturaController()
        {
            _solicitudBLL = new SolicitudVacacionesBLL();
            _usuarioBLL = new UsuarioBLL();
            _historialBLL = new HistorialSolicitudBLL();
        }

        // 1. Listar solicitudes pendientes de revisión
        public IActionResult Pendientes()
        {
            var pendientes = _solicitudBLL.ObtenerPendientes();

            if (pendientes.Count == 0)
                ViewBag.Mensaje = "No hay solicitudes pendientes de revisión.";

            return View(pendientes);
        }

        // 2. Mostrar detalle de una solicitud
        public IActionResult Detalle(int id)
        {
            var solicitud = _solicitudBLL.ObtenerPorId(id);
            if (solicitud == null)
                return NotFound();

            var usuario = _usuarioBLL.ObtenerPorId(solicitud.UsuarioId);
            ViewBag.NombreFuncionario = usuario?.NombreCompleto ?? "Funcionario desconocido";

            return View(solicitud);
        }

        // 3. Aprobar solicitud
        [HttpPost]
        public IActionResult Aprobar(int id, string comentario)
        {
            var idJefatura = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            _solicitudBLL.CambiarEstado(id, "Aprobada", comentario, idJefatura);

            _historialBLL.Registrar(id, idJefatura, "Aprobada", comentario);

            TempData["Mensaje"] = "Solicitud aprobada correctamente.";
            return RedirectToAction("Pendientes");
        }

        // 4. Rechazar solicitud
        [HttpPost]
        public IActionResult Rechazar(int id, string comentario)
        {
            var idJefatura = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            _solicitudBLL.CambiarEstado(id, "Rechazada", comentario, idJefatura);

            _historialBLL.Registrar(id, idJefatura, "Rechazada", comentario);

            TempData["Mensaje"] = "Solicitud rechazada correctamente.";
            return RedirectToAction("Pendientes");
        }
    }
}
