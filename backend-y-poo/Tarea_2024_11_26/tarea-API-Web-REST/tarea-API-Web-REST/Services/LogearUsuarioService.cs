using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services
{
    public class LogearUsuarioService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public LogearUsuarioService()
        {
            usuarioDAO = new UsuarioDAO();
        }

        public Usuario LogearUsuario(Credenciales reqBody)
        {
            //buscar usuario
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(reqBody.mail);
            if (usuarioEncontrado == null) throw new NotFoundException($"Error en login: no se encontro al usuario con mail '{reqBody.mail}' en la Base de Datos.");



            //comprarar credenciales
            if (usuarioEncontrado.username != reqBody.username
                ||
                usuarioEncontrado.password != reqBody.password
            ) {
                throw new InvalidCredentialsException($"Las credenciales para el usuario con mail '{reqBody.mail}' son invalidas.");
            }

            return usuarioEncontrado;
              
        }
    }
}
