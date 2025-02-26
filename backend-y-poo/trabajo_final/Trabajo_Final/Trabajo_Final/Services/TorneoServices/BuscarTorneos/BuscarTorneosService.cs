using DAO.DAOs.Cartas;
using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.Custom.TorneoGanado;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using System.Text.Json;
using Trabajo_Final.DTO.ListaTorneos;
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


        public async Task<IList<TorneoVistaCompletaDTO>> BuscarTorneos(string[] fases, int id_organizador)
        {
            //torneos
            IEnumerable<Torneo> torneos =
                await torneoDAO.BuscarTorneos(fases, id_organizador);

            //series
            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            //jueces
            IEnumerable<Juez_Torneo> jueces_torneos =
                await torneoDAO.BuscarJuecesDeTorneos(torneos);

            //jugadores inscriptos (aceptados)
            IEnumerable<Jugador_Inscripto> jugadores =
                await torneoDAO.BuscarJugadoresAceptados(torneos);


            //armar DTO
            IList<TorneoVistaCompletaDTO> result = new List<TorneoVistaCompletaDTO>();

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

                IList<int> id_jugadores_aceptados = 
                    jugadores
                    .Where(jugador => jugador.Id_torneo == torneo.Id)
                    .Select(jugador => jugador.Id_jugador)
                    .ToList();


                result.Add(new TorneoVistaCompletaDTO()
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
                    series_habilitadas = series.ToArray(),
                    id_jueces_torneo = id_jueces.ToArray(),
                    id_jugadores_aceptados = id_jugadores_aceptados.ToArray()
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
                    series_habilitadas = series.ToArray(),
                    id_jueces_torneo = id_jueces.ToArray(),
                    id_jugadores_inscriptos = id_jugadores.ToArray()
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
                    Id = torneo.Id,
                    Id_ganador = id_jugador,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Series_habilitadas = series.ToArray(),
                    Id_jugadores = id_jugadores.ToArray(),
                    Id_cartas_mazo = id_cartas_mazo.ToArray()
                });

            }

            return result;

        }


    }
}
