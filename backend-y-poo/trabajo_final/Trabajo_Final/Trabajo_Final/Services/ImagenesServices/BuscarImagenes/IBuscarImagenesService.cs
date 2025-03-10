namespace Trabajo_Final.Services.ImagenesServices.BuscarImagenes
{
    public interface IBuscarImagenesService
    {
        public Task<Byte[]> BuscarIlustracion(int id_carta);
    }
}
