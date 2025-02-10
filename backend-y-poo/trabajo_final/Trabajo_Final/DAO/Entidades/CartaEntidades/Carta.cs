using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.Cartas
{
    public class Carta
    {
        public int Id { get; set; }
        public int Ataque {  get; set; }
        public int Defensa { get; set; }
        public string Ilustracion { get; set; }
    }
}
