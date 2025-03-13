using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.PartidaServices.Buscar_Datos_Partidas
{
    public interface IBuscarPartidasService
    {
        public Task<IEnumerable<Partida>> BuscarPartidas(
            string rol_logeado, int id_logeado, int[] id_partidas);
    }

}
