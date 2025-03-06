using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Torneos;
using DAO.DTOs_en_DAOs.InsertPartidas;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.ProcesamientoDatos.FechaHoraPartidas;
using Trabajo_Final.Services.PartidaServices.ArmarPartidasService;

namespace Trabajo_Final.Services.TorneoServices.IniciarTorneo
{
    public class IniciarTorneoService : IIniciarTorneoService
    {
        ITorneoDAO torneoDAO;
        IArmarPartidasService armarPartidasService;
        public IniciarTorneoService(ITorneoDAO torneoDao, IArmarPartidasService armarPartidas) 
        {
            torneoDAO = torneoDao;
            armarPartidasService = armarPartidas;
        }


        public async Task<bool> IniciarTorneo(int id_torneo, int id_organizador)
        {
            //buscar torneo y validar
            Torneo torneo = await torneoDAO.BuscarTorneoActivo( new Torneo() { Id = id_torneo });

            if (torneo == null) throw new InvalidInputException($"No se pudo iniciar el torneo por alguna de estas razones: 1. El torneo [{id_torneo}] no existe. 2. El torneo está cancelado.");
            if (torneo.Fase != FasesTorneo.REGISTRO) throw new InvalidInputException($"El torneo {id_torneo} ya ha iniciado. Fase actual del torneo: {torneo.Fase}");
            if (torneo.Id_organizador != id_organizador) throw new InvalidInputException($"El torneo {id_torneo} no pertenece al organizador.");

            //buscar jueces
            IEnumerable<Torneo> busqueda = Enumerable.Empty<Torneo>();
            busqueda = busqueda.Append(new Torneo() { Id = id_torneo });
            IEnumerable<Juez_Torneo> jueces = await torneoDAO.BuscarJuecesDeTorneos(busqueda);
       
            if (jueces.Count() == 0) throw new Exception($"No se pudo obtener los jueces del torneo [{id_torneo}]");


            
            //armar fechaHora de inicio y fin de partidas
            int cantidad_partidas_primera_ronda =
                (int) Math.Pow(2, torneo.Cantidad_rondas - 1);

            IEnumerable<FechaHoraPartida> fechaHoras = 
                armarPartidasService.ArmarFechaHoraPartidas(
                    torneo.Fecha_hora_inicio,
                    torneo.Horario_diario_inicio,
                    torneo.Horario_diario_fin,
                    cantidad_partidas_primera_ronda);


            //DemoLeerDateTime(torneo);

            //buscar jugadores
            int cantidad_jugadores = (int) Math.Pow(2, torneo.Cantidad_rondas);
            
            IEnumerable<Jugador_Inscripto> jugadores_aceptables =
                await torneoDAO.BuscarJugadoresInscriptos(id_torneo, cantidad_jugadores);


            //armar partidas
            int ronda = 1;
            IEnumerable<InsertPartidaDTO> partidas = 
                armarPartidasService.ArmarPartidas_JugadoresAleatorios(
                    torneo.Id, 
                    fechaHoras.ToList(), 
                    jugadores_aceptables.ToList(),
                    jueces.ToList(),
                    ronda);


            //transaction: UPDATE torneo, UPDATE jugadores_inscriptos, INSERT partidas
            bool exito = await torneoDAO.IniciarTorneo(
                FasesTorneo.TORNEO,
                id_torneo,
                jugadores_aceptables.Select(j => j.Id_jugador).ToList(),
                partidas.ToList());


            return exito;
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
