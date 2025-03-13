using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.TorneoEntidades
{
    public class Jugador_Inscripto
    {
        public int Id_jugador {  get; set; }
        public int Id_torneo { get; set; }
        public int Orden {  get; set; }
        public bool Aceptado {  get; set; }
    }
}
