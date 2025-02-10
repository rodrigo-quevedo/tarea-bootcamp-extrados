namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Desactivar
{
    public interface IDesactivarRefreshTokenService
    {
        public Task<bool> DesactivarRefreshToken(int id, string refreshToken);
    }
}
