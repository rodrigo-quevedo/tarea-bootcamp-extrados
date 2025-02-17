using Trabajo_Final.DTO.ListaTorneos;

namespace Trabajo_Final.Services.TorneoServices.BuscarTorneos
{
    public interface IBuscarTorneosService
    {
        public Task<IList<TorneoDTO>> BuscarTorneos(string[] fases);
    }
}
