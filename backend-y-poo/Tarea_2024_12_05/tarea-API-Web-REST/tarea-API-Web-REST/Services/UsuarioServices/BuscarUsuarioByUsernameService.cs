using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class BuscarUsuarioByUsernameService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public BuscarUsuarioByUsernameService(string connectionString)
        {
            usuarioDAO = new UsuarioDAO(connectionString);
        }


        public Usuario BuscarUsuario(string username)
        {
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorUsername(username);

            if (usuarioEncontrado == null) throw new NotFoundException($"No se encontró al usuario con username '{username}'");

            usuarioEncontrado.mostrarDatos();

            return usuarioEncontrado;
        }


    }
}
