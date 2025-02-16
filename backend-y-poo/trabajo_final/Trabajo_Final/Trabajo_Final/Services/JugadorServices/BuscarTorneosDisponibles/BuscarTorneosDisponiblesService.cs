using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.ListaTorneos;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.JugadorServices.BuscarTorneosDisponibles
{
    public class BuscarTorneosDisponiblesService: IBuscarTorneosDisponiblesService
    {
        ITorneoDAO torneoDAO;
        ICartaDAO cartaDAO;
        public BuscarTorneosDisponiblesService(ITorneoDAO torneoDao, ICartaDAO cartaDao) 
        {
            torneoDAO = torneoDao;
            cartaDAO = cartaDao; 
        }    

        
        public async Task<IList<TorneoDisponibleDTO>> BuscarTorneosDisponibles()
        {
            //torneos
            IEnumerable<Torneo> torneos = 
                await torneoDAO.BuscarTorneos(new Torneo() { Fase = FasesTorneo.REGISTRO});
                        
            //series
            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);

            //jueces
            IEnumerable<Juez_Torneo> jueces_torneos = 
                await torneoDAO.BuscarJuecesDeTorneos(torneos);


            //armar DTO
            IList<TorneoDisponibleDTO> result = new List<TorneoDisponibleDTO>();
            
            foreach(Torneo torneo in torneos)
            {
                IList<string> series = new List<string>();
                foreach (Serie_Habilitada serie in series_habilitadas)
                    if (serie.Id_torneo == torneo.Id) 
                        series.Add(serie.Nombre_serie);

                IList<int> id_jueces = new List<int>();
                foreach (Juez_Torneo juez in jueces_torneos)
                    if (juez.Id_torneo == torneo.Id)
                        id_jueces.Add(juez.Id_juez);


                result.Add(new TorneoDisponibleDTO()
                {
                    id = torneo.Id,
                    fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    fecha_hora_fin = torneo.Fecha_hora_fin,
                    horario_diario_inicio = torneo.Horario_diario_inicio,
                    horario_diario_fin = torneo.Horario_diario_fin,
                    pais = torneo.Pais,
                    series_habilitadas = series.ToArray(),
                    id_jueces_torneo = id_jueces.ToArray()
                });

            }
            return result;
        }

    }
}
