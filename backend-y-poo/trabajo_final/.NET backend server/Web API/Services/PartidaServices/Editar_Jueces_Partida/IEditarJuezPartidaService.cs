namespace Trabajo_Final.Services.PartidaServices.Editar_Jueces_Partida
{
    public interface IEditarJuezPartidaService
    {

        public Task<bool> EditarJuezPartida(int id_organizador, int id_partida, int id_juez);
    }
}
