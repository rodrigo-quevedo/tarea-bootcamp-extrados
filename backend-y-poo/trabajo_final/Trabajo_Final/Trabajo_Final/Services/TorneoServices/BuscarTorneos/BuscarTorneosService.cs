using DAO.DAOs.Cartas;
using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.Entidades.Custom.Ganador_Torneo;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;
using Trabajo_Final.DTO.ListaTorneos.ResponseDTO;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.BuscarTorneos
{
    public class BuscarTorneosService : IBuscarTorneosService
    {
        IPartidaDAO partidaDAO;
        ITorneoDAO torneoDAO;
        ICartaDAO cartaDAO;
        public BuscarTorneosService(IPartidaDAO partidaDao, ITorneoDAO torneoDao, ICartaDAO cartaDao)
        {
            partidaDAO = partidaDao;
            torneoDAO = torneoDao;
            cartaDAO = cartaDao;
        }

        public async Task<IList<TorneoVistaAdminDTO>> BuscarTorneos(string[] fases)
        {
            //torneos
            IEnumerable<Torneo> torneos =
                await torneoDAO.BuscarTorneos(fases);

            //series
            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            //jueces
            IEnumerable<Juez_Torneo> jueces_torneos =
                await torneoDAO.BuscarJuecesDeTorneos(torneos);

            //jugadores que se inscribieron
            IEnumerable<Jugador_Inscripto> jugadores_registro =
                await torneoDAO.BuscarJugadoresInscriptos(torneos);

            //jugadores inscriptos (aceptados)
            IEnumerable<Jugador_Inscripto> jugadores =
                await torneoDAO.BuscarJugadoresAceptados(torneos);

            //ganadores de torneos
            IEnumerable<GanadorTorneo> ganadores =
                await torneoDAO.BuscarGanadoresTorneos(torneos);

            //partidas del torneo
            IEnumerable<Partida> partidas =
                await partidaDAO.BuscarPartidasDelTorneo(torneos);


            //armar DTO
            IList<TorneoVistaAdminDTO> result = new List<TorneoVistaAdminDTO>();

            foreach (Torneo torneo in torneos)
            {
                string[] series =
                    series_habilitadas
                    .Where(serie => serie.Id_torneo == torneo.Id)
                    .Select(serie => serie.Nombre_serie)
                    .ToArray();

                int[] id_jueces =
                    jueces_torneos
                    .Where(juez => juez.Id_torneo == torneo.Id)
                    .Select(juez => juez.Id_juez)
                    .ToArray();

                int[] id_jugadores_inscriptos = 
                    jugadores_registro
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador)
                    .ToArray();

                int[]? id_jugadores_aceptados;
                if (torneo.Fase == FasesTorneo.REGISTRO) id_jugadores_aceptados = null;
                else id_jugadores_aceptados =
                    jugadores
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador)
                    .ToArray();

                int? id_ganador;
                if (torneo.Fase != FasesTorneo.FINALIZADO) id_ganador = null;
                else id_ganador =
                        ganadores
                        .First(ganador => ganador.Id_torneo == torneo.Id)
                        .Id_ganador;

                int[]? id_partidas;
                if (torneo.Fase == FasesTorneo.REGISTRO) id_partidas = null;
                else id_partidas =
                    partidas
                    .Where(partida => partida.Id_torneo == torneo.Id)
                    .Select(partida => partida.Id)
                    .ToArray();



                result.Add(new TorneoVistaAdminDTO()
                {
                    Id = torneo.Id,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Fase = torneo.Fase,
                    Id_organizador = torneo.Id_organizador,
                    Series_habilitadas = series,
                    Id_jueces = id_jueces,
                    Id_jugadores_inscriptos = id_jugadores_inscriptos,
                    Id_jugadores_aceptados = id_jugadores_aceptados,
                    Id_ganador = id_ganador,
                    Id_partidas = id_partidas
                });

            }


            return result;
        }

        public async Task<IList<TorneoOrganizadoDTO>> BuscarTorneosOrganizados(string[] fases, int id_organizador)
        {
            //torneos
            IEnumerable<Torneo> torneos =
                await torneoDAO.BuscarTorneosOrganizados(fases, id_organizador);

            //series
            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            //jueces
            IEnumerable<Juez_Torneo> jueces_torneos =
                await torneoDAO.BuscarJuecesDeTorneos(torneos);

            //jugadores inscriptos (aceptados)
            IEnumerable<Jugador_Inscripto> jugadores =
                await torneoDAO.BuscarJugadoresAceptados(torneos);

            //ganadores de torneos
            IEnumerable<GanadorTorneo> ganadores =
                await torneoDAO.BuscarGanadoresTorneos(torneos);

            //partidas del torneo
            IEnumerable<Partida> partidas =
                await partidaDAO.BuscarPartidasDelTorneo(torneos);


            //armar DTO
            IList<TorneoOrganizadoDTO> result = new List<TorneoOrganizadoDTO>();

            foreach (Torneo torneo in torneos)
            {
                string[] series =
                    series_habilitadas
                    .Where(serie => serie.Id_torneo == torneo.Id)
                    .Select(serie => serie.Nombre_serie)
                    .ToArray ();

                int[] id_jueces =
                    jueces_torneos
                    .Where(juez => juez.Id_torneo == torneo.Id)
                    .Select(juez => juez.Id_juez)
                    .ToArray();

                int[]? id_jugadores_aceptados;
                if (torneo.Fase == FasesTorneo.REGISTRO) id_jugadores_aceptados = null;
                else id_jugadores_aceptados =
                    jugadores
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador)
                    .ToArray();

                int? id_ganador;
                if (torneo.Fase != FasesTorneo.FINALIZADO) id_ganador = null;
                else id_ganador = 
                        ganadores
                        .First(ganador => ganador.Id_torneo == torneo.Id)
                        .Id_ganador;

                int[]? id_partidas;
                if (torneo.Fase == FasesTorneo.REGISTRO) id_partidas = null;
                else id_partidas =
                    partidas
                    .Where(partida => partida.Id_torneo == torneo.Id)
                    .Select(partida => partida.Id)
                    .ToArray();



                result.Add(new TorneoOrganizadoDTO()
                {
                    Id = torneo.Id,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Fase = torneo.Fase,
                    Id_organizador = torneo.Id_organizador,
                    Series_habilitadas = series,
                    Id_jueces = id_jueces,
                    Id_jugadores = id_jugadores_aceptados,
                    Id_ganador = id_ganador,
                    Id_partidas = id_partidas
                });

            }


            return result;
        }


        public async Task<IList<TorneoLlenoDTO>> BuscarTorneosLlenos(int id_organizador)
        {
            //torneos
            IEnumerable<Torneo> torneos =
                await torneoDAO.BuscarTorneosLlenos(FasesTorneo.REGISTRO, id_organizador);

            if (torneos == null || torneos.Count() == 0) return null;

            //series
            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            //jueces
            IEnumerable<Juez_Torneo> jueces_torneos =
                await torneoDAO.BuscarJuecesDeTorneos(torneos);

            //jugadores
            IEnumerable<Jugador_Inscripto> jugadores_inscriptos =
                await torneoDAO.BuscarJugadoresInscriptos(torneos);

            //armar DTO
            IList<TorneoLlenoDTO> result = new List<TorneoLlenoDTO>();

            foreach (Torneo torneo in torneos)
            {
                IList<string> series =
                    series_habilitadas
                    .Where(serie => serie.Id_torneo == torneo.Id)
                    .Select(serie => serie.Nombre_serie)
                    .ToList();

                IList<int> id_jueces =
                    jueces_torneos
                    .Where(juez => juez.Id_torneo == torneo.Id)
                    .Select(juez => juez.Id_juez)
                    .ToList();

                IList<int> id_jugadores =
                    jugadores_inscriptos
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador)
                    .ToList();


                result.Add(new TorneoLlenoDTO()
                {
                    Id = torneo.Id,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Fase = torneo.Fase,
                    Id_organizador = torneo.Id_organizador,
                    Series_habilitadas = series.ToArray(),
                    Id_jueces = id_jueces.ToArray(),
                    Id_jugadores_inscriptos = id_jugadores.ToArray()
                });

            }

            return result;
        }

        public async Task<IList<TorneoGanadoDTO>> BuscarTorneosGanados(int id_jugador)
        {
            IEnumerable<Partida> finalesGanadas = await partidaDAO.BuscarFinalesGanadas(id_jugador);    
            
            IEnumerable<Torneo> torneos = await torneoDAO.BuscarTorneos(
                finalesGanadas
                .Select(final => final.Id_torneo)
                .ToList()
            );

            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            IEnumerable<Jugador_Inscripto> jugadores_participantes =
                await torneoDAO.BuscarJugadoresAceptados(torneos);

            IEnumerable<Carta_Del_Mazo> cartas_mazo =
                await cartaDAO.BuscarMazosInscriptos(
                    id_jugador,
                    torneos.Select(torneo => torneo.Id).ToList());


            //armar DTO
            IList<TorneoGanadoDTO> result = new List<TorneoGanadoDTO>();

            foreach (Torneo torneo in torneos)
            {
                IList<string> series =
                    series_habilitadas
                    .Where(serie => serie.Id_torneo == torneo.Id)
                    .Select(serie => serie.Nombre_serie)
                    .ToList();

                IList<int> id_jugadores =
                    jugadores_participantes
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador)
                    .ToList();

                IList<int> id_cartas_mazo =
                    cartas_mazo
                    .Where(carta => carta.Id_torneo == torneo.Id)
                    .Select(carta => carta.Id_carta)
                    .ToList();


                result.Add(new TorneoGanadoDTO()
                {
                    Id_torneo = torneo.Id,
                    Id_ganador = id_jugador,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Series_habilitadas = series.ToArray(),
                    //Id_jugadores = id_jugadores.ToArray(),
                    Id_cartas_mazo = id_cartas_mazo.ToArray()
                });

            }

            return result;

        }


        public async Task<IList<TorneoOficializadoDTO>> BuscarTorneosOficializados(int id_juez)
        {
            //filtrar torneos oficializados (finalizados)
            IEnumerable<int> id_torneos_oficializados =
                await torneoDAO.BuscarIdTorneosOficializados(id_juez, FasesTorneo.FINALIZADO);

            IEnumerable<Torneo> torneos =
                await torneoDAO.BuscarTorneos(id_torneos_oficializados.ToList());

            //datos extra del torneo:

            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            IEnumerable<Jugador_Inscripto> jugadores_aceptados =
                await torneoDAO.BuscarJugadoresAceptados(torneos);

            IEnumerable<GanadorTorneo> ganadores_torneos =
                await torneoDAO.BuscarGanadoresTorneos(torneos);

            IEnumerable<Partida> partidas =
                await partidaDAO.BuscarPartidasOficializadasDelTorneo(id_juez, torneos);


            //armar DTO:
            IList<TorneoOficializadoDTO> result = new List<TorneoOficializadoDTO>();

            foreach (Torneo torneo in torneos)
            {
                string[] series_habilitadas_torneo = 
                    series_habilitadas
                    .Where(serie => serie.Id_torneo == torneo.Id)
                    .Select(serie => serie.Nombre_serie)
                    .ToArray();

                int[] id_jugadores =
                    jugadores_aceptados
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador) 
                    .ToArray();

                int id_ganador =
                    ganadores_torneos
                    .Where(ganador => ganador.Id_torneo == torneo.Id)
                    .Select(ganador => ganador.Id_ganador)
                    .First();

                int[] id_partidas =
                    partidas
                    .Where(partida => partida.Id_torneo == torneo.Id)
                    .Select(partida => partida.Id)
                    .ToArray();


                result.Add(new TorneoOficializadoDTO()
                {
                    Id_torneo = torneo.Id,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Id_ganador = id_ganador,
                    Id_jugadores = id_jugadores,
                    Series_habilitadas = series_habilitadas_torneo,
                    Id_partidas_oficializadas = id_partidas
                });
            }


            return result;
        }

    }
}
