using DAO.DAOs.DI;
using DAO.Entidades;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public class RegistrarUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        public RegistrarUsuarioService(IUsuarioDAO usuarioDAO)
        {
            this.usuarioDAO = usuarioDAO;
        }

        //Consideraciones: (Rol usuario -> rol que puede registrar)
        //admin -> admin, organizador, juez, jugador
        //organizador -> juez
        //juez -> ninguno
        //jugador -> ninguno
        //usuario no logeado -> jugador
        //public Usuario RegistrarUsuario(DatosRegistroDTO cred)
        //{

        //}
    }
}
