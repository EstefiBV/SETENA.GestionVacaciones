using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SETENA.GestionVacaciones.Models
{
    /// <summary>
    /// Representa una unidad organizacional de SETENA. 
    /// Permite asociar usuarios (funcionarios y jefaturas) al flujo de aprobación.
    /// </summary>
    public class Departamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        public string? Descripcion { get; set; }

        [Display(Name = "Jefatura del Departamento")]
        public int? IdJefatura { get; set; } // Relación con el usuario jefatura

        // Navegación
        public Usuario? Jefatura { get; set; }
        public List<Usuario>? Funcionarios { get; set; }
    }
}
