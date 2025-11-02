using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SETENA.GestionVacaciones.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SolicitudVacacionesBLL _solicitudBLL;
        private readonly FeriadoBLL _feriadoBLL;
        private readonly UsuarioBLL _usuarioBLL;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _solicitudBLL = new SolicitudVacacionesBLL();
            _feriadoBLL = new FeriadoBLL();
            _usuarioBLL = new UsuarioBLL();
        }

        // ?? Página principal del sistema
        public IActionResult Index()
        {
            return View();
        }

        // ?? Vista con feriados institucionales desde la base de datos
        public IActionResult Feriados()
        {
            try
            {
                var feriados = _feriadoBLL.ObtenerTodos();

                if (feriados.Count == 0)
                    ViewBag.Mensaje = "No hay feriados registrados en la base de datos.";

                _logger.LogInformation("Vista de feriados cargada correctamente.");
                return View(feriados);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cargar feriados: {ex.Message}");
                ViewBag.Error = "No se pudieron cargar los feriados. Inténtelo más tarde.";
                return View(new List<Feriado>());
            }
        }

        // ?? Vista del historial de solicitudes de vacaciones del usuario autenticado
        public IActionResult MisSolicitudes()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null)
            {
                TempData["Error"] = "Debe iniciar sesión para ver sus solicitudes.";
                return RedirectToAction("Login", "Cuenta");
            }

            int usuarioId = int.Parse(usuarioIdClaim.Value);

            try
            {
                var solicitudes = _solicitudBLL.ObtenerPorUsuario(usuarioId);
                var usuario = _usuarioBLL.ObtenerPorId(usuarioId);

                if (usuario == null)
                {
                    TempData["Error"] = "El usuario no fue encontrado.";
                    return RedirectToAction("Login", "Cuenta");
                }

                ViewBag.Nombre = usuario.NombreCompleto;
                ViewBag.Rol = usuario.Rol;
                ViewBag.Correo = usuario.Correo;

                _logger.LogInformation($"El usuario {usuario.NombreCompleto} consultó sus solicitudes de vacaciones.");
                return View(solicitudes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cargar solicitudes del usuario {usuarioId}: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al obtener sus solicitudes.";
                return RedirectToAction("Index");
            }
        }

        // ?? Manejo de errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
