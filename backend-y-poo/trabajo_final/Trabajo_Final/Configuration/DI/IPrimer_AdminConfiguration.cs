using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.DI
{
    public interface IPrimer_AdminConfiguration
    {
        public string Pais { get; set; }
        public string Nombre_apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
       
    }
}
