using DAO.Entidades.PartidaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.Custom.Partida_CantidadRondas
{
    public class Partida_CantidadRondasDTO: Partida
    {
        public int Cantidad_rondas {  get; set; }
    }
}
