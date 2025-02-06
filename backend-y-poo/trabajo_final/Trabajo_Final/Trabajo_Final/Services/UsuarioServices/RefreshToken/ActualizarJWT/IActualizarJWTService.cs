using DAO.Entidades;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Validar
{
    public interface IActualizarJWTService
    {
        public string ActualizarJWT(string refreshToken);
    }
}
