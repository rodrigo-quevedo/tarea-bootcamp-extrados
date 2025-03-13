using Trabajo_Final.DTO.Response.Cartas;

namespace Trabajo_Final.Services.JugadorServices.ObtenerColeccion
{
    public interface IObtenerColeccionService
    {
        public Task<int[]> ObtenerColeccion(int id_jugador);
    }
}
