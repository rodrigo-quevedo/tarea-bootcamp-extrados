using DAO.Entidades;

namespace Trabajo_Final.Services.UsuarioServices.Jwt
{
    public interface ICrearJwtService
    {
        public string CrearJwt(Usuario usuarioValidado, IResponseCookies resCookies);
    }
}
