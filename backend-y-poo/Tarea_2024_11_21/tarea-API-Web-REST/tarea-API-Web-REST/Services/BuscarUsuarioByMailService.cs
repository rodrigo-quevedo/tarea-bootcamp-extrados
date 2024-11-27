using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;

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

            if (usuarioEncontrado == null) throw new Exception("usuarioEncontrado is null");

            usuarioEncontrado.mostrarDatos();
          
            return usuarioEncontrado;
        }


    }
}
