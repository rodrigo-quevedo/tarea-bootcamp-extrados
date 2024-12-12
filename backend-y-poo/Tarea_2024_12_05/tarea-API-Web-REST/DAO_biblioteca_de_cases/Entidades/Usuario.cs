using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO_biblioteca_de_cases.Entidades
{
    public class Usuario
    {
        public string mail { get; set; }
        public string nombre { get; set; }
        public int edad {  get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public Usuario (string mail, string nombre, int edad, string username, string password)
        {
            this.mail = mail;
            this.nombre = nombre;
            this.edad = edad;
            this.username = username;
            this.password = password;
        }

        public void mostrarDatos()
        {
            Console.WriteLine($"Mail: {this.mail}");
            Console.WriteLine($"Nombre: {this.nombre}");
            Console.WriteLine($"Edad: {this.edad}");
            Console.WriteLine($"Username: {this.username}");
            Console.WriteLine($"Password: {this.password}");
        }

    }
}
