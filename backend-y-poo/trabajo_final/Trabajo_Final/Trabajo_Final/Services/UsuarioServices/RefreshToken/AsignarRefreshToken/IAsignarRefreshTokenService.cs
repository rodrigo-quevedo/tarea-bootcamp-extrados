using DAO.Entidades.Usuario;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.AsignarRefreshToken
{
    public interface IAsignarRefreshTokenService
    {
        public bool AsignarRefreshToken(Usuario usuarioValidado, string refreshToken);
    }
}
