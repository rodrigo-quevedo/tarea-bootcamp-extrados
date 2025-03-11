namespace Trabajo_Final.Services.ImagenesServices.BuscarImagenes
{
    public interface IBuscarImagenesService
    {
        public Task<Byte[]> BuscarIlustracion(int id_ilustracion);

        public Task<Byte[]> BuscarFotoPerfil(string id_foto_perfil);
    }
}
