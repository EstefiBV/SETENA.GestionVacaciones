using Microsoft.Data.SqlClient;
using SETENA.GestionVacaciones.Models;
using System;
using System.Collections.Generic;

namespace SETENA.GestionVacaciones.DAL
{
    public class UsuarioDAL
    {
        private readonly ConexionDAL _conexion;

        public UsuarioDAL()
        {
            _conexion = new ConexionDAL();
        }

        // Autenticación básica
        public Usuario Autenticar(string correo, string contrasena)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"SELECT U.*, R.NombreRol 
                             FROM Usuarios U 
                             INNER JOIN Rol R ON U.RolId = R.Id
                             WHERE U.CorreoInstitucional = @Correo AND U.Contrasena = @Contrasena AND U.Activo = 1";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Correo", correo);
            cmd.Parameters.AddWithValue("@Contrasena", contrasena);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = (int)reader["IdUsuario"],
                    NombreCompleto = reader["NombreCompleto"].ToString(),
                    Correo = reader["CorreoInstitucional"].ToString(),
                    Rol = reader["NombreRol"].ToString(),
                    FotoPerfil = reader["FotoPerfil"] != DBNull.Value ? reader["FotoPerfil"].ToString() : null
                };
            }
            return null;
        }

        // Registrar un nuevo usuario (Funcionario por defecto)
        public bool Registrar(Usuario usuario)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"INSERT INTO Usuarios (NombreCompleto, CorreoInstitucional, Contrasena, RolId, Activo)
                             VALUES (@Nom, @Cor, @Con, @RolId, 1)";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Nom", usuario.NombreCompleto);
            cmd.Parameters.AddWithValue("@Cor", usuario.Correo);
            cmd.Parameters.AddWithValue("@Con", usuario.Contrasena);
            cmd.Parameters.AddWithValue("@RolId", ObtenerIdRol(usuario.Rol, con));

            return cmd.ExecuteNonQuery() > 0;
        }

        // Obtener ID del rol por nombre (Admin, Jefatura, Funcionario)
        private int ObtenerIdRol(string nombreRol, SqlConnection con)
        {
            string query = "SELECT Id FROM Rol WHERE NombreRol = @Rol";
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Rol", nombreRol);
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // Obtener usuario por correo
        public Usuario ObtenerPorCorreo(string correo)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"SELECT U.*, R.NombreRol 
                             FROM Usuarios U 
                             INNER JOIN Rol R ON U.RolId = R.Id
                             WHERE U.CorreoInstitucional = @Correo";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Correo", correo);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = (int)reader["IdUsuario"],
                    NombreCompleto = reader["NombreCompleto"].ToString(),
                    Correo = reader["CorreoInstitucional"].ToString(),
                    Contrasena = reader["Contrasena"].ToString(),
                    Rol = reader["NombreRol"].ToString(),
                    FotoPerfil = reader["FotoPerfil"] != DBNull.Value ? reader["FotoPerfil"].ToString() : null
                };
            }
            return null;
        }

        // Actualizar datos del usuario
        public void Actualizar(Usuario usuario)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = "UPDATE Usuarios SET NombreCompleto = @Nom, CorreoInstitucional = @Cor";

            if (!string.IsNullOrEmpty(usuario.Contrasena))
                query += ", Contrasena = @Con";

            if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                query += ", FotoPerfil = @Foto";

            query += ", RolId = @RolId WHERE IdUsuario = @Id";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Nom", usuario.NombreCompleto);
            cmd.Parameters.AddWithValue("@Cor", usuario.Correo);
            cmd.Parameters.AddWithValue("@RolId", ObtenerIdRol(usuario.Rol, con));
            cmd.Parameters.AddWithValue("@Id", usuario.Id);

            if (!string.IsNullOrEmpty(usuario.Contrasena))
                cmd.Parameters.AddWithValue("@Con", usuario.Contrasena);

            if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                cmd.Parameters.AddWithValue("@Foto", usuario.FotoPerfil);

            cmd.ExecuteNonQuery();
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"SELECT U.IdUsuario, U.NombreCompleto, U.CorreoInstitucional, 
                            U.Contrasena, U.FotoPerfil, U.Activo, 
                            R.NombreRol 
                     FROM Usuarios U
                     INNER JOIN Rol R ON R.Id = U.RolId
                     WHERE U.IdUsuario = @Id";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", idUsuario);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = (int)reader["IdUsuario"],
                    NombreCompleto = reader["NombreCompleto"].ToString(),
                    Correo = reader["CorreoInstitucional"].ToString(),
                    Contrasena = reader["Contrasena"].ToString(),
                    Rol = reader["NombreRol"].ToString(),
                    FotoPerfil = reader["FotoPerfil"] != DBNull.Value ? reader["FotoPerfil"].ToString() : null
                };
            }

            return null;
        }
        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"SELECT U.IdUsuario, U.NombreCompleto, U.CorreoInstitucional, 
                            U.FotoPerfil, U.Contrasena, U.Activo, 
                            R.NombreRol
                     FROM Usuarios U
                     INNER JOIN Rol R ON U.RolId = R.Id";

            using var cmd = new SqlCommand(query, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    Id = (int)reader["IdUsuario"],
                    NombreCompleto = reader["NombreCompleto"].ToString(),
                    Correo = reader["CorreoInstitucional"].ToString(),
                    Contrasena = reader["Contrasena"].ToString(),
                    Rol = reader["NombreRol"].ToString(),
                    FotoPerfil = reader["FotoPerfil"] != DBNull.Value ? reader["FotoPerfil"].ToString() : null
                });
            }

            return lista;
        }
        public void ActualizarBasico(Usuario usuario)
        {
            using var con = _conexion.ObtenerConexion();
            con.Open();

            string query = @"UPDATE Usuarios 
                     SET NombreCompleto = @Nombre,
                         CorreoInstitucional = @Correo,
                         RolId = (SELECT Id FROM Rol WHERE NombreRol = @Rol)
                     WHERE IdUsuario = @IdUsuario";

            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Nombre", usuario.NombreCompleto);
            cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@IdUsuario", usuario.Id);

            cmd.ExecuteNonQuery();
        }



    }
}
