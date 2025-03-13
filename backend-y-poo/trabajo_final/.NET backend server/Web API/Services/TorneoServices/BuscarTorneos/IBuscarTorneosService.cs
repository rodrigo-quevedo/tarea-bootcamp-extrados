using Trabajo_Final.DTO.Response.TorneoResponseDTO;

namespace Trabajo_Final.Services.TorneoServices.BuscarTorneos
{
    public interface IBuscarTorneosService
    {
        public Task<IList<TorneoVistaAdminDTO>> BuscarTorneos(string[] fases);//admin
        public Task<IList<TorneoOrganizadoDTO>> BuscarTorneosOrganizados(string[] fases, int id_organizador);
        public Task<IList<TorneoLlenoDTO>> BuscarTorneosParaIniciar(int id_organizador);

        public Task<IList<TorneoDisponibleDTO>> BuscarTorneosDisponibles();
        public Task<IList<TorneoInscriptoDTO>> BuscarTorneosInscriptos(int id_jugador);
        public Task<IList<TorneoGanadoDTO>> BuscarTorneosGanados(int id_jugador);

        public Task<IList<TorneoOficializadoDTO>> BuscarTorneosOficializados(int id_juez);
    }
}
