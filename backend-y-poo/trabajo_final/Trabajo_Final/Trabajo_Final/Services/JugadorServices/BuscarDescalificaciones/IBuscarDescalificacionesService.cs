using DAO.Entidades.Custom.Descalificaciones;

namespace Trabajo_Final.Services.JugadorServices.BuscarDescalificaciones
{
    public interface IBuscarDescalificacionesService
    {
        public Task<IEnumerable<DescalificacionDTO>> BuscarDescalificaciones(int id_jugador);
    }
}
