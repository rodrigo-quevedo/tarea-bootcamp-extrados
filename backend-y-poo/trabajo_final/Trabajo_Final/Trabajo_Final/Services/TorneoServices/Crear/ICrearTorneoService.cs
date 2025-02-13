namespace Trabajo_Final.Services.TorneoServices.Crear
{
    public interface ICrearTorneoService
    {
        public Task<bool> CrearTorneo(
            int id_organizador,
            DateTime fecha_hora_inicio, DateTime fecha_hora_fin,
            string pais,
            string[] series_habilitadas,
            int[] id_jueces
        );
    }
}
