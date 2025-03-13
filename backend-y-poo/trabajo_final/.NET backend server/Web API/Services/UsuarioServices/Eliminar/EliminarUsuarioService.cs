using DAO.DAOs.UsuarioDao;

namespace Trabajo_Final.Services.UsuarioServices.Eliminar
{
    public class EliminarUsuarioService : IEliminarUsuarioService
    {
        IUsuarioDAO usuarioDAO;
        public EliminarUsuarioService(IUsuarioDAO usuarioDao) { 
            usuarioDAO = usuarioDao;
        }

        public async Task<bool> EliminarUsuario(int id_usuario)
        {
            return await usuarioDAO.BorradoLogicoUsuarioYSesionesActivas(id_usuario);
        }
    }
}
