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

            //DemoLeerDateTime(torneo);

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
                DateTime inicio_partida = momento_pointer;
                DateTime fin_partida = momento_pointer.AddMinutes(30);

                //Averiguar hora de cierre del dia actual
                DateTime momento_cierre = 
                    CrearTorneoService.ParseHorario(
                        torneo.Horario_diario_fin, 
                        momento_pointer);

                //Averiguar si la partida excede el horario de cierre
                if (momento_pointer.AddMinutes(30) > momento_cierre)
                {   //Si lo excede:

                    momento_pointer.AddDays(1); //voy al dia siguiente

                    momento_pointer = //voy al inicio de ese dia
                        CrearTorneoService.ParseHorario(
                            torneo.Horario_diario_inicio,
                            momento_pointer);

                    inicio_partida = momento_pointer;//seteo el inicio de partida

                    fin_partida = momento_pointer.AddMinutes(30);//seteo el final de partida
                
                }

                //Mover el pointer del inicio de partida al final:
                momento_pointer = momento_pointer.AddMinutes(30);
                

                fechaHora_partidas.Add(new FechaHoraPartida()
                {
                    fecha_hora_inicio = inicio_partida,
                    fecha_hora_fin = fin_partida
                });

                Console.WriteLine($"{inicio_partida} -> {fin_partida}");
                partida_pointer++;
            }

            return fechaHora_partidas;
        }


        //private IEnumerable<Partida> ArmarPartidas(
        //    IList<FechaHoraPartida> fechaHoraPartidas, IList<Jugador_Inscripto> jugadoresPartida)
        //{

        //}



        private void DemoLeerDateTime(Torneo torneo)
        {
            Console.WriteLine($"inicio: {torneo.Fecha_hora_inicio} | {torneo.Fecha_hora_inicio.ToString("o")}");
            Console.WriteLine($"inicio en timezone local: {torneo.Fecha_hora_inicio.ToLocalTime()}" +
                $" | {torneo.Fecha_hora_inicio.ToLocalTime().ToString("o")}");
            Console.WriteLine($"inicio .Date: {torneo.Fecha_hora_inicio.Date}");
            Console.WriteLine($"inicio Kind: {torneo.Fecha_hora_inicio.Kind}");
            Console.WriteLine($"fin: {torneo.Fecha_hora_fin} | {torneo.Fecha_hora_fin.ToString("o")}");
            Console.WriteLine($"fin en timezone local: {torneo.Fecha_hora_fin.ToLocalTime()} " +
                $"| {torneo.Fecha_hora_fin.ToLocalTime().ToString("o")}");
            Console.WriteLine($"fin Kind: {torneo.Fecha_hora_fin.Kind}");
            Console.WriteLine($"substract (ambos tienen Kind=unspecified): {torneo.Fecha_hora_fin.Subtract(torneo.Fecha_hora_inicio)}");
            DateTime fin_utc = torneo.Fecha_hora_fin;
            fin_utc = DateTime.SpecifyKind(fin_utc, DateTimeKind.Utc);

            //fin_utc = fin_utc.AddMinutes(3);
            //Console.WriteLine($"fin {torneo.Fecha_hora_fin} | fin_utc {fin_utc}");
            Console.WriteLine($"fin_utc Kind: {fin_utc.Kind}");
            Console.WriteLine($"substract (fin_utc tiene Kind=utc, inicio Kind=unspecified): {fin_utc.Subtract(torneo.Fecha_hora_inicio)}");

            DateTime inicio_local = torneo.Fecha_hora_inicio;
            inicio_local = DateTime.SpecifyKind(inicio_local, DateTimeKind.Local);
            Console.WriteLine($"inicio_local Kind: {inicio_local.Kind}");
            Console.WriteLine($"substract (fin3_utc tiene Kind=utc, inicio Kind=local): {fin_utc.Subtract(inicio_local)}");


            throw new Exception("fin programa");
        }

    }
}
