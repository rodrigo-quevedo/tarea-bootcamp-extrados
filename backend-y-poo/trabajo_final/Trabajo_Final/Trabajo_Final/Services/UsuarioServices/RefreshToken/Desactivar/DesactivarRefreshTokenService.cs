using DAO.DAOs.DI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Desactivar
{
    public class DesactivarRefreshTokenService : IDesactivarRefreshTokenService
    {
        private IUsuarioDAO usuarioDAO;
        public DesactivarRefreshTokenService(IUsuarioDAO dao)
        {
            usuarioDAO = dao;
        }


        public bool DesactivarRefreshToken(int id, string refreshToken)
        {
            //si no tiene cookie refreshToken, no hace falta hacer nada
            if (refreshToken == null || refreshToken == "") return true;

            int filasDeTablaAfectadas = usuarioDAO.BorradoLogicoRefreshToken(id, refreshToken);
            if (filasDeTablaAfectadas == 0) throw new Exception("Se intentó crear una nueva sesión pero no se pudo cerrar la anterior.");//error 500

            return true;

        }

    }
}
