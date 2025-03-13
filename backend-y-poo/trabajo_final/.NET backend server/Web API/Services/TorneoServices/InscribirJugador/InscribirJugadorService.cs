using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.Entidades.Cartas;
using DAO.Entidades.TorneoEntidades;

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

            IList<Torneo> busqueda = new List<Torneo>();
            busqueda.Add(new Torneo() { Id =id_torneo });

            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(busqueda);


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


            //Incribir jugador al torneo:
            await torneoDAO.InscribirJugador(
                id_jugador, Roles.JUGADOR,
                id_torneo, FasesTorneo.REGISTRO,
                id_cartas_mazo);


            //Una vez el jugador se inscribe, determinar los jugadores aceptados:

            Torneo torneo = await torneoDAO.BuscarTorneoActivo(new Torneo() { Id = id_torneo });

            IEnumerable<Jugador_Inscripto> jugadores_inscriptos = 
                await torneoDAO.BuscarJugadoresInscriptos(id_torneo);

            //Si se llega a una cantidad potencia de 2, se deben aceptar todos
            //los jugadores anteriores y al jugador que se inscribió.

            //Y si no se llega a esa cantidad, igualmente hay que asegurarse de que 
            //todos los jugadores anteriores a [mayor potencia de 2 posible según
            //la cantidad de usuarios inscriptos] sean aceptados.
            //Es decir, cada vez que un jugador se inscriba, se va a realizar una 
            //aceptacion automática de jugadores, según su orden.
            
            //Esto le sirve a los jugadores para saber el estado de su inscripcion cuanto antes
            //(ya no es necesario que el organizador inicie el torneo para que los
            //jugadores sepan si han sido aceptados).

            int cantidad_inscriptos = jugadores_inscriptos.Count();
            int cantidad_aceptable = 0;
            int rondas = 1;
            while ((int)Math.Pow(2, rondas) <= cantidad_inscriptos)
            {
                cantidad_aceptable = (int)Math.Pow(2, rondas++);
            }

            if (cantidad_aceptable < 2) return true;//No hay jugadores para aceptar


            jugadores_inscriptos = jugadores_inscriptos.OrderBy(j => j.Orden);
            
            Jugador_Inscripto ultimo_jugador_aceptable =
                jugadores_inscriptos.ToArray()[cantidad_aceptable - 1];

            IEnumerable<Jugador_Inscripto> jugadores_para_aceptar =
                jugadores_inscriptos
                .Where(j =>
                    j.Orden <= ultimo_jugador_aceptable.Orden
                    &&
                    j.Aceptado == false
                );

            return await torneoDAO.ActualizarJugadoresYCantidadRondas(
                id_torneo,
                jugadores_para_aceptar.Select(j=>j.Id_jugador), 
                rondas - 1 //le resto 1 porque cuando corta el while se está excediendo en 1
            );

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
