using DAO.DAOs.Partidas;
using DAO.Entidades.Custom.Descalificaciones;

namespace Trabajo_Final.Services.JugadorServices.BuscarDescalificaciones
{
    public class BuscarDescalificacionesService : IBuscarDescalificacionesService
    {
        IPartidaDAO partidaDAO;
        public BuscarDescalificacionesService(IPartidaDAO partidaDao)
        {
            partidaDAO = partidaDao;
        }

        public async Task<IEnumerable<DescalificacionDTO>> BuscarDescalificaciones(int id_jugador)
        {
            return await partidaDAO.BuscarDescalificaciones(id_jugador);

        }
    }
}
