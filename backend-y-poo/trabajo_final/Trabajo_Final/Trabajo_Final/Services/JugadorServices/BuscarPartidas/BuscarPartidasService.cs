using DAO.DAOs.Partidas;
using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.JugadorServices.BuscarPartidas
{
    public class BuscarPartidasService : IBuscarPartidasService
    {
        IPartidaDAO partidaDAO;
        public BuscarPartidasService(IPartidaDAO partidaDao)
        {
            partidaDAO = partidaDao;
        }

        public async Task<IEnumerable<Partida>> BuscarDescalificaciones(int id_jugador)
        {
            return await partidaDAO.BuscarDescalificaciones(id_jugador);

        }

        public async Task<IEnumerable<Partida>> BuscarPartidasGanadas(int id_jugador)
        {
            return await partidaDAO.BuscarPartidasGanadas(id_jugador);
        }

        public async Task<IEnumerable<Partida>> BuscarPartidasPerdidas(int id_jugador)
        {
            return await partidaDAO.BuscarPartidasPerdidas(id_jugador);
        }
    }
}
