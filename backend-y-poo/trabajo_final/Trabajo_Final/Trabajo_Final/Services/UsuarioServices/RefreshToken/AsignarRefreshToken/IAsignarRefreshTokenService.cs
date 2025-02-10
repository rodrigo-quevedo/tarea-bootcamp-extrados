using DAO.Entidades.UsuarioEntidades;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.AsignarRefreshToken
{
    public interface IAsignarRefreshTokenService
    {
        public Task<bool> AsignarRefreshToken(Usuario usuarioValidado, string refreshToken);
    }
}
