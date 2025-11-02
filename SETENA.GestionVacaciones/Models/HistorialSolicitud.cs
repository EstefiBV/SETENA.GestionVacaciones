using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SETENA.GestionVacaciones.Models
{
    public class HistorialSolicitud
    {
        [Key]
        public int IdHistorial { get; set; }

        [Required]
        public int IdSolicitud { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Accion { get; set; } // "Enviada", "Aprobada", "Rechazada"

        public DateTime FechaAccion { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string? Observaciones { get; set; }

        // Relaciones
        [ForeignKey("IdSolicitud")]
        public SolicitudVacaciones Solicitud { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }
}
