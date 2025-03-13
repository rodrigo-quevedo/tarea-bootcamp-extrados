using DAO.Entidades.TorneoEntidades;

namespace Trabajo_Final.DTO.Response.TorneoResponseDTO
{
    //Organizador: Ver los torneos llenos, listos para iniciar
    public class TorneoLlenoDTO : Torneo
    {
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jueces { get; set; }
        public int[] Id_jugadores_inscriptos { get; set; }
    }
}
