using Trabajo_Final.DTO.ColeccionCartas;
using Trabajo_Final.DTO.Response.ResponseColeccionar;

namespace Trabajo_Final.Services.JugadorServices.ColeccionarCartas
{
    public interface IColeccionarCartasService
    {
        public Task<ResponseColeccionarDTO> Coleccionar(int id_usuario, int[] id_cartas);
    }
}
