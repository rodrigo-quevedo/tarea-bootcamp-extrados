
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;

namespace Trabajo_Final.Services.JugadorServices.QuitarCartas
{
    public class QuitarCartasService : IQuitarCartasService
    {
        private ICartaDAO cartaDAO;
        public QuitarCartasService(ICartaDAO dao)
        {
            cartaDAO = dao;
        }
        

        public async Task<bool> QuitarCartas(int id_jugador, int[] id_cartas)
        {
            bool exito = await cartaDAO.QuitarCartasColeccionadas(id_jugador, id_cartas);
            if (!exito) throw new DefaultException("No se pudo quitar las cartas de la colección.");

            return true;
        }
    }
}
