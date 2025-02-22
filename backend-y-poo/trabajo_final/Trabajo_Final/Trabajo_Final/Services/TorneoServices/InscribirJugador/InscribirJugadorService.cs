using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.Entidades.Cartas;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.InscribirJugador
{
    public class InscribirJugadorService : IInscribirJugadorService
    {
        ITorneoDAO torneoDAO;
        ICartaDAO cartaDAO;
        public InscribirJugadorService(ITorneoDAO torneoDao, ICartaDAO cartaDao)
        {
            torneoDAO = torneoDao;
            cartaDAO = cartaDao;
        }

        public async Task<bool> Inscribir(int id_jugador, int id_torneo, int[] id_cartas_mazo)
        {

            //Se verifica series habilitadas y existencia de cartas

            IEnumerable<Serie_De_Carta> series_de_cartas = 
                await cartaDAO.BuscarSeriesDeCartas(id_cartas_mazo);

            //foreach (Serie_De_Carta serie in series_de_cartas) Console.WriteLine($"{serie.Id_carta} -> {serie.Nombre_serie}");

            IList<Torneo> busqueda = new List<Torneo>();
            busqueda.Add(new Torneo() { Id =id_torneo });

            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(busqueda);


            //foreach (Serie_Habilitada serie in series_habilitadas) Console.WriteLine($"torneo {serie.Id_torneo} -> {serie.Nombre_serie}");

            foreach(int id_carta in id_cartas_mazo)
            {
                bool habilitada = false;

                IList<string> series_carta =
                    series_de_cartas
                        .Where(serie => serie.Id_carta == id_carta)
                        .Select(serie => serie.Nombre_serie)
                        .ToList();

                foreach (string serie in series_carta) {
                    if (series_habilitadas.Select(s => s.Nombre_serie).Contains(serie))
                        habilitada = true;
                }

                if (!habilitada) throw new InvalidInputException($"La carta {id_carta} no pertenece a ninguna serie habilitada del torneo [{id_torneo}].");
            }


            //Se verifica jugador, torneo, y carta en coleccion
            await torneoDAO.InscribirJugador(
                id_jugador, Roles.JUGADOR,
                id_torneo, FasesTorneo.REGISTRO,
                id_cartas_mazo);

            return true;
        }


        public void VerificarRepeticionesMazo(int[] id_cartas_mazo)
        {
            int id_repetida = 0;

            try
            {
                id_repetida = id_cartas_mazo
                    .GroupBy(id => id)
                    .First(id => id.Count() > 1)
                    .Key;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no matching element"))
                    Console.WriteLine("No hay cartas repetidas en el mazo");

                else throw ex;
            }

            if (id_repetida != 0) throw new InvalidInputException($"La carta id [{id_repetida}] esta repetida.");
        }
    }
}
