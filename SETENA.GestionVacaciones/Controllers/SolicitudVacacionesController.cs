using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;

namespace SETENA.GestionVacaciones.Controllers
{
    public class SolicitudVacacionesController : Controller
    {
        private readonly SolicitudVacacionesBLL _bll;
        private readonly UsuarioBLL _usuarioBLL;

        public SolicitudVacacionesController()
        {
            _bll = new SolicitudVacacionesBLL();
            _usuarioBLL = new UsuarioBLL();
        }

        // ========================
        // FORMULARIO DE CREACIÓN
        // ========================
        public IActionResult Crear()
        {
            return View(new SolicitudVacaciones());
        }

        // ========================
        // GUARDAR SOLICITUD
        // ========================
        [HttpPost]
        public IActionResult Crear(SolicitudVacaciones solicitud)
        {
            if (!ModelState.IsValid)
                return View(solicitud);

            // 🔹 Simulación: usuario autenticado (en producción usar sesión)
            var usuarioActivo = _usuarioBLL.ObtenerPorCorreo("usuario@setena.go.cr");
            if (usuarioActivo == null)
            {
                TempData["Error"] = "No se pudo identificar al usuario actual.";
                return RedirectToAction("Crear");
            }

            solicitud.UsuarioId = usuarioActivo.Id;

            // Validar que las fechas tengan sentido
            if (solicitud.FechaFin < solicitud.FechaInicio)
            {
                TempData["Error"] = "La fecha de fin no puede ser anterior a la de inicio.";
                return View(solicitud);
            }

            // Enviar a la capa de negocio
            bool exito = _bll.Crear(solicitud);

            if (exito)
            {
                TempData["Mensaje"] = "Solicitud enviada correctamente. Pendiente de aprobación por la jefatura.";
                return RedirectToAction("MisSolicitudes");
            }

            TempData["Error"] = "Ocurrió un error al enviar la solicitud.";
            return View(solicitud);
        }

        // ========================
        // VER SOLICITUDES DEL USUARIO
        // ========================
        public IActionResult MisSolicitudes()
        {
            // Simulación de sesión
            var usuarioActivo = _usuarioBLL.ObtenerPorCorreo("usuario@setena.go.cr");

            if (usuarioActivo == null)
            {
                TempData["Error"] = "Debe iniciar sesión.";
                return RedirectToAction("Login", "Cuenta");
            }

            var solicitudes = _bll.ObtenerPorUsuario(usuarioActivo.Id);
            return View(solicitudes);
        }

        // ========================
        // LISTA PARA JEFATURA (Revisión)
        // ========================
        public IActionResult PendientesJefatura()
        {
            // Simula jefatura logueada
            var rol = "Jefatura";

            if (rol != "Jefatura")
            {
                TempData["Error"] = "No tiene permisos para revisar solicitudes.";
                return RedirectToAction("MisSolicitudes");
            }

            var pendientes = _bll.ObtenerPendientes();
            return View(pendientes);
        }

        // ========================
        // APROBAR O RECHAZAR
        // ========================
        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado, string observaciones)
        {
            if (nuevoEstado != "Aprobada" && nuevoEstado != "Rechazada")
            {
                TempData["Error"] = "Estado inválido.";
                return RedirectToAction("PendientesJefatura");
            }

            _bll.CambiarEstado(id, nuevoEstado, observaciones);

            TempData["Mensaje"] = $"La solicitud fue {nuevoEstado.ToLower()} exitosamente.";
            return RedirectToAction("PendientesJefatura");
        }
    }
}
