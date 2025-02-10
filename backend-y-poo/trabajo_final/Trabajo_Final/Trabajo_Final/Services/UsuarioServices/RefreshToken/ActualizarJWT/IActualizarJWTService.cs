using DAO.Entidades;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Validar
{
    public interface IActualizarJWTService
    {
        public Task<string> ActualizarJWT(string refreshToken);
    }
}
