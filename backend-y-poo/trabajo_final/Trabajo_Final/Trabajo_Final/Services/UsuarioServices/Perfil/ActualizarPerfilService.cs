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
            string responseMessage;

            if (url_foto == null && alias == null)
                throw new InvalidInputException("Debe ingresar la 'url_foto' y/o el 'alias' para actualizarlos.");
                
            if (url_foto != null && alias != null)
                responseMessage = await usuarioDAO.ActualizarPerfil(id_usuario, url_foto, alias);

            else if (url_foto != null)
                responseMessage = await usuarioDAO.ActualizarFotoPerfil(id_usuario, url_foto);

            else
                responseMessage = await usuarioDAO.ActualizarAliasPerfil(id_usuario, alias);

            return responseMessage;
        }
    }
}
