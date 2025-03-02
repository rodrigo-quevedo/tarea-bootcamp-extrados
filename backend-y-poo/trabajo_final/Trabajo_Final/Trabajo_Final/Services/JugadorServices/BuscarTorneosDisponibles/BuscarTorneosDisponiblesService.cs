using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.TorneoEntidades;
using Trabajo_Final.DTO.ListaTorneos.ResponseDTO;
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
                await torneoDAO.BuscarTorneosActivos(new Torneo() { Fase = FasesTorneo.REGISTRO});
                        
            //series
            IEnumerable<Serie_Habilitada> series_habilitadas =
                await torneoDAO.BuscarSeriesDeTorneos(torneos);


            //armar DTO
            IList<TorneoDisponibleDTO> result = new List<TorneoDisponibleDTO>();
            
            foreach(Torneo torneo in torneos)
            {
                IList<string> series = new List<string>();
                foreach (Serie_Habilitada serie in series_habilitadas)
                    if (serie.Id_torneo == torneo.Id) 
                        series.Add(serie.Nombre_serie);


                result.Add(new TorneoDisponibleDTO()
                {
                    Id_torneo = torneo.Id,
                    Fecha_hora_inicio = torneo.Fecha_hora_inicio,
                    Fecha_hora_fin = torneo.Fecha_hora_fin,
                    Horario_diario_inicio = torneo.Horario_diario_inicio,
                    Horario_diario_fin = torneo.Horario_diario_fin,
                    Cantidad_rondas = torneo.Cantidad_rondas,
                    Pais = torneo.Pais,
                    Series_habilitadas = series.ToArray()
                });

            }
            return result;
        }

    }
}
