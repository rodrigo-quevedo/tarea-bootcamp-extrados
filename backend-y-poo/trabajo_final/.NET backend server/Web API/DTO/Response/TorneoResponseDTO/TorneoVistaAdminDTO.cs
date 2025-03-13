using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.Response.Jugador_Mazo;

namespace Trabajo_Final.DTO.Response.TorneoResponseDTO
{
    //Admin: ver torneos
    public class TorneoVistaAdminDTO : Torneo
    {
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jueces { get; set; }
        public int[] Id_jugadores_inscriptos { get; set; }
        public IdJugador_IdCartasMazoDTO[]? IdJugadoresAceptados_IdCartasMazos { get; set; }
        public int? Id_ganador { get; set; }
        public int[]? Id_partidas { get; set; }
    }
}
