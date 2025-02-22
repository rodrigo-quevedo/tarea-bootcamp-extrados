using DAO.DAOs.Partidas;
using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.PartidaServices.Buscar_Partidas
{
    public class BuscarPartidasService : IBuscarPartidasService
    {
        IPartidaDAO partidaDAO;
        public BuscarPartidasService(IPartidaDAO partidaDao) 
        {
            partidaDAO = partidaDao;
        }


        public async Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez)
        {
            return await partidaDAO.BuscarPartidasParaOficializar(id_juez);
        }
    }
}
