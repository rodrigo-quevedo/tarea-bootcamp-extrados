using DAO.DAOs;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Trabajo_Final.utils.Exceptions.BaseException;
using Trabajo_Final.utils.Exceptions.Exceptions;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.AsignarRefreshToken
{
    public class AsignarRefreshTokenService : IAsignarRefreshTokenService
    {
        private IUsuarioDAO usuarioDAO;
        public AsignarRefreshTokenService(IUsuarioDAO dao) 
        {
            usuarioDAO = dao;
        }


        public bool AsignarRefreshToken(Usuario usuarioValidado, string refreshToken)
        {
            //guardar refresh token en db
            int filasAfectadas = usuarioDAO.GuardarRefreshToken(usuarioValidado.Id, refreshToken);
            if (filasAfectadas == 0) throw new DefaultException("No se pudo guardar el refresh token en la base de datos.");

            return true;
        }
    }
}
