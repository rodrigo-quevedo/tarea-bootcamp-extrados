using DAO.Entidades.UsuarioEntidades;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Crear
{
    public interface ICrearRefreshTokenService
    {
        public string CrearRefreshToken(Usuario usuarioValidado);
    }
}
