using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Primer_Admin
{
    public class Primer_AdminConfiguration : IPrimer_AdminConfiguration
    {
        public string Pais { get; set; }
        public string Nombre_apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public Primer_AdminConfiguration(
            string pais,
            string nombre_apellido,
            string email,
            string password
        )
        {
            Pais = pais;
            Nombre_apellido = nombre_apellido;
            Email = email;
            Password = password;
        }

        public Primer_AdminConfiguration() { }



    }



}
