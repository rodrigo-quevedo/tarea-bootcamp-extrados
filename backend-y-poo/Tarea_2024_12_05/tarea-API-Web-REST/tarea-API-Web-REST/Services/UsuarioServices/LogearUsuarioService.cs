using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using Isopoh.Cryptography.Argon2;
using tarea_API_Web_REST.Utils.Exceptions;
using tarea_API_Web_REST.Utils.RequestBodyParams;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class LogearUsuarioService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public LogearUsuarioService(string connectionString)
        {
            usuarioDAO = new UsuarioDAO(connectionString);
        }

        public Usuario LogearUsuario(Credenciales reqBody)
        {
            //buscar usuario
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(reqBody.mail);
            if (usuarioEncontrado == null) throw new NotFoundException($"Error en login: no se encontro al usuario con mail '{reqBody.mail}' en la Base de Datos.");



            //comprarar credenciales
            if (usuarioEncontrado.username != reqBody.username
                ||
                !Argon2.Verify(usuarioEncontrado.password, reqBody.password)
            )
            {
                throw new InvalidCredentialsException($"Las credenciales para el usuario con mail '{reqBody.mail}' son invalidas.");
            }

            return usuarioEncontrado;

        }
    }
}
