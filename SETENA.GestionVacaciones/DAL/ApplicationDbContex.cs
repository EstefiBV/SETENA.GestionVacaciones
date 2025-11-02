using Microsoft.EntityFrameworkCore;
using SETENA.GestionVacaciones.Models;

namespace SETENA.GestionVacaciones.DAL
{
    /// <summary>
    /// Contexto principal de la base de datos SetenaVacacionesV3.
    /// Administra todas las entidades y sus relaciones según el modelo EF Core.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // === ENTIDADES PRINCIPALES ===
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Rol> Roles { get; set; }

        // === VACACIONES Y GESTIÓN ===
        public DbSet<SolicitudVacaciones> SolicitudesVacaciones { get; set; }
        public DbSet<SaldoVacaciones> SaldosVacaciones { get; set; }

        // === CONTROL ADMINISTRATIVO ===
        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<RebajoMasivo> RebajosMasivos { get; set; }
        public DbSet<HistorialSolicitud> HistorialSolicitudes { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        /// <summary>
        /// Configura los nombres de las tablas y las relaciones
        /// para que coincidan exactamente con la estructura SQL existente.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Asignación de nombres de tabla SQL exactos ===
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Departamento>().ToTable("Departamentos");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<SolicitudVacaciones>().ToTable("SolicitudesVacaciones");
            modelBuilder.Entity<SaldoVacaciones>().ToTable("SaldosVacaciones");
            modelBuilder.Entity<Feriado>().ToTable("Feriados");
            modelBuilder.Entity<RebajoMasivo>().ToTable("RebajosMasivos");
            modelBuilder.Entity<HistorialSolicitud>().ToTable("HistorialSolicitudes");
            modelBuilder.Entity<Auditoria>().ToTable("Auditoria");

            // === Relaciones personalizadas ===

            // Un departamento tiene una jefatura (usuario)
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Jefatura)
                .WithMany()
                .HasForeignKey(d => d.IdJefatura)
                .OnDelete(DeleteBehavior.Restrict);

            // Un usuario pertenece a un departamento
            modelBuilder.Entity<Usuario>()
                .HasOne<Departamento>()
                .WithMany(d => d.Funcionarios)
                .HasForeignKey("IdDepartamento")
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario tiene un rol
            modelBuilder.Entity<Usuario>()
                .HasOne<Rol>()
                .WithMany()
                .HasForeignKey("RolId")
                .OnDelete(DeleteBehavior.Restrict);

            // Solicitud de vacaciones pertenece a un usuario
            modelBuilder.Entity<SolicitudVacaciones>()
                .HasOne<Usuario>()
                .WithMany(u => u.Solicitudes)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre historial y solicitud
            modelBuilder.Entity<HistorialSolicitud>()
                .HasOne(h => h.Solicitud)
                .WithMany()
                .HasForeignKey(h => h.IdSolicitud)
                .OnDelete(DeleteBehavior.Cascade);

            // Auditoría -> Usuario
            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
