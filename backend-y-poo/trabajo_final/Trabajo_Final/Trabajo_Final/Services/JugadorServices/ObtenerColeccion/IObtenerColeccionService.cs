using Trabajo_Final.DTO.ColeccionCartas;

namespace Trabajo_Final.Services.JugadorServices.ObtenerColeccion
{
    public interface IObtenerColeccionService
    {
        public Task<CartaColeccionadaDTO[]> ObtenerColeccion(int id_jugador);
    }
}
