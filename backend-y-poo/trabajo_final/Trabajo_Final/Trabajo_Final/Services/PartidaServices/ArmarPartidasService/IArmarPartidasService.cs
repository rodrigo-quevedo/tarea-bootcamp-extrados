using DAO.DTOs_en_DAOs.InsertPartidas;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.ProcesamientoDatos.FechaHoraPartidas;

namespace Trabajo_Final.Services.PartidaServices.ArmarPartidasService
{
    public interface IArmarPartidasService
    {
        public IEnumerable<FechaHoraPartida> ArmarFechaHoraPartidas(
            DateTime fecha_hora_inicio,
            string horario_diario_inicio,
            string horario_diario_fin,
            int cantidad_partidas);


        public IEnumerable<InsertPartidaDTO> ArmarPartidas_JugadoresAleatorios(
           int id_torneo,
           IList<FechaHoraPartida> fechaHoraPartidas,
           IList<Jugador_Inscripto> jugadoresPartida,
           IList<Juez_Torneo> jueces,
           int ronda);


        public IEnumerable<InsertPartidaDTO> ArmarPartidas_JugadoresEnOrdenCronologico(
            int id_torneo,
            IList<FechaHoraPartida> fechaHoraPartidas,
            IList<Partida> ganadores_ronda_anterior,
            IList<Juez_Torneo> jueces,
            int ronda);

    }

}
 
