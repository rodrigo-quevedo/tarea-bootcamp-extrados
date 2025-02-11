using DAO.DAOs;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Custom_Exceptions.Exceptions.Exceptions;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.AsignarRefreshToken
{
    public class AsignarRefreshTokenService : IAsignarRefreshTokenService
    {
        private IUsuarioDAO usuarioDAO;
        public AsignarRefreshTokenService(IUsuarioDAO dao) 
        {
            usuarioDAO = dao;
        }


        public async Task<bool> AsignarRefreshToken(Usuario usuarioValidado, string refreshToken)
        {
            //guardar refresh token en db
            int filasAfectadas = await usuarioDAO.GuardarRefreshTokenAsync(usuarioValidado.Id, refreshToken);
            if (filasAfectadas == 0) throw new DefaultException("No se pudo guardar el refresh token en la base de datos.");

            return true;
        }
    }
}
