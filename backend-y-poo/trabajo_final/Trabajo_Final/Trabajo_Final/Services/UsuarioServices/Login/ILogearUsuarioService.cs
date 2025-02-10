using DAO.Entidades.UsuarioEntidades;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Login
{
    public interface ILogearUsuarioService
    {
        public Task<Usuario> LogearUsuario(CredencialesLoginDTO cred);
    }
}
