﻿using DAO.DAOs;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Server.IIS.Core;
using Custom_Exceptions.Exceptions.Exceptions;
using Trabajo_Final.DTO.Request.InputLogin;

namespace Trabajo_Final.Services.UsuarioServices.Login
{
    public class LogearUsuarioService : ILogearUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        public LogearUsuarioService(IUsuarioDAO dao)
        {
            usuarioDAO = dao;
        }


        public async Task<Usuario> LogearUsuario(CredencialesLoginDTO cred)
        {
            //buscar si existe
            Usuario usuarioExistente = await usuarioDAO.BuscarUnUsuarioAsync(new Usuario(0, null, null, null, cred.email, null, true, null));
            if (usuarioExistente == null) throw new NotFoundException($"No se encontró al usuario con email '{cred.email}'");

            //comparar passwords
            bool passValida = Argon2.Verify(usuarioExistente.Password, cred.password);
            if (!passValida) throw new InvalidCredentialsException($"Usuario [{cred.email}]: La contraseña es incorrecta.");

            return usuarioExistente;
        }

    }
}
