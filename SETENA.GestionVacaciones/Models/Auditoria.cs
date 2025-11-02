using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SETENA.GestionVacaciones.Models
{
    public class Auditoria
    {
        [Key]
        public int IdAuditoria { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [StringLength(50)]
        public string? ModuloAfectado { get; set; }

        [StringLength(255)]
        public string? DescripcionAccion { get; set; }

        public DateTime FechaAccion { get; set; } = DateTime.Now;

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }
}
