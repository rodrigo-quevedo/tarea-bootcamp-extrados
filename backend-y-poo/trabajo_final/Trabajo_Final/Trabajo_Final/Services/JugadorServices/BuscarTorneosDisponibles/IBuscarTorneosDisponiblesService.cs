using Trabajo_Final.DTO.Response.ResponseDTO;

namespace Trabajo_Final.Services.JugadorServices.BuscarTorneosDisponibles
{
    public interface IBuscarTorneosDisponiblesService
    {
        public Task<IList<TorneoDisponibleDTO>> BuscarTorneosDisponibles();
    }
}
