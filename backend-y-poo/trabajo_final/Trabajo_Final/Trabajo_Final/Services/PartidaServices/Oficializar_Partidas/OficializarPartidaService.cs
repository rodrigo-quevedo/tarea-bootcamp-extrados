using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.Entidades.Custom;
using DAO.Entidades.Custom.Partida_CantidadRondas;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.Partidas;
using Trabajo_Final.Services.PartidaServices.ArmarPartidasService;

namespace Trabajo_Final.Services.PartidaServices.Oficializar_Partidas
{
    public class OficializarPartidaService : IOficializarPartidaService
    {
        IPartidaDAO partidaDAO;
        ITorneoDAO torneoDAO;

        IArmarPartidasService armarPartidasService;
        public OficializarPartidaService(IPartidaDAO partidaDao, ITorneoDAO torneoDao, IArmarPartidasService armarPartidas)
        {
            partidaDAO = partidaDao;
            torneoDAO = torneoDao;

            armarPartidasService = armarPartidas;
        }

        public async Task<bool> OficializarPartida(
            int id_juez, 
            int id_partida, 
            int id_ganador, 
            int? id_descalificado,
            string motivo_descalificacion)
        {
            //buscar partida y cantidad de rondas del torneo
            //(necesito la ronda de la partida para ver si es la ultima partida de esa ronda)
            Partida_CantidadRondasDTO datosPartida = 
                await partidaDAO.BuscarDatosParaOficializar(new Partida() { 
                    Id = id_partida, 
                    Id_juez = id_juez });

            if (datosPartida == null) throw new InvalidInputException($"No se pudo oficializar la partida por alguna de estas razones: 1. El juez [{id_juez}] no está asignado a la partida con id [{id_partida}]. 2. La partida no existe. 3. El torneo de la partida fue cancelado.");


            //verificar que no este oficializada aún
            if (datosPartida.Id_ganador != null) throw new InvalidInputException($"La partida [{id_partida}] ya ha sido oficializada.");


            //verificar fechaHora de oficializacion
            DateTime tiempoOficializacionUTC = DateTime.UtcNow;
            if (datosPartida.Fecha_hora_inicio >= tiempoOficializacionUTC)
                throw new InvalidInputException("La partida aún no ha comenzado, no se puede oficializar.");
            

            //(?) se puede verificar ganador/descalificado, pero ya está en el CHECK de la tabla

           
            //verificar si es ultima partida de ronda
            bool ultimaPartidaDeRonda = await partidaDAO.VerificarUltimaPartidaDeRonda(
                id_partida,
                datosPartida.Id_torneo,
                datosPartida.Ronda);

            //--->No es ultima partida de la ronda:
            if (!ultimaPartidaDeRonda) //DAO: UPDATE partida
                return await partidaDAO.OficializarResultado(id_partida, id_ganador, id_descalificado, motivo_descalificacion);


            //--->Es ultima partida de ronda:
            //Verificar FINAL     
            bool esFinal = datosPartida.Ronda == datosPartida.Cantidad_rondas;
            
            //---> es final
            if (esFinal) //DAO: UPDATE partida +  UPDATE torneo (fase = 'finalizado')
                return await partidaDAO.OficializarFinal(
                    id_partida, 
                    id_ganador, 
                    id_descalificado, motivo_descalificacion,
                    datosPartida.Id_torneo, 
                    FasesTorneo.FINALIZADO);



            //--->NO es final: crear partidas de la siguiente ronda

            //buscar torneo (horario_diario_inicio y horario_diario_fin)
            Torneo torneo = await torneoDAO.BuscarTorneoActivo(new Torneo() { Id = datosPartida.Id_torneo });
            if (torneo == null) throw new Exception($"No se pudo obtener los datos del torneo [{datosPartida.Id_torneo}]");

            //armar fechahoras
            int proxima_ronda = datosPartida.Ronda + 1;

            int cantidad_partidas_proxima_ronda = 
                (int) Math.Pow(2, torneo.Cantidad_rondas - proxima_ronda);

            IEnumerable<FechaHoraPartida> fechaHoras = 
                armarPartidasService.ArmarFechaHoraPartidas(
                    datosPartida.Fecha_hora_fin,//empieza a armar desde que termina esta ultima partida
                    torneo.Horario_diario_inicio,
                    torneo.Horario_diario_fin,
                    cantidad_partidas_proxima_ronda);

            //buscar jueces
            IEnumerable<Torneo> busqueda = Enumerable.Empty<Torneo>().Append(torneo);

            IEnumerable<Juez_Torneo> jueces = await torneoDAO.BuscarJuecesDeTorneos(busqueda);
            if (!jueces.Any()) throw new Exception($"No se pudo obtener la información de los participantes del torneo [{torneo.Id}]");


            //buscar jugadores ganadores
            // cantidad_ganadores = cantidad_partidas de ronda - 1 (por la partida actual)
            int cantidad_partidas_ronda = 
                (int) Math.Pow(2, datosPartida.Cantidad_rondas - datosPartida.Ronda);

            int cantidad_ganadores = cantidad_partidas_ronda - 1;

            //ganadores de la db:
            IEnumerable<Partida> ganadores =
                await partidaDAO.BuscarJugadoresGanadores(
                    datosPartida.Id_torneo,
                    datosPartida.Ronda,
                    cantidad_ganadores);

            //agregar ganador de esta partida:
            ganadores = ganadores.Append(new Partida()
            {
                Id = datosPartida.Id,
                Id_ganador = id_ganador
            });


            //armar partidas
            IEnumerable <InsertPartidaDTO> partidas = 
                armarPartidasService.ArmarPartidas_JugadoresEnOrdenCronologico(
                    torneo.Id,
                    fechaHoras.ToList(),
                    ganadores.ToList(),
                    jueces.ToList(),
                    proxima_ronda);

            //DAO: UPDATE partida + INSERT partidas
            return await partidaDAO.OficializarUltimaPartidaDeRonda(
                id_partida,
                id_ganador,
                id_descalificado, motivo_descalificacion,
                partidas);

        }
    }
}
