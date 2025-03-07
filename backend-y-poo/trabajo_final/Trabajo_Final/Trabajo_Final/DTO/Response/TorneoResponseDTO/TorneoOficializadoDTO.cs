using Trabajo_Final.DTO.Response.Jugador_Mazo;

namespace Trabajo_Final.DTO.Response.TorneoResponseDTO
{
    //Juez: ver torneos que oficializó
    public class TorneoOficializadoDTO : TorneoBaseDTO
    {
        public string[] Series_habilitadas { get; set; }
        public IdJugador_IdCartasMazoDTO[] IdJugadores_IdCartasMazos { get; set; }
        public int Id_ganador { get; set; }
        public int[] Id_partidas_oficializadas { get; set; }
    }
}
