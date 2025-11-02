using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SETENA.GestionVacaciones.Models
{
    public class SaldoVacaciones
    {
        [Key]
        public int IdSaldo { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int Periodo { get; set; } // Ejemplo: 2025

        [Required]
        [Range(0, 20, ErrorMessage = "Los días disponibles no pueden exceder 20.")]
        public decimal DiasDisponibles { get; set; }

        [Range(0, 40, ErrorMessage = "El acumulado no puede exceder 40 días (dos períodos).")]
        public decimal DiasAcumulados { get; set; }

        public DateTime UltimaActualizacion { get; set; } = DateTime.Now;

        // Relaciones
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
    }
}
