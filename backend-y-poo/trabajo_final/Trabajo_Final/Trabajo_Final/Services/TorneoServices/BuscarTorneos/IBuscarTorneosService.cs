using DAO.Entidades.Custom.TorneoGanado;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.ListaTorneos;

namespace Trabajo_Final.Services.TorneoServices.BuscarTorneos
{
    public interface IBuscarTorneosService
    {
        public Task<IList<TorneoVistaCompletaDTO>> BuscarTorneos(string[] fases, int id_organizador);
        public Task<IList<TorneoLlenoDTO>> BuscarTorneosLlenos(int id_organizador);

        public Task<IList<TorneoGanadoDTO>> BuscarTorneosGanados(int id_jugador);
    }
}
