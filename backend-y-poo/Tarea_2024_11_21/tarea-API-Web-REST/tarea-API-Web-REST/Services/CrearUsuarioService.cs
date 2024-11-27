using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;

namespace tarea_API_Web_REST.Services
{
    public class CrearUsuarioService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public CrearUsuarioService() { 
            usuarioDAO = new UsuarioDAO();
        }

        public Usuario CrearUsuario(string mail, string nombre, int edad)
        {
            Usuario usuarioCreado = usuarioDAO.CrearUsuario(mail, nombre, edad);

            if (usuarioCreado == null) throw new Exception("usuarioCreado is null");
                
            usuarioCreado.mostrarDatos();
            
            return usuarioCreado;
        }
    }
}
