namespace SETENA.GestionVacaciones.Models
{
    public class SolicitudVacaciones
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal? DiasSolicitados { get; set; }
        public string? Observaciones { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaDecision { get; set; }
        public int? IdJefatura { get; set; }
        public string? ComentarioJefatura { get; set; }
    }
}
