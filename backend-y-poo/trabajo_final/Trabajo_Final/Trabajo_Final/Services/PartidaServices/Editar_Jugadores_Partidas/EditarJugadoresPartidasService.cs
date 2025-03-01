using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.EditarPartidas;

namespace Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas
{
    public class EditarJugadoresPartidasService : IEditarJugadoresPartidasService
    {
        private IPartidaDAO partidaDAO;
        private ITorneoDAO torneoDAO;
        public EditarJugadoresPartidasService(IPartidaDAO partidaDao, ITorneoDAO torneoDao) 
        { 
            partidaDAO = partidaDao;
            torneoDAO = torneoDao;
        }

        public async Task<bool> EditarJugadoresDePartidas(
            int id_organizador, 
            JugadoresPartida[] jugadores_partidas)
        {
            //verificar partidas:
            // - son partidas de un mismo torneo
            // - el torneo fue organizado por id_organizador
            // - las partidas pertenecen a la ronda 1
            IEnumerable<Partida> partidasVerificadas =
                await partidaDAO.VerificarPartidas_EditarJugadoresPartida(
                    jugadores_partidas.Select(j => j.Id_partida),
                    id_organizador);


            //verificar jugadores
            // - no hay jugadores NULL ->
            //      tiraria error al parsear el JSON del Request body (ya que id_jugador es int)
            // - todos los jugadores son jugadores aceptados del torneo
            IEnumerable<Torneo> busqueda = Enumerable.Empty<Torneo>();

            IEnumerable<Jugador_Inscripto> jugadores_aceptados =
                await torneoDAO.BuscarJugadoresAceptados( 
                    busqueda.Append(
                        new Torneo() { Id = partidasVerificadas.First().Id_torneo }
                    )
                );



            // - al tomarlos junto a los jugadores de la db, no hay jugadores repetidos


            throw new NotImplementedException();
        }
    }
}
