
using Configuration.FilesPathConfiguration;
using Custom_Exceptions.Exceptions.Exceptions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Trabajo_Final.Services.ImagenesServices.BuscarImagenes
{
    public class BuscarImagenesService : IBuscarImagenesService
    {
        private IFilesPathsConfigurations filesPathConfig;
        public BuscarImagenesService(IFilesPathsConfigurations config)
        {
            this.filesPathConfig = config;
        }

        public async Task<byte[]> BuscarIlustracion(int id_ilustracion)
        {
            try
            {
                //En este caso todas las ilustraciones de cartas se van a guardar
                //en formato JPG, y ya que son guardadas por la app y no por los usuarios,
                //es más fácil trabajarlas en un solo formato.
                byte[] result =
                    await File.ReadAllBytesAsync(@$"{filesPathConfig.GetIlustracionesPath()}/{id_ilustracion}.jpg");

                if (result == null || !result.Any()) throw new Exception($"No se pudo encontrar la imagen de la carta [{id_ilustracion}]");

                return result;
            }
            catch (Exception ex) 
            {
                if (ex.Message.Contains("Could not find file")) throw new NotFoundException($"No se encontró la ilustracion de la carta [{id_ilustracion}].");

                throw ex;
            }
            
        }

        public async Task<byte[]> BuscarFotoPerfil(string id_foto_perfil)
        {
            try
            {
                //En el caso de las fotos perfil, como son los usuarios los que las
                //cargan, puede haber múltiples formatos, por lo que se va a guardar
                //la extensión del archivo como parte de la ID/route.
                byte[] result =
                    await File.ReadAllBytesAsync(@$"{filesPathConfig.GetFotoPerfilPath()}/{id_foto_perfil}");

                if (result == null || !result.Any()) throw new Exception($"No se pudo encontrar la foto perfil [{id_foto_perfil}]");

                return result;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Could not find file")) throw new NotFoundException($"No se encontró la foto perfil [{id_foto_perfil}].");

                throw ex;
            }
        }


    }

}
