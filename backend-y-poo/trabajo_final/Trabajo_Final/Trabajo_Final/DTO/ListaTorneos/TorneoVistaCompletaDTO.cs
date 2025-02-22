using DAO.Entidades.TorneoEntidades;

namespace Trabajo_Final.DTO.ListaTorneos
{
    public class TorneoVistaCompletaDTO : Torneo
    {
       
        public string[] series_habilitadas { get; set; }

        public int[] id_jueces_torneo { get; set; }

        public int[] id_jugadores_aceptados { get; set; }
    }
}
