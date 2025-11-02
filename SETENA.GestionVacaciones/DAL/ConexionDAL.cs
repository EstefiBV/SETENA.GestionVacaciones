using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SETENA.GestionVacaciones.DAL
{
    /// <summary>
    /// Gestiona la conexión directa con la base de datos SQL Server.
    /// Compatible con el contexto SetenaVacacionesV3 y las operaciones ADO.NET.
    /// </summary>
    public class ConexionDAL
    {
        private readonly string _cadenaConexion;

        public ConexionDAL()
        {
            // Construye la configuración leyendo appsettings.json
            var configuracion = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Se recomienda mantener el mismo nombre que usa EF Core para consistencia
            _cadenaConexion = configuracion.GetConnectionString("ConexionSQL");

        }

        /// <summary>
        /// Devuelve una conexión abierta lista para usar en consultas ADO.NET.
        /// </summary>
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }
    }
}
