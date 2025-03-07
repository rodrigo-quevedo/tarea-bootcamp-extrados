using DAO.Entidades.Cartas;

namespace Trabajo_Final.Services.CartasServices.BuscarSeries
{
    public interface IBuscarSeriesService
    {
        public Task<IEnumerable<Serie>> BuscarSeries(string[] nombres_series);
    }
}
