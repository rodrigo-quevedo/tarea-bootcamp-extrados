using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.TorneoEntidades
{
    public class Carta_Del_Mazo
    {
        public int Id_jugador {  get; set; }
        public int Id_carta { get; set; }
        public int Id_torneo { get; set; }
    }
}
