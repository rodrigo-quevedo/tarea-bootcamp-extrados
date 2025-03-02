namespace Trabajo_Final.Services.UsuarioServices.Eliminar
{
    public interface IEliminarUsuarioService
    {
        public Task<bool> EliminarUsuario(int id_usuario);
    }
}
