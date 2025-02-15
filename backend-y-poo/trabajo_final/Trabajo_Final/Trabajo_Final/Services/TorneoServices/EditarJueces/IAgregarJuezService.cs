namespace Trabajo_Final.Services.TorneoServices.EditarJueces
{
    public interface IAgregarJuezService
    {
        public Task<bool> AgregarJuez(int id_torneo, int id_juez);
    }
}
