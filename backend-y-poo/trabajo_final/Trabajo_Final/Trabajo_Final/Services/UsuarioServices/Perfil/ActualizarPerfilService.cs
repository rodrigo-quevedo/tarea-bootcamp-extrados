using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.UsuarioDao;

namespace Trabajo_Final.Services.UsuarioServices.Perfil
{
    public class ActualizarPerfilService : IActualizarPerfilService
    {
        IUsuarioDAO usuarioDAO;

        public ActualizarPerfilService(IUsuarioDAO usuarioDao)
        {
            usuarioDAO = usuarioDao;
        }


        public async Task<string> ActualizarPerfil(int id_usuario, string url_foto, string alias)
        {
            if (url_foto == null && alias == null)
                throw new InvalidInputException("Debe ingresar la 'url_foto' y/o el 'alias' para actualizarlos.");

            if (alias != null) await usuarioDAO.VerificarAliasExistente(id_usuario, alias);

            return await usuarioDAO.ActualizarPerfil(id_usuario, url_foto, alias);

        }
    }
}
