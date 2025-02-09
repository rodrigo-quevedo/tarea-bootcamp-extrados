﻿using DAO.Entidades.UsuarioEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.UsuarioDao
{
    public interface IUsuarioDAO
    {

        public int CrearUsuario(Usuario usuario);
        public Task<int> CrearUsuarioAsync(Usuario usuario);
        public Task<int> CrearUsuarioAsync(Usuario usuario, int id_usuario_creador);
        
        public Usuario BuscarUnUsuario(Usuario usuario);
        public Task<Usuario> BuscarUnUsuarioAsync(Usuario usuario);

        public Task<int> GuardarRefreshTokenAsync(int id, string refreshToken);
        public Task<int> BorradoLogicoRefreshTokenAsync(int id, string refreshToken);
        public Task<Refresh_Token> BuscarRefreshTokenAsync(string refreshToken, bool activo);

    }
}
