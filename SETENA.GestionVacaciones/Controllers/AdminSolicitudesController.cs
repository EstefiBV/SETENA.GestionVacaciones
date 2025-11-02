using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;

namespace SETENA.GestionVacaciones.Controllers
{
    public class AdminSolicitudesController : Controller
    {
        private readonly SolicitudVacacionesBLL _solicitudBLL;
        private readonly UsuarioBLL _usuarioBLL;

        public AdminSolicitudesController()
        {
            _solicitudBLL = new SolicitudVacacionesBLL();
            _usuarioBLL = new UsuarioBLL();
        }

        // ==========================
        // LISTADO GENERAL DE SOLICITUDES
        // ==========================
        public IActionResult Index()
        {
            try
            {
                var solicitudes = _solicitudBLL.ObtenerPendientes(); // Puedes usar ObtenerTodas() si deseas todas
                return View(solicitudes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar las solicitudes: " + ex.Message;
                return View(new List<SolicitudVacaciones>());
            }
        }

        // ==========================
        // CAMBIAR ESTADO (Aprobada / Rechazada / Revisión)
        // ==========================
        [HttpPost]
        public IActionResult CambiarEstado(int id, string estado, string comentario = "")
        {
            try
            {
                // Se pasa null como IdJefatura ya que el admin no es jefatura directa
                _solicitudBLL.CambiarEstado(id, estado, comentario, null);
                TempData["Mensaje"] = $"La solicitud #{id} fue marcada como {estado}.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cambiar el estado: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ==========================
        // GESTIÓN DE USUARIOS
        // ==========================
        public IActionResult GestionUsuarios()
        {
            try
            {
                var usuarios = _usuarioBLL.ObtenerTodos();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar usuarios: " + ex.Message;
                return View(new List<Usuario>());
            }
        }

        // ==========================
        // ACTUALIZAR USUARIO (Nombre, Correo, Rol)
        // ==========================
        [HttpPost]
        public IActionResult ActualizarUsuario(UsuarioEdicionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Verifique los datos antes de guardar.";
                var usuarios = _usuarioBLL.ObtenerTodos();
                return View("GestionUsuarios", usuarios);
            }

            var usuario = _usuarioBLL.ObtenerPorId(model.Id);
            if (usuario == null)
                return NotFound();

            usuario.NombreCompleto = model.NombreCompleto;
            usuario.Correo = model.Correo;
            usuario.Rol = model.Rol;

            try
            {
                _usuarioBLL.ActualizarBasico(usuario);
                TempData["Mensaje"] = "Usuario actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar usuario: " + ex.Message;
            }

            return RedirectToAction("GestionUsuarios");
        }
    }
}
