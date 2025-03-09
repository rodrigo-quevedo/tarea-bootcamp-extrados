using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.PartidaServices.Buscar_Partidas_Para_Oficializar
{
    public interface IBuscarPartidasParaOficializarService
    {
        public Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez);
    }
}
