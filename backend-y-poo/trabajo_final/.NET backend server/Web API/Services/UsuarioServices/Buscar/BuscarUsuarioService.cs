using Configuration.ServerURL;
using DAO.DAOs.UsuarioDao;
using DAO.DTOs_en_DAOs.DatosUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Buscar
{
    public class BuscarUsuarioService : IBuscarUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        private IServerURLConfiguration serverUrlConfig;
        public BuscarUsuarioService(IUsuarioDAO usuarioDao, IServerURLConfiguration config) 
        { 
            usuarioDAO = usuarioDao;
            serverUrlConfig = config;
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

            DatosCompletosUsuarioDTO result = 
                await usuarioDAO.BuscarDatosCompletosUsuario(id_logeado, rol_logueado, id_usuario);

            //inyectar URL del server a foto Perfil
            result.Foto = serverUrlConfig.GetServerURL() + result.Foto;

            return result;

        }

        //Admin: buscar todos los usuarios
        public async Task<IEnumerable<DatosCompletosUsuarioDTO>> BuscarDatosCompletosUsuarios()
        {

            IEnumerable<DatosCompletosUsuarioDTO> result =
                await usuarioDAO.BuscarDatosCompletosUsuarios();

            //inyectar URL del server a foto Perfil
            
            foreach (DatosCompletosUsuarioDTO usuario in result)
            {
                //Console.WriteLine($"foto:{usuario.Foto}");
                usuario.Foto = serverUrlConfig.GetServerURL() + usuario.Foto;
            }

            return result;

        }

        //Juez y jugador
        public async Task<PerfilUsuarioDTO> BuscarPerfilUsuario(
            int id_logeado, string rol_logueado, int id_usuario)
        {
            //Juez: perfil de jugadores en torneos que oficializó
            //Jugador: perfil de jugadores y jueces en partidas que jugó

            PerfilUsuarioDTO result = 
                await usuarioDAO.BuscarPerfilUsuarioDTO(id_logeado, rol_logueado, id_usuario);

            //inyectar URL del server a foto Perfil
            result.Foto = serverUrlConfig.GetServerURL() + result.Foto;

            return result;

        }
    }
}
