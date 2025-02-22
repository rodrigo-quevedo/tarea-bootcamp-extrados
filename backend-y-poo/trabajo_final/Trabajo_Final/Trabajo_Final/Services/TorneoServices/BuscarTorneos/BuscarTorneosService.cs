using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.ListaTorneos;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.BuscarTorneos
{
    public class BuscarTorneosService : IBuscarTorneosService
    {
        ITorneoDAO torneoDAO;
        ICartaDAO cartaDAO;
        public BuscarTorneosService(ITorneoDAO torneoDao, ICartaDAO cartaDao)
        {
            torneoDAO = torneoDao;
            cartaDAO = cartaDao;
        }


        public async Task<IList<TorneoDTO>> BuscarTorneos(string[] fases, int id_organizador)
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
            IList<TorneoDTO> result = new List<TorneoDTO>();

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


                result.Add(new TorneoDTO()
                {
                    id = torneo.Id,
                    fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    fecha_hora_fin = torneo.Fecha_hora_fin,
                    horario_diario_inicio = torneo.Horario_diario_inicio,
                    horario_diario_fin = torneo.Horario_diario_fin,
                    cantidad_rondas = torneo.Cantidad_rondas,
                    pais = torneo.Pais,
                    fase = torneo.Fase,
                    id_organizador = torneo.Id_organizador,
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
                    id = torneo.Id,
                    fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    fecha_hora_fin = torneo.Fecha_hora_fin,
                    horario_diario_inicio = torneo.Horario_diario_inicio,
                    horario_diario_fin = torneo.Horario_diario_fin,
                    cantidad_rondas = torneo.Cantidad_rondas,
                    pais = torneo.Pais,
                    fase = torneo.Fase,
                    id_organizador = torneo.Id_organizador,
                    series_habilitadas = series.ToArray(),
                    id_jueces_torneo = id_jueces.ToArray(),
                    id_jugadores = id_jugadores.ToArray()
                });

            }

            return result;
        }
    }
}
