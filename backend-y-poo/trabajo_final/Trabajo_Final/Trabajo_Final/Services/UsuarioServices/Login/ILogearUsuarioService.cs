using DAO.Entidades;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Login
{
    public interface ILogearUsuarioService
    {
        public Usuario LogearUsuario(CredencialesLoginDTO cred);
    }
}
