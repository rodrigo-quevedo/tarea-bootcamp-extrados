﻿using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services
{
    public class BuscarUsuarioByMailService
    {
        UsuarioDAO usuarioDAO {  get; set; }
        public BuscarUsuarioByMailService()
        {
            usuarioDAO = new UsuarioDAO();
        }


        public Usuario BuscarUsuarioByMail(string mail)
        {
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(mail);

            if (usuarioEncontrado == null) throw new NotFoundException($"No se encontró al usuario con mail '{mail}'");

            usuarioEncontrado.mostrarDatos();
          
            return usuarioEncontrado;
        }


    }
}