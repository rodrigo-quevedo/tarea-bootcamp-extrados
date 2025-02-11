using Trabajo_Final.DTO.ColeccionCartas;

namespace Trabajo_Final.Services.JugadorServices.ColeccionarCartas
{
    public interface IColeccionarCartasService
    {
        public Task<bool> Coleccionar(int id_usuario, int[] id_cartas);
    }
}
