using DAO.Entidades;
using System.Security.Claims;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public interface IRegistroUsuarioService
    {
        //autoregistro de jugadores:
        public bool RegistrarUsuario(DatosRegistroDTO datos);


        //usuario logeado registra a otro jugador:
        public bool RegistrarUsuario(DatosRegistroDTO datos, int id_usuario_creador);
    }
}
