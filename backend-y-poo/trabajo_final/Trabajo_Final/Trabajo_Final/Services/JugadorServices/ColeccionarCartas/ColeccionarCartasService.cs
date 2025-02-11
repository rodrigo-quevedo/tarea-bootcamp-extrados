
using DAO.DAOs.Cartas;

namespace Trabajo_Final.Services.JugadorServices.ColeccionarCartas
{
    public class ColeccionarCartasService : IColeccionarCartasService
    {
        ICartaDAO cartaDAO;
        public ColeccionarCartasService(ICartaDAO dao)
        {
            cartaDAO = dao;
        }

        public async Task<bool> Coleccionar(int id_usuario, int[] id_cartas)
        {
            bool exito = await cartaDAO.ColeccionarCartas(id_usuario, id_cartas);
            if (!exito) throw new Exception("No se pudo agregar las cartas a la colección.");
            
            return exito;
        }
    }
}
