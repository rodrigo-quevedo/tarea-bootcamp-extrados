using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.PartidaServices.Buscar_Partidas
{
    public interface IBuscarPartidasService
    {
        public Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez);
    }
}
