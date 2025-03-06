using DAO.Entidades.TorneoEntidades;
using System.Runtime.CompilerServices;

namespace Trabajo_Final.DTO.Response.ResponseDTO
{
    //Organizador: Ver los torneos llenos, listos para iniciar
    public class TorneoLlenoDTO : Torneo
    {
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jueces { get; set; }
        public int[] Id_jugadores_inscriptos { get; set; }
    }
}
