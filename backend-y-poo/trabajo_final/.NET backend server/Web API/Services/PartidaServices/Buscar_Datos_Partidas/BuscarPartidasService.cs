using DAO.DAOs.Partidas;
using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.PartidaServices.Buscar_Datos_Partidas
{
    public class BuscarPartidasService : IBuscarPartidasService
    {
        IPartidaDAO partidaDAO;
        public BuscarPartidasService(IPartidaDAO partidaDao)
        {
            partidaDAO = partidaDao;
        }

        public async Task<IEnumerable<Partida>> BuscarPartidas(
            string rol_logeado, int id_logeado, int[] id_partidas)
        {
            IEnumerable<Partida> result =
                await partidaDAO.BuscarPartidas(rol_logeado, id_logeado, id_partidas);

            if (result == null || !result.Any()) throw new Exception("No se pudo obtener ninguna partida buscada.");

            return result;
        }
    }
}
