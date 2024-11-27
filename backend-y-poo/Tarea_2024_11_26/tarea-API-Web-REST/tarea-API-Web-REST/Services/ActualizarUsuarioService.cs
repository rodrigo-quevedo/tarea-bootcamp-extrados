using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;

namespace tarea_API_Web_REST.Services
{
    public class ActualizarUsuarioService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public ActualizarUsuarioService() {
            usuarioDAO = new UsuarioDAO();
        }

        public Usuario ActualizarUsuario(Usuario usuario)
        {
            //chequear si el usuario ya existe
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(usuario.mail);
            
            if (usuarioEncontrado == null) throw new Exception($"El usuario con mail '{usuario.mail}' no existe.");
            
            Console.WriteLine($"Usuario con mail={usuario.mail} antes del update:");            
            usuarioEncontrado.mostrarDatos();

            
            //actualizar el usuario
            usuarioDAO.ActualizarUsuario(usuario);


            //corroborar que se actualizo el usuario
            Usuario usuarioActualizado = usuarioDAO.BuscarUsuarioPorMail(usuario.mail);
            if (usuarioActualizado == null) throw new Exception("usuarioActualizado es null");

            usuarioActualizado.mostrarDatos();

            return usuarioActualizado;
        }
    }
}
