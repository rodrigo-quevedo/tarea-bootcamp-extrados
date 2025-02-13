using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.UsuarioEntidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Rol { get; set; }
        public string Pais { get; set; }
        public string Nombre_apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; } //borrado logico (Activo = false)
        public int? Id_usuario_creador { get; set; }
        public string Refresh_token { get; set; }

        public Usuario() { }
        public Usuario(int id, string rol, string pais, string nombre_apellido, string email, string password, bool activo, int? id_usuario_creador, string refresh_token)
        {
            Id = id;
            Rol = rol;
            Pais = pais;
            Nombre_apellido = nombre_apellido;
            Email = email;
            Password = password;
            Activo = activo;
            Id_usuario_creador = id_usuario_creador;
            Refresh_token = refresh_token;
        }

        //compatible hacia atras (algunos services ya están implementados con Usuarios sin refresh_token)
        public Usuario(int id, string rol, string pais, string nombre_apellido, string email, string password, bool activo, int? id_usuario_creador)
        {
            Id = id;
            Rol = rol;
            Pais = pais;
            Nombre_apellido = nombre_apellido;
            Email = email;
            Password = password;
            Activo = activo;
            Id_usuario_creador = id_usuario_creador;
        }

        //busqueda por id
        public Usuario(int id, bool activo)
        {
            Id = id;
            Activo = activo;
        }

        //busqueda por email
        public Usuario(string email, bool activo)
        {
            Email = email;
            Activo = activo;
        }
    }
}

