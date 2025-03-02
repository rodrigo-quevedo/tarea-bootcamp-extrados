using Trabajo_Final.DTO.Torneos;

namespace Trabajo_Final.Services.TorneoServices.CancelarTorneo
{
    public interface ICancelarTorneoService
    {
        public Task<bool> CancelarTorneo(int id_admin, CancelarTorneoDTO dto);
    }
}
