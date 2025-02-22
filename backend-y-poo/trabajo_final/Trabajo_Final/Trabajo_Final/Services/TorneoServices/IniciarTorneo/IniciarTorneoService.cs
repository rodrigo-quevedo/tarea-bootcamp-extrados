using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Torneos;
using DAO.Entidades.Custom;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using System.Text.Json;
using Trabajo_Final.DTO.Partidas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.IniciarTorneo
{
    public class IniciarTorneoService : IIniciarTorneoService
    {
        ITorneoDAO torneoDAO;
        public IniciarTorneoService(ITorneoDAO torneoDao) 
        {
            torneoDAO = torneoDao;
        }


        public async Task<bool> IniciarTorneo(int id_torneo, int id_organizador)
        {
            //buscar torneo
            Torneo torneo = await torneoDAO.BuscarTorneo( new Torneo() { Id = id_torneo });

            if (torneo == null) throw new InvalidInputException($"El torneo {id_torneo} no existe.");
            if (torneo.Fase != FasesTorneo.REGISTRO) throw new InvalidInputException($"El torneo {id_torneo} ya ha iniciado. Fase actual del torneo: {torneo.Fase}");
            if (torneo.Id_organizador != id_organizador) throw new InvalidInputException($"El torneo {id_torneo} no pertenece al organizador.");

            //buscar jueces
            IEnumerable<Torneo> busqueda = Enumerable.Empty<Torneo>();
            busqueda = busqueda.Append(new Torneo() { Id = id_torneo });
            IEnumerable<Juez_Torneo> jueces = await torneoDAO.BuscarJuecesDeTorneos(busqueda);
       

            if (torneo == null || jueces.Count() == 0) throw new Exception($"No se pudo obtener el torneo [{id_torneo}]");

            
            //armar fechaHora de inicio y fin de partidas
            int cantidad_partidas_primera_ronda =
                (int) Math.Pow(2, torneo.Cantidad_rondas - 1);

            IEnumerable<FechaHoraPartida> fechaHoras = 
                ArmarFechaHoraPartidas_primeraRonda(torneo, cantidad_partidas_primera_ronda);


            //DemoLeerDateTime(torneo);

            //buscar jugadores
            int cantidad_jugadores = (int) Math.Pow(2, torneo.Cantidad_rondas);
            
            IEnumerable<Jugador_Inscripto> jugadores_aceptables =
                await torneoDAO.BuscarJugadoresInscriptos(id_torneo, cantidad_jugadores);

            
            //armar partidas
            IEnumerable<DatosPartidaDTO> partidas = 
                ArmarPartidas(
                    torneo, 
                    fechaHoras.ToList(), 
                    jugadores_aceptables.ToList(),
                    jueces.ToList());


            //transaction: UPDATE torneo, UPDATE jugadores_inscriptos, INSERT partidas
            bool exito = await torneoDAO.IniciarTorneo(
                FasesTorneo.TORNEO,
                id_torneo,
                jugadores_aceptables.Select(j => j.Id_jugador).ToList(),
                partidas.ToList());


            return exito;
        }


        //private int CalcularCantidadPartidas(int cantidad_rondas)
        //{
        //    int partidas = 1;

        //    for (int ronda = 1; ronda <= cantidad_rondas; ronda++)
        //    {
        //        if (ronda == 1) continue;

        //        partidas = partidas + (int) Math.Pow(2, ronda - 1);
        //        Console.WriteLine($"partidas: {partidas} | ronda: {ronda}");
        //    }

        //    return partidas;
        //}

        private IEnumerable<FechaHoraPartida> ArmarFechaHoraPartidas_primeraRonda(
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


        private IEnumerable<DatosPartidaDTO> ArmarPartidas(
            Torneo torneo,
            IList<FechaHoraPartida> fechaHoraPartidas, 
            IList<Jugador_Inscripto> jugadoresPartida,
            IList<Juez_Torneo> jueces)
        {
            IList<DatosPartidaDTO> partidas = new List<DatosPartidaDTO>();

            Random rnd = new Random();

            foreach (FechaHoraPartida fechahora in fechaHoraPartidas) 
            {
                //sortear jugadores
                int index_jugador_1 = rnd.Next(jugadoresPartida.Count);
                Jugador_Inscripto j1 = jugadoresPartida[index_jugador_1];
                jugadoresPartida.RemoveAt(index_jugador_1);


                int index_jugador_2 = rnd.Next(jugadoresPartida.Count);
                Jugador_Inscripto j2 = jugadoresPartida[index_jugador_2];
                jugadoresPartida.RemoveAt(index_jugador_2);


                //sortear juez
                int index_juez = rnd.Next(jueces.Count);
                Juez_Torneo juez = jueces[index_juez];

                //armar partida
                partidas.Add(new DatosPartidaDTO()
                {
                    Ronda = 1,
                    Id_torneo = torneo.Id,
                    Id_jugador_1 = j1.Id_jugador,
                    Id_jugador_2 = j2.Id_jugador,
                    Fecha_hora_inicio = fechahora.fecha_hora_inicio,
                    Fecha_hora_fin = fechahora.fecha_hora_fin,
                    Id_juez = juez.Id_juez,
                }); 
            }

            return partidas;
        }



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
