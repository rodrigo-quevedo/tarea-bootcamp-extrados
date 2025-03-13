using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;
using DAO.Entidades.ColeccionCartas;
using Trabajo_Final.DTO.Response.Cartas;

namespace Trabajo_Final.Services.JugadorServices.ObtenerColeccion
{
    public class ObtenerColeccionService : IObtenerColeccionService
    {
        private ICartaDAO cartaDAO;
        public ObtenerColeccionService(ICartaDAO dao)
        {
            cartaDAO = dao;
        }

        public async Task<int[]> ObtenerColeccion(int id_jugador)
        {
            
            IEnumerable<Carta> cartas_coleccionadas = 
                await cartaDAO.BuscarCartasColeccionadas(id_jugador);

            return cartas_coleccionadas.Select(c=>c.Id).ToArray();
        }
    }
}
