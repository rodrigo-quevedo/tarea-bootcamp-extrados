using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services
{
    public class ActualizarUsuarioService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public ActualizarUsuarioService(string connectionString) {
            usuarioDAO = new UsuarioDAO(connectionString);
        }

        public Usuario ActualizarUsuario(Usuario usuario)
        {
            //chequear si el usuario ya existe
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(usuario.mail);
            
            if (usuarioEncontrado == null) throw new NotFoundException($"Error al intentar actualizar: el usuario con mail '{usuario.mail}' no existe.");
            
            Console.WriteLine($"Usuario con mail={usuario.mail} antes del update:");            
            usuarioEncontrado.mostrarDatos();

            
            //actualizar el usuario
            usuarioDAO.ActualizarUsuario(usuario);


            //corroborar que se actualizo el usuario
            Usuario usuarioActualizado = usuarioDAO.BuscarUsuarioPorMail(usuario.mail);
            if (usuarioActualizado == null) throw new NotFoundException($"Error al actualizar: el usuario con mail '{usuario.mail}' no se encuentra en la Base de Datos.");

            usuarioActualizado.mostrarDatos();

            return usuarioActualizado;
        }
    }
}
