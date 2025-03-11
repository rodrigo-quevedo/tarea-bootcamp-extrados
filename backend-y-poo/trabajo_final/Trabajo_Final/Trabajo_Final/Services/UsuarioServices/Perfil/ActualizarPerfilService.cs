using Configuration.FilesPathConfiguration;
using Configuration.ServerRoutes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.UsuarioDao;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading;

namespace Trabajo_Final.Services.UsuarioServices.Perfil
{
    public class ActualizarPerfilService : IActualizarPerfilService
    {
        IUsuarioDAO usuarioDAO;
        IServerRoutesConfiguration serverRoutesConfig;
        IFilesPathsConfigurations filesPathsConfig;

        public ActualizarPerfilService(
            IUsuarioDAO usuarioDao, 
            IServerRoutesConfiguration serverRoutes,
            IFilesPathsConfigurations filesPaths)
        {
            usuarioDAO = usuarioDao;
            serverRoutesConfig = serverRoutes;
            filesPathsConfig = filesPaths;
        }


        public async Task<string> ActualizarPerfil(int id_usuario, IFormFile foto, string alias)
        {
            if (foto == null && alias == null) throw new InvalidInputException("Debe ingresar la 'url_foto' y/o el 'alias' para actualizarlos.");

            if (alias != null) await usuarioDAO.VerificarAliasExistente(id_usuario, alias);

            string url_foto = null;

            if (foto != null)
            {
                string foto_path =
                    filesPathsConfig.GetFotoPerfilPath()
                    + @"\"
                    + id_usuario
                    + Path.GetExtension(foto.FileName);


                //Si la foto anterior tiene la misma extension, el archivo se sobreescribe
                using (var stream = new FileStream(foto_path, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }


                url_foto = 
                    serverRoutesConfig.GetFotoPerfilRoute() 
                    + "/" 
                    + id_usuario  
                    + Path.GetExtension(foto.FileName);
            }

            return await usuarioDAO.ActualizarPerfil(id_usuario, url_foto, alias);
        }



    }
}
