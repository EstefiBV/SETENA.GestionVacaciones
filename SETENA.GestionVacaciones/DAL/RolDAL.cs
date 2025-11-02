using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class RolDAL
    {
        private readonly ConexionDAL _conexion;

        public RolDAL()
        {
            _conexion = new ConexionDAL();
        }

        public List<Rol> ObtenerRoles()
        {
            var lista = new List<Rol>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "SELECT * FROM Rol ORDER BY NombreRol";
            using var cmd = new SqlCommand(query, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Rol
                {
                    Id = (int)reader["Id"],
                    NombreRol = reader["NombreRol"].ToString()
                });
            }

            return lista;
        }
    }
}
