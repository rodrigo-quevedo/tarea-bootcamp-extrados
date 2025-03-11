namespace Trabajo_Final.Services.UsuarioServices.Perfil
{
    public interface IActualizarPerfilService
    {
        public Task<string> ActualizarPerfil(int id_usuario, IFormFile foto, string alias);
    }
}
