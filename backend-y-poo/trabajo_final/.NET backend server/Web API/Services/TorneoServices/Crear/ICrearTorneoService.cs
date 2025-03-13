namespace Trabajo_Final.Services.TorneoServices.Crear
{
    public interface ICrearTorneoService
    {
        public Task<bool> CrearTorneo(
            int id_organizador,
            string fecha_hora_inicio, string fecha_hora_fin,
            string horario_inicio, string horario_fin,
            string pais,
            string[] series_habilitadas,
            int[] id_jueces
        );
    }
}
