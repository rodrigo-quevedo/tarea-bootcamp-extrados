using DAO.Entidades.Usuario;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Login
{
    public interface ILogearUsuarioService
    {
        public Usuario LogearUsuario(CredencialesLoginDTO cred);
    }
}
