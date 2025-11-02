using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SETENA.GestionVacaciones.BILL;
using SETENA.GestionVacaciones.Models;
using System.Security.Claims;

namespace SETENA.GestionVacaciones.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UsuarioBLL _usuarioBLL;

        public CuentaController()
        {
            _usuarioBLL = new UsuarioBLL();
        }

        // ==========================
        // LOGIN (GET)
        // ==========================
        public IActionResult Login() => View();

        // ==========================
        // LOGIN (POST)
        // ==========================
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
            var usuario = _usuarioBLL.Autenticar(correo, contrasena);

            if (usuario != null)
            {
                // Claims del usuario autenticado
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Rol ?? "Funcionario"),
                    new Claim("FotoPerfil", usuario.FotoPerfil ?? "/img/default-user.png")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Mantener sesión aunque cierre navegador (puedes cambiarlo)
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirección según el rol
                switch (usuario.Rol)
                {
                    case "Administrador":
                    case "Administrador Principal":
                        return RedirectToAction("Index", "Admin");

                    case "Jefatura":
                        return RedirectToAction("PendientesJefatura", "SolicitudVacaciones");

                    case "Funcionario":
                    default:
                        return RedirectToAction("MisSolicitudes", "SolicitudVacaciones");
                }
            }

            ViewBag.Mensaje = "Correo o contraseña incorrectos.";
            return View();
        }

        // ==========================
        // REGISTRO (GET)
        // ==========================
        public IActionResult Registro() => View();

        // ==========================
        // REGISTRO (POST)
        // ==========================
        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            usuario.Rol = "Funcionario"; // Por defecto al crear
            var resultado = _usuarioBLL.Registrar(usuario);

            if (resultado)
            {
                TempData["Mensaje"] = "Usuario registrado correctamente.";
                return RedirectToAction("Login");
            }

            ViewBag.Mensaje = "Error al registrar el usuario.";
            return View(usuario);
        }

        // ==========================
        // LOGOUT
        // ==========================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
