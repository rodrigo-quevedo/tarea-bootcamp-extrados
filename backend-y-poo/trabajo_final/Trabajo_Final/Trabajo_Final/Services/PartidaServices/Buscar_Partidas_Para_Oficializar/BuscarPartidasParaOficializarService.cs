using DAO.DAOs.Partidas;
using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.PartidaServices.Buscar_Partidas_Para_Oficializar
{
    public class BuscarPartidasParaOficializarService : IBuscarPartidasParaOficializarService
    {
        IPartidaDAO partidaDAO;
        public BuscarPartidasParaOficializarService(IPartidaDAO partidaDao)
        {
            partidaDAO = partidaDao;
        }


        public async Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez)
        {
            return await partidaDAO.BuscarPartidasParaOficializar(id_juez);
        }
    }
}
