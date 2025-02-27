using Trabajo_Final.DTO.ListaTorneos.ResponseDTO;

namespace Trabajo_Final.Services.JugadorServices.BuscarTorneosDisponibles
{
    public interface IBuscarTorneosDisponiblesService
    {
        public Task<IList<TorneoDisponibleDTO>> BuscarTorneosDisponibles();
    }
}
