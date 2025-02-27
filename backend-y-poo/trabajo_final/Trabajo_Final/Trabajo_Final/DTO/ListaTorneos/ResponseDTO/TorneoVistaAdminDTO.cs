using DAO.Entidades.TorneoEntidades;

namespace Trabajo_Final.DTO.ListaTorneos.ResponseDTO
{
    //Admin: ver torneos
    public class TorneoVistaAdminDTO : Torneo
    {
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jueces { get; set; }
        public int[] Id_jugadores_inscriptos { get; set; }
        public int[]? Id_jugadores_aceptados { get; set; }
        public int? Id_ganador {  get; set; }
        public int[]? Id_partidas { get; set; }
    }
}
