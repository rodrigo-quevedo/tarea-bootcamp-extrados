using DAO.Entidades.PartidaEntidades;

namespace Trabajo_Final.Services.TorneoServices.IniciarTorneo
{
    public interface IIniciarTorneoService
    {
        public Task<bool> IniciarTorneo(int id_torneo, int id_organizador);
    }
}
