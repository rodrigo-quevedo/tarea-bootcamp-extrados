﻿using Configuration;
using Configuration.DI;
using DAO.Entidades.UsuarioEntidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Crear
{
    public interface ICrearRefreshTokenService
    {
        public string CrearRefreshToken(Usuario usuarioValidado);
    }
}
