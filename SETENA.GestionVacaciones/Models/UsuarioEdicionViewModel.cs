using System.ComponentModel.DataAnnotations;

namespace SETENA.GestionVacaciones.Models
{
    /// <summary>
    /// ViewModel para editar información básica del usuario (desde el perfil o módulo de administración).
    /// No gestiona contraseñas ni solicitudes directamente.
    /// </summary>
    public class UsuarioEdicionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [RegularExpression("^(Funcionario|Jefatura|Administrador)$",
            ErrorMessage = "El rol debe ser Funcionario, Jefatura o Administrador.")]
        public string Rol { get; set; }

        [Display(Name = "Foto de Perfil")]
        public string? FotoPerfil { get; set; } // opcional, útil si se edita en perfil

        [Display(Name = "Activo en el sistema")]
        public bool Activo { get; set; } = true; // Nuevo: permite desactivar usuarios sin eliminarlos

        // Campo informativo, no editable
        [Display(Name = "Último acceso")]
        public DateTime? UltimoAcceso { get; set; }
    }
}
