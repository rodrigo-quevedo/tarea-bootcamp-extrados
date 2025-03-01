using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.Entidades.Custom.JugadoresPartidas;
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
            //la búsqueda también verifica las partidas:
            // - son partidas de un mismo torneo
            // - el torneo fue organizado por id_organizador
            // - las partidas pertenecen a la ronda 1
            int id_torneo = await partidaDAO.BuscarIdTorneoVerificandoPartidas(
                jugadores_partidas.Select(j => j.Id_partida),
                id_organizador,
                1);


            IEnumerable<Partida> partidasPrimeraRonda =
                await partidaDAO.BuscarPartidasPrimeraRonda(id_torneo);

            //verificar jugadores
            // - no hay jugadores NULL -> si hubiera alguno null, tiraria error al parsear el
            //JSON del Request body (ya que id_jugador es int)
            // - todos los jugadores son jugadores aceptados del torneo
            IEnumerable<Torneo> busqueda = Enumerable.Empty<Torneo>();

            IEnumerable<Jugador_Inscripto> jugadores_aceptados =
                await torneoDAO.BuscarJugadoresAceptados(
                    busqueda.Append(
                        new Torneo() { Id = id_torneo }
                    )
                );

            IList<int> id_jugadores_aceptados =
                jugadores_aceptados.Select(j => j.Id_jugador).ToList();

            IList<int> id_jugadores_editar =
                jugadores_partidas.Select(j => j.Id_jugador_1)
                .Concat(jugadores_partidas.Select(j => j.Id_jugador_2))
                .ToList();

            foreach (int id_jugador_editar in id_jugadores_editar)
            {
                if (!id_jugadores_aceptados.Contains(id_jugador_editar))
                    throw new InvalidInputException($"El jugador [{id_jugador_editar}] no participa en el torneo de la partida que se quiere editar.");
            }



            // - al tomarlos junto a los jugadores de la db (de partidas que NO VAN A SER
            // EDITADAS), no hay jugadores repetidos:
            IList<int> id_partidas_para_editar = jugadores_partidas.Select(p => p.Id_partida).ToList();

            IList<Partida> partidas_sin_editar =
                partidasPrimeraRonda
                .Where(partida => !id_partidas_para_editar.Contains(partida.Id))
                .ToList();

            IList<int> id_jugadores_de_partidas_sin_editar =
                partidas_sin_editar.Select(p => p.Id_jugador_1)
                .Concat(partidas_sin_editar.Select(p => p.Id_jugador_2))
                .ToList();


            IList<int> id_jugadores_resultante =
                id_jugadores_editar
                .Concat(id_jugadores_de_partidas_sin_editar)
                .ToList();

            IEnumerable<IGrouping<int, int>> idRepetidas_agrupadas =
                id_jugadores_resultante
                .GroupBy(id => id)
                .Where(grupo => grupo.Count() > 1);

            IList<int> id_jugadores_repetidos = new List<int>();
            foreach(var grupo in idRepetidas_agrupadas)
            {
                id_jugadores_repetidos.Add(grupo.Key);
            }

            if (idRepetidas_agrupadas.Any())
                throw new InvalidInputException(
                    $"Los siguientes IDs de jugador quedarían repetidos" +
                    $"en las partidas de la primera ronda: " +
                    $"[{String.Join(", ", id_jugadores_repetidos)}]." +
                    $"Revise las partidas y los jugadores.");


            //DAO: UPDATE partidas
            return await partidaDAO.EditarJugadoresPartidas(jugadores_partidas.AsEnumerable());
        }
    }
}
