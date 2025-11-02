using System;
using System.ComponentModel.DataAnnotations;

namespace SETENA.GestionVacaciones.Models
{
    public class Feriado
    {
        [Key]
        public int IdFeriado { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [StringLength(100)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("^(Nacional|Institucional|Masivo)$", ErrorMessage = "Tipo inválido.")]
        public string Tipo { get; set; } = "Nacional";
    }
}
