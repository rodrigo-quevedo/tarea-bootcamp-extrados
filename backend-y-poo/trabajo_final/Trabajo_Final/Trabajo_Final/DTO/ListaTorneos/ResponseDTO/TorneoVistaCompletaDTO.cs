using DAO.Entidades.TorneoEntidades;

namespace Trabajo_Final.DTO.ListaTorneos.ResponseDTO
{
    //Organizador: ver torneos organizados por sí mismo
    //Admin: ver torneos
    public class TorneoVistaCompletaDTO : Torneo
    {
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jueces { get; set; }
        public int[] Id_jugadores { get; set; }
        public int? Id_ganador {  get; set; }
        public int[] Id_partidas { get; set; }
    }
}
