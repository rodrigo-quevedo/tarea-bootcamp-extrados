using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO_biblioteca_de_cases.Entidades
{
    public class Credenciales
    {
        public string mail { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public void mostrarDatos()
        {
            Console.WriteLine($"Mail: {this.mail}");
            Console.WriteLine($"Username: {this.username}");
            Console.WriteLine($"Password: {this.password}");
        }
    }
}
