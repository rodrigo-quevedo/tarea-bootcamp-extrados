using Trabajo_Final.DTO.ListaTorneos.ResponseDTO;

namespace Trabajo_Final.Services.TorneoServices.BuscarTorneos
{
    public interface IBuscarTorneosService
    {
        public Task<IList<TorneoVistaCompletaDTO>> BuscarTorneos(string[] fases, int id_organizador);
        public Task<IList<TorneoLlenoDTO>> BuscarTorneosLlenos(int id_organizador);

        public Task<IList<TorneoGanadoDTO>> BuscarTorneosGanados(int id_jugador);

        public Task<IList<TorneoOficializadoDTO>> BuscarTorneosOficializados(int id_juez);
    }
}
