namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Desactivar
{
    public interface IDesactivarRefreshTokenService
    {
        public bool DesactivarRefreshToken(int id, string refreshToken);
    }
}
