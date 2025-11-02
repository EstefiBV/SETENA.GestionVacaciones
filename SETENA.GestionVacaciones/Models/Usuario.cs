using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SETENA.GestionVacaciones.Models
{
    public class Usuario
    {
        // Identificador único del usuario
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        public string Correo { get; set; }

        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
        public string Contrasena { get; set; }

        // Ruta o nombre de archivo de imagen guardada en el servidor
        public string? FotoPerfil { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(50)]
        public string Rol { get; set; }  // Ejemplo: "Administrador", "Jefatura", "Funcionario"

        // Control de vacaciones y saldo
        [Range(0, 30, ErrorMessage = "El saldo de vacaciones no puede ser negativo ni mayor a 30 días.")]
        public decimal SaldoVacaciones { get; set; }  // Días acumulados actualmente

        [Range(0, 20, ErrorMessage = "El tope máximo anual es de 20 días según la Ley 10159.")]
        public decimal DiasGanadosAnualmente { get; set; } = 20;  // Días que gana cada año

        public DateTime FechaIngreso { get; set; } = DateTime.Now;  // Para cálculos proporcionales

        // Control de auditoría y registro de rebajos
        public bool EsActivo { get; set; } = true;  // Para desactivar usuarios sin borrarlos
        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? ComentarioUltimaActualizacion { get; set; }  // Ej: "Rebajo masivo aplicado"

        // Relaciones con otras tablas (por ejemplo, solicitudes de vacaciones)
        public List<SolicitudVacaciones> Solicitudes { get; set; } = new List<SolicitudVacaciones>();
    }
}
