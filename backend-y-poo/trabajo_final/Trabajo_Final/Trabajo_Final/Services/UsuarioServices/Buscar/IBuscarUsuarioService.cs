using DAO.Entidades.Custom.DatosUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Buscar
{
    public interface IBuscarUsuarioService
    {
        public Task<DatosCompletosUsuarioDTO> BuscarDatosCompletosUsuario(
            int id_logeado, string rol_logeado, int id_usuario);
        public Task<PerfilUsuarioDTO> BuscarPerfilUsuario(
            int id_logeado, string rol_logeado, int id_usuario);

    }
}
