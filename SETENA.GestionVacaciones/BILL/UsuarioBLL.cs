using SETENA.GestionVacaciones.DAL;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.BILL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _usuarioDAL;
        private readonly RolDAL _rolDAL;
        private readonly AuditoriaDAL _auditoriaDAL;

        public UsuarioBLL()
        {
            _usuarioDAL = new UsuarioDAL();
            _rolDAL = new RolDAL();
            _auditoriaDAL = new AuditoriaDAL();
        }

        // 🔐 Autenticación básica del usuario
        public Usuario Autenticar(string correo, string contrasena)
        {
            var usuario = _usuarioDAL.Autenticar(correo, contrasena);
            if (usuario != null)
            {
                _auditoriaDAL.RegistrarAccion(usuario.Id, "Autenticación", "Inicio de sesión exitoso");
            }
            return usuario;
        }

        // 🧾 Registrar un nuevo usuario con validación de duplicado
        public bool Registrar(Usuario usuario)
        {
            var existente = _usuarioDAL.ObtenerPorCorreo(usuario.Correo);
            if (existente != null)
                throw new System.Exception("Ya existe un usuario con este correo institucional.");

            bool registrado = _usuarioDAL.Registrar(usuario);
            if (registrado)
            {
                _auditoriaDAL.RegistrarAccion(usuario.Id, "Usuarios", $"Registro de nuevo usuario: {usuario.NombreCompleto}");
            }
            return registrado;
        }

        // Obtener un usuario por correo
        public Usuario ObtenerPorCorreo(string correo) =>
            _usuarioDAL.ObtenerPorCorreo(correo);

        // Obtener un usuario por ID
        public Usuario ObtenerPorId(int id) =>
            _usuarioDAL.ObtenerPorId(id);

        // Obtener todos los usuarios
        public List<Usuario> ObtenerTodos() =>
            _usuarioDAL.ObtenerTodos();

        // Actualizar datos del usuario
        public void Actualizar(Usuario usuario)
        {
            _usuarioDAL.Actualizar(usuario);
            _auditoriaDAL.RegistrarAccion(usuario.Id, "Usuarios", $"Actualización de perfil de {usuario.NombreCompleto}");
        }

        // Actualización simplificada (sin contraseña)
        public void ActualizarBasico(Usuario usuario)
        {
            _usuarioDAL.ActualizarBasico(usuario);
            _auditoriaDAL.RegistrarAccion(usuario.Id, "Usuarios", $"Actualización básica de {usuario.NombreCompleto}");
        }

        // ⚙️ Obtener lista de roles del sistema
        public List<Rol> ObtenerRoles() =>
            _rolDAL.ObtenerRoles();
    }
}
