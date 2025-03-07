using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;

namespace Trabajo_Final.Services.CartasServices.BuscarSeries
{
    public class BuscarSeriesService : IBuscarSeriesService
    {
        private ICartaDAO cartaDAO;
        public BuscarSeriesService(ICartaDAO cartaDao) 
        {
            cartaDAO = cartaDao;
        }

        public async Task<IEnumerable<Serie>> BuscarSeries(string[] nombres_series)
        {

            IEnumerable<Serie> series = 
                await cartaDAO.BuscarSeries(nombres_series);

            if (series == null || !series.Any())
                throw new Exception("No se pudo obtener ninguna informacion de las series buscadas.");

            return series;
        }
    }
}
