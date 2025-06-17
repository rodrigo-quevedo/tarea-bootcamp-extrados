using Trabajo_Final.DTO.Response.Cartas;

namespace Trabajo_Final.Services.CartasServices.BuscarCartas
{
    public interface IBuscarCartasService
    {
        public Task<IEnumerable<DatosCartaDTO>> BuscarCartas(int[] id_cartas);

        public Task<IEnumerable<DatosCartaDTO>> BuscarTodasLasCartas();
    }
}
