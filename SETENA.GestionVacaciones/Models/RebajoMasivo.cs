using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SETENA.GestionVacaciones.Models
{
    public class RebajoMasivo
    {
        [Key]
        public int IdRebajo { get; set; }

        [StringLength(100)]
        public string? Descripcion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public int IdAdministrador { get; set; }

        [ForeignKey("IdAdministrador")]
        public Usuario Administrador { get; set; }
    }
}
