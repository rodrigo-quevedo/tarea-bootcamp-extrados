using DAO.Entidades.Custom;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.Partidas;

namespace Trabajo_Final.Services.PartidaServices.ArmarPartidasService
{
    public interface IArmarPartidasService
    {
        public IEnumerable<FechaHoraPartida> ArmarFechaHoraPartidas(
            DateTime fecha_hora_inicio,
            string horario_diario_inicio,
            string horario_diario_fin,
            int cantidad_partidas);


        public IEnumerable<DatosPartidaDTO> ArmarPartidas(
           int id_torneo,
           IList<FechaHoraPartida> fechaHoraPartidas,
           IList<Jugador_Inscripto> jugadoresPartida,
           IList<Juez_Torneo> jueces);

    }

}
 
