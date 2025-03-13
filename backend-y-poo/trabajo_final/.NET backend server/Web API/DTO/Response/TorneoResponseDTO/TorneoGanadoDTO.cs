using DAO.Entidades.Cartas;
using DAO.Entidades.PartidaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabajo_Final.DTO.Response.TorneoResponseDTO
{
    //Jugador: ver torneos ganados
    public class TorneoGanadoDTO : TorneoBaseDTO
    {
        public int Id_ganador { get; set; }
        public string[] Series_habilitadas { get; set; }
        public int[] Id_cartas_mazo { get; set; }
        public int[] Id_partidas_jugadas { get; set; }


        //public int[] Id_jugadores { get; set; } //No se si puede tener esta informacion
    }
}
