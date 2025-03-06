using DAO.DAOs.UsuarioDao;
using DAO.Entidades.Custom.DatosUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Buscar
{
    public class BuscarUsuarioService : IBuscarUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        public BuscarUsuarioService(IUsuarioDAO usuarioDao) 
        { 
            usuarioDAO = usuarioDao;
        }

        //Admin y organizador
        public async Task<DatosCompletosUsuarioDTO> BuscarDatosCompletosUsuario(
            int id_logeado, string rol_logueado, int id_usuario)
        {
            //Admin: Datos completos de todos los usuarios
            //Organizador:
                //-Sin acceso a datos de admin/otros organizadores.
                //-Datos completos de jueces (necesita ver los jueces para asignarlos a torneos que organiza)
                //-Datos completos de usuarios que se inscribieron a sus torneos

            return await usuarioDAO.BuscarDatosCompletosUsuario(id_logeado, rol_logueado, id_usuario);

        }

        //Juez y jugador
        public async Task<PerfilUsuarioDTO> BuscarPerfilUsuario(
            int id_logeado, string rol_logueado, int id_usuario)
        {
            //Juez: perfil de jugadores en torneos que oficializó
            //Jugador: perfil de jugadores y jueces en partidas que jugó

            return await usuarioDAO.BuscarPerfilUsuarioDTO(id_logeado, rol_logueado, id_usuario);

        }
    }
}
