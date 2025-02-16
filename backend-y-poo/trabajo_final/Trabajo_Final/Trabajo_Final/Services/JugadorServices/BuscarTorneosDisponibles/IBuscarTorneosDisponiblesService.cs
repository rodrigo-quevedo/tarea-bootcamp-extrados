using Trabajo_Final.DTO.ListaTorneos;

namespace Trabajo_Final.Services.JugadorServices.BuscarTorneosDisponibles
{
    public interface IBuscarTorneosDisponiblesService
    {
        public Task<IList<TorneoDisponibleDTO>> BuscarTorneosDisponibles();
    }
}
