using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;
using System.Security.Claims;

namespace SETENA.GestionVacaciones.Controllers
{
    public class VacacionesController : Controller
    {
        private readonly SolicitudVacacionesBLL _vacacionesBLL;

        public VacacionesController()
        {
            _vacacionesBLL = new SolicitudVacacionesBLL();
        }

        // ==========================
        // ADMINISTRADOR: Lista completa de solicitudes
        // ==========================
        public IActionResult IndexAdmin()
        {
            try
            {
                var lista = _vacacionesBLL.ObtenerTodas();
                return View(lista);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar solicitudes: " + ex.Message;
                return View(new List<SolicitudVacaciones>());
            }
        }

        // ==========================
        // FUNCIONARIO: Formulario para solicitar vacaciones
        // ==========================
        public IActionResult Solicitar()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var nombre = User.FindFirstValue(ClaimTypes.Name);

            var solicitud = new SolicitudVacaciones
            {
                UsuarioId = usuarioId,
                NombreUsuario = nombre
            };

            return View(solicitud);
        }

        [HttpPost]
        public IActionResult Solicitar(SolicitudVacaciones solicitud)
        {
            if (solicitud.FechaInicio >= solicitud.FechaFin)
            {
                ModelState.AddModelError("", "La fecha final debe ser posterior a la fecha de inicio.");
                return View(solicitud);
            }

            solicitud.Estado = "Pendiente";
            solicitud.FechaSolicitud = DateTime.Now;

            try
            {
                bool exito = _vacacionesBLL.Crear(solicitud);

                if (exito)
                {
                    TempData["Mensaje"] = "Solicitud enviada correctamente.";
                    return RedirectToAction("MisVacaciones");
                }

                TempData["Error"] = "No se pudo registrar la solicitud.";
                return View(solicitud);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear solicitud: " + ex.Message;
                return View(solicitud);
            }
        }

        // ==========================
        // FUNCIONARIO: Ver sus solicitudes
        // ==========================
        public IActionResult MisVacaciones()
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized();

            var usuarioId = int.Parse(usuarioIdClaim);
            var misSolicitudes = _vacacionesBLL.ObtenerPorUsuario(usuarioId);
            return View(misSolicitudes);
        }

        // ==========================
        // JEFATURA: Ver solicitudes pendientes para aprobación
        // ==========================
        public IActionResult Aprobaciones()
        {
            try
            {
                var pendientes = _vacacionesBLL.ObtenerPendientes();
                return View(pendientes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar aprobaciones: " + ex.Message;
                return View(new List<SolicitudVacaciones>());
            }
        }

        // ==========================
        // JEFATURA: Cambiar estado de solicitud (Aprobar/Rechazar)
        // ==========================
        [HttpPost]
        public IActionResult ActualizarEstado(int id, string estado, string comentario = "")
        {
            var jefaturaId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                _vacacionesBLL.CambiarEstado(id, estado, comentario, jefaturaId);
                TempData["Mensaje"] = $"Solicitud #{id} actualizada a '{estado}'.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar estado: " + ex.Message;
            }

            return RedirectToAction("Aprobaciones");
        }

        // ==========================
        // DETALLE DE UNA SOLICITUD
        // ==========================
        public IActionResult Detalle(int id)
        {
            try
            {
                var solicitud = _vacacionesBLL.ObtenerPorId(id);
                if (solicitud == null)
                {
                    TempData["Error"] = "Solicitud no encontrada.";
                    return RedirectToAction("MisVacaciones");
                }
                return View(solicitud);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar detalle: " + ex.Message;
                return RedirectToAction("MisVacaciones");
            }
        }
    }
}
