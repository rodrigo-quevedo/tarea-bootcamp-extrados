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

        public Usuario ActualizarUsuario(string mail, string nombre, int edad)
        {
            Usuario usuarioActualizado = usuarioDAO.ActualizarUsuario(mail, nombre, edad);

            if (usuarioActualizado == null) throw new Exception("usuarioActualizado es null");

            usuarioActualizado.mostrarDatos();

            return usuarioActualizado;
        }
    }
}
