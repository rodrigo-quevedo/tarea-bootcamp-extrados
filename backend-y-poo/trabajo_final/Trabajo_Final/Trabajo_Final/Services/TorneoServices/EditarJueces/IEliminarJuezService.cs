namespace Trabajo_Final.Services.TorneoServices.EditarJueces
{
    public interface IEliminarJuezService
    {
        public Task<bool> EliminarJuez(int id_organizador, int id_torneo, int id_juez);
    }
}
