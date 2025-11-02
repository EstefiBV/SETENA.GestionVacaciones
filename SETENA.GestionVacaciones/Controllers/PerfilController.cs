using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;
using System.Security.Claims;

namespace SETENA.GestionVacaciones.Controllers
{
    public class PerfilController : Controller
    {
        private readonly UsuarioBLL _usuarioBLL;

        public PerfilController()
        {
            _usuarioBLL = new UsuarioBLL();
        }

        // Obtener usuario autenticado desde los claims
        private Usuario ObtenerUsuarioDesdeSesion()
        {
            var userCorreo = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userCorreo))
                return null;

            return _usuarioBLL.ObtenerPorCorreo(userCorreo);
        }

        // Mostrar perfil
        public IActionResult Perfil()
        {
            var usuario = ObtenerUsuarioDesdeSesion();
            if (usuario == null)
                return RedirectToAction("Login", "Cuenta");

            return View(usuario);
        }

        // Actualizar perfil
        [HttpPost]
        public async Task<IActionResult> Perfil(Usuario model, IFormFile Foto, string FotoPerfilRecortada)
        {
            var usuario = ObtenerUsuarioDesdeSesion();
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return View(model);
            }

            usuario.NombreCompleto = model.NombreCompleto;

            // Cambiar contraseña solo si el usuario la actualizó
            if (!string.IsNullOrEmpty(model.Contrasena))
                usuario.Contrasena = model.Contrasena;

            // === Procesar imagen recortada desde CropperJS ===
            if (!string.IsNullOrEmpty(FotoPerfilRecortada))
            {
                try
                {
                    var base64Data = FotoPerfilRecortada.Substring(FotoPerfilRecortada.IndexOf(',') + 1);
                    var bytes = Convert.FromBase64String(base64Data);

                    var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "perfil");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    var nombreArchivo = $"perfil_{usuario.Id}_{DateTime.Now:yyyyMMddHHmmss}.png";
                    var ruta = Path.Combine(carpeta, nombreArchivo);

                    await System.IO.File.WriteAllBytesAsync(ruta, bytes);
                    usuario.FotoPerfil = "/img/perfil/" + nombreArchivo;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error al guardar la imagen recortada: " + ex.Message;
                    return View(model);
                }
            }
            // === Si el usuario sube una imagen normal ===
            else if (Foto != null)
            {
                try
                {
                    var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "perfil");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    var extension = Path.GetExtension(Foto.FileName);
                    var nombreArchivo = $"perfil_{usuario.Id}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                    var ruta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await Foto.CopyToAsync(stream);
                    }

                    usuario.FotoPerfil = "/img/perfil/" + nombreArchivo;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error al subir la foto: " + ex.Message;
                    return View(model);
                }
            }

            // === Guardar cambios en la base de datos ===
            try
            {
                _usuarioBLL.Actualizar(usuario);

                // Refrescar sesión y claims
                var identity = (ClaimsIdentity)User.Identity;
                var fotoClaim = identity.FindFirst("FotoPerfil");
                if (fotoClaim != null)
                    identity.RemoveClaim(fotoClaim);

                identity.AddClaim(new Claim("FotoPerfil", usuario.FotoPerfil ?? "/img/default-user.png"));

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity)
                );

                TempData["Mensaje"] = "Perfil actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar el perfil: " + ex.Message;
            }

            // Actualizar el modelo para recargar la vista con los nuevos datos
            model.FotoPerfil = usuario.FotoPerfil;
            model.NombreCompleto = usuario.NombreCompleto;
            model.Correo = usuario.Correo;
            model.Rol = usuario.Rol;

            return View(model);
        }
    }
}
