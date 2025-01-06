using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Rol { get; set; }
        public string Pais { get; set; }
        public string Nombre_apellido {  get; set; }   
        public string Email {  get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; } //borrado logico (Activo = false)

    
        public Usuario(int id, string rol, string pais, string nombre_apellido, string email, string password, bool activo) { 
            this.Id = id;
            this.Rol = rol;
            this.Pais = pais;
            this.Nombre_apellido = nombre_apellido;
            this.Email = email;
            this.Password = password;
            this.Activo = activo;
        }
    }
}

