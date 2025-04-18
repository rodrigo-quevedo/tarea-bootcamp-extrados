﻿using DAO.DAOs.UsuarioDao;
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


        public async Task<bool> DesactivarRefreshToken(int id, string refreshToken)
        {
            //si no tiene cookie refreshToken, no hace falta hacer nada
            if (refreshToken == null || refreshToken == "") return true;

            int filasDeTablaAfectadas = await usuarioDAO.BorradoLogicoRefreshTokenAsync(id, refreshToken);
            if (filasDeTablaAfectadas == 0) Console.WriteLine($"No se pudo cerrar la anterior. Refresh token: {refreshToken}");

            return true;

        }

    }
}
