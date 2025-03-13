namespace Trabajo_Final.Services.TorneoServices.ValidarJueces
{
    public interface IValidarJuecesService
    {
        public Task<bool> ValidarIdsJueces(int[] id_jueces);
    }
}
