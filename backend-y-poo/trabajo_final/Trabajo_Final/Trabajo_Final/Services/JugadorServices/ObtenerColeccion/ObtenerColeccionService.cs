using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;
using DAO.Entidades.ColeccionCartas;
using Trabajo_Final.DTO.ColeccionCartas;

namespace Trabajo_Final.Services.JugadorServices.ObtenerColeccion
{
    public class ObtenerColeccionService : IObtenerColeccionService
    {
        private ICartaDAO cartaDAO;
        public ObtenerColeccionService(ICartaDAO dao)
        {
            cartaDAO = dao;
        }

        public async Task<CartaColeccionadaDTO[]> ObtenerColeccion(int id_jugador)
        {
            
            IEnumerable<Carta> arrCartasColeccionadas = 
                await cartaDAO.BuscarCartasColeccionadas(id_jugador);


            int[] id_cartas = new int[arrCartasColeccionadas.Count()];

            int i = 0;
            foreach (Carta carta in arrCartasColeccionadas){
                id_cartas[i++] = carta.Id;
                //Console.WriteLine($"{carta.Id} |ataque: {carta.Ataque} |defensa: {carta.Defensa} |ilustracion:{carta.Ilustracion}");
            }

            IEnumerable<Serie_De_Carta> arrSeriesDeCartas =
                await cartaDAO.BuscarSeriesDeCartas(id_cartas);
            
            //test
            //foreach (Series_De_Carta series_de_carta in arrSeriesDeCartas) {
            //    Console.WriteLine($"{series_de_carta.Id_carta} -> {series_de_carta.Nombre_serie}");
            //}

            CartaColeccionadaDTO[] coleccion = 
                new CartaColeccionadaDTO[arrCartasColeccionadas.Count()];
            i = 0;
            foreach (Carta carta in arrCartasColeccionadas)
            {
                List<string> series = new List<string>();
                foreach (Serie_De_Carta serie_de_carta in arrSeriesDeCartas) {
                    if (serie_de_carta.Id_carta == carta.Id) 
                        series.Add(serie_de_carta.Nombre_serie);
                    
                }

                coleccion[i++] = new CartaColeccionadaDTO()
                {
                    Id = carta.Id,
                    Ataque = carta.Ataque,
                    Defensa = carta.Defensa,
                    Ilustracion = carta.Ilustracion,
                    Series = series.ToArray()
                };
            }

            return coleccion;
        }
    }
}
