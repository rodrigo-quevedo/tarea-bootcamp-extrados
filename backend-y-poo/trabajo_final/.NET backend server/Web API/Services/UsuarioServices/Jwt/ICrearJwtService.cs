﻿using DAO.Entidades.UsuarioEntidades;

namespace Trabajo_Final.Services.UsuarioServices.Jwt
{
    public interface ICrearJwtService
    {
        public string CrearJwt(Usuario usuarioValidado);
    }
}
