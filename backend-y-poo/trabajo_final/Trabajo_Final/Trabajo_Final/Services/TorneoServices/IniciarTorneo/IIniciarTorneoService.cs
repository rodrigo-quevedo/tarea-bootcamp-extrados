using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.TorneoServices.IniciarTorneo
{
    public interface IIniciarTorneoService
    {
        public Task<IEnumerable<Partida>> IniciarTorneo(int id_torneo);
    }
}
