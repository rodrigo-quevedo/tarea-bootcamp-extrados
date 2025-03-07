using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.JugadorServices.BuscarPartidas
{
    public interface IBuscarPartidasService
    {
        public Task<IEnumerable<Partida>> BuscarDescalificaciones(int id_jugador);

        public Task<IEnumerable<Partida>> BuscarPartidasGanadas(int id_jugador);

        public Task<IEnumerable<Partida>> BuscarPartidasPerdidas(int id_jugador);
    }
}
