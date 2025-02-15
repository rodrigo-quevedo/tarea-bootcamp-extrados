
using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;
using Trabajo_Final.DTO.ColeccionCartas.ResponseColeccionar;

namespace Trabajo_Final.Services.JugadorServices.ColeccionarCartas
{
    public class ColeccionarCartasService : IColeccionarCartasService
    {
        ICartaDAO cartaDAO;
        public ColeccionarCartasService(ICartaDAO dao)
        {
            cartaDAO = dao;
        }

        public async Task<ResponseColeccionarDTO> Coleccionar(int id_usuario, int[] id_cartas)
        {
            //Obtener cartas coleccionadas
            IEnumerable<Carta> cartasColeccionadas = await cartaDAO.BuscarCartasColeccionadas(id_usuario);
            //Sacar las IDs
            IEnumerable<int> id_cartasColeccionadas = cartasColeccionadas.Select(c => c.Id);

            //Elimino las repetidas
            IList<int> coleccionadasRepetidas = new List<int>();
            IList<int> coleccionadasSinRepetir = new List<int>();

            id_cartas.ToList().ForEach(id =>
            {
                if (id_cartasColeccionadas.Contains(id)) coleccionadasRepetidas.Add(id);
                else coleccionadasSinRepetir.Add(id);
            });

            
            bool exito = await cartaDAO.ColeccionarCartas(id_usuario, coleccionadasSinRepetir.ToArray());
            if (!exito) throw new Exception("No se pudo agregar las cartas a la colección.");
            
            return new ResponseColeccionarDTO
            {
                id_cartas_repetidas = coleccionadasRepetidas.ToArray(),
                id_cartas_agregadas = coleccionadasSinRepetir.ToArray(),
                message = "Se agregaron las cartas a la colección."
            };
        }
    }
}
