using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;
using System.IO;

namespace SETENA.GestionVacaciones.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioBLL _usuarioBLL;

        public UsuarioController()
        {
            _usuarioBLL = new UsuarioBLL();
        }

        // ========================
        // PERFIL DE USUARIO
        // ========================
        public IActionResult Perfil()
        {
            // Simulación de usuario activo (debería venir de sesión o autenticación)
            string correo = "usuario@setena.go.cr";
            var usuario = _usuarioBLL.ObtenerPorCorreo(correo);

            if (usuario == null)
            {
                TempData["Error"] = "No se encontró el usuario en la base de datos.";
                return RedirectToAction("Login", "Cuenta");
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Perfil(Usuario usuario, string FotoPerfilRecortada)
        {
            if (!string.IsNullOrEmpty(FotoPerfilRecortada))
            {
                usuario.FotoPerfil = FotoPerfilRecortada;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _usuarioBLL.Actualizar(usuario);
                    TempData["Mensaje"] = "Perfil actualizado correctamente.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al actualizar el perfil: {ex.Message}";
                }
            }

            return View(usuario);
        }

        // ========================
        // LISTA DE USUARIOS (solo admin)
        // ========================
        public IActionResult ListaUsuarios()
        {
            var usuarios = _usuarioBLL.ObtenerTodos();

            // Validación por rol (debería venir de sesión, por ahora fijo)
            string rolActual = "Administrador";
            if (rolActual != "Administrador")
            {
                TempData["Error"] = "No tiene permisos para acceder a esta sección.";
                return RedirectToAction("Perfil");
            }

            return View(usuarios);
        }

        // ========================
        // DETALLES DE USUARIO
        // ========================
        public IActionResult Detalle(int id)
        {
            var usuario = _usuarioBLL.ObtenerPorId(id);
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction("ListaUsuarios");
            }

            return View(usuario);
        }
    }
}
