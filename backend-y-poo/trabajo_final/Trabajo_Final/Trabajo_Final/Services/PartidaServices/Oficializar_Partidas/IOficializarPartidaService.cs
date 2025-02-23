namespace Trabajo_Final.Services.PartidaServices.Oficializar_Partidas
{
    public interface IOficializarPartidaService
    {
        public Task<bool> OficializarPartida(
            int id_juez,
            int id_partida,
            int id_ganador,
            int? id_descalificado
            );
    }
}
