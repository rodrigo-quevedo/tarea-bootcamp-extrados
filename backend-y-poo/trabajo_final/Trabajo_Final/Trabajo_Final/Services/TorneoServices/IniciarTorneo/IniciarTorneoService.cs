using DAO.DAOs.Torneos;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.Partidas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.Crear;

namespace Trabajo_Final.Services.TorneoServices.IniciarTorneo
{
    public class IniciarTorneoService : IIniciarTorneoService
    {
        ITorneoDAO torneoDAO;
        public IniciarTorneoService(ITorneoDAO torneoDao) 
        {
            torneoDAO = torneoDao;
        }


        public async Task<IEnumerable<Partida>> IniciarTorneo(int id_torneo)
        {
            //buscar torneo
            Torneo torneo = await torneoDAO.BuscarTorneo(new Torneo() { Id = id_torneo });

            if (torneo == null) throw new Exception($"No se pudo obtener el torneo [{id_torneo}]");

            //armar fechaHora de inicio y fin de partidas
            int cantidad_partidas = CalcularCantidadPartidas(torneo.Cantidad_rondas);
            
            IEnumerable<FechaHoraPartida> fechaHoras = ArmarFechaHoraPartidas(torneo, cantidad_partidas);

            //armar jugadores
            int cantidad_jugadores = (int) Math.Pow(2, torneo.Cantidad_rondas);
            IEnumerable<Jugador_Inscripto> jugadores_aceptables =
                await torneoDAO.BuscarJugadoresInscriptos(id_torneo, cantidad_jugadores);



            //sortear jugadores
            //IEnumerable<Partida> partidas = ArmarPartidas(fechaHoras.ToList(), jugadores_aceptables.ToList());


            //transaction: UPDATE torneo, INSERT partidas

            

            throw new NotImplementedException();
        }


        private int CalcularCantidadPartidas(int cantidad_rondas)
        {
            int partidas = 1;

            for (int ronda = 1; ronda <= cantidad_rondas; ronda++)
            {
                if (ronda == 1) continue;

                partidas = partidas + (int) Math.Pow(2, ronda - 1);
                Console.WriteLine($"partidas: {partidas} | ronda: {ronda}");
            }

            return partidas;
        }

        private IEnumerable<FechaHoraPartida> ArmarFechaHoraPartidas(
            Torneo torneo, 
            int cantidad_partidas)
        {
            IList<FechaHoraPartida> fechaHora_partidas = new List<FechaHoraPartida>();

            int partida_pointer = 1;

            DateTime momento_pointer = torneo.Fecha_hora_inicio;
            

            while (partida_pointer <= cantidad_partidas)
            {
                DateTime inicio = momento_pointer;
                DateTime fin = momento_pointer.AddMinutes(30);

                DateTime horario_fin =
                    CrearTorneoService.ParseHorario(
                        torneo.Horario_diario_fin, 
                        momento_pointer);

                if (momento_pointer.AddMinutes(30) > horario_fin) {
                    
                    momento_pointer.AddDays(1); //voy al dia siguiente

                    momento_pointer = 
                        CrearTorneoService.ParseHorario(
                            torneo.Horario_diario_inicio, //voy al inicio de ese dia
                            momento_pointer);

                    inicio = momento_pointer;//empieza partida

                    momento_pointer = momento_pointer.AddMinutes(30);
                    fin = momento_pointer;//final partida
                }

                else {
                    momento_pointer = momento_pointer.AddMinutes(30);
                }

                fechaHora_partidas.Add(new FechaHoraPartida()
                {
                    fecha_hora_inicio = inicio,
                    fecha_hora_fin = fin
                });

                Console.WriteLine($"{inicio} -> {fin}");
                partida_pointer++;
            }

            return fechaHora_partidas;
        }
    
        
        //private IEnumerable<Partida> ArmarPartidas(
        //    IList<FechaHoraPartida> fechaHoraPartidas, IList<Jugador_Inscripto> jugadoresPartida)
        //{

        //}
    
    
    }
}
