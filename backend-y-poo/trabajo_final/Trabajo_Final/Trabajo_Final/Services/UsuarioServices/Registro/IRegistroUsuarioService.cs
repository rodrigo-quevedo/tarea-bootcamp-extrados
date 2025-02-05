using DAO.Entidades;
using System.Security.Claims;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public interface IRegistroUsuarioService
    {
        public Usuario RegistrarUsuario(DatosRegistroDTO datos);
        public Usuario RegistrarUsuario(DatosRegistroDTO datos, int id_usuario_creador);

        public Usuario RegistrarUsuario(DatosRegistroDTO datos, ClaimsPrincipal claims_usuario_creador);
    }
}
