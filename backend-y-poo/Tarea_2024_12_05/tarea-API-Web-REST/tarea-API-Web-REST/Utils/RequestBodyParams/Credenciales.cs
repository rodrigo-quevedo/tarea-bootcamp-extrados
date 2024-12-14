using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarea_API_Web_REST.Utils.RequestBodyParams
{
    public class Credenciales
    {
        public string mail { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public void mostrarDatos()
        {
            Console.WriteLine($"Mail: {mail}");
            Console.WriteLine($"Username: {username}");
            Console.WriteLine($"Password: {password}");
        }
    }
}
