
using Custom_Exceptions.Exceptions.Exceptions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Trabajo_Final.Services.ImagenesServices.BuscarImagenes
{
    public class BuscarImagenesService : IBuscarImagenesService
    {
        private string ilustraciones_os_path;
        public BuscarImagenesService(string ilustraciones_path)
        {
            this.ilustraciones_os_path = ilustraciones_path;
        }

        public async Task<byte[]> BuscarIlustracion(int id_carta)
        {
            try
            {
                byte[] result =
                    await File.ReadAllBytesAsync(@$"{ilustraciones_os_path}/{id_carta}.jpg");

                if (result == null || !result.Any()) throw new Exception($"No se pudo encontrar la imagen de la carta [{id_carta}]");

                return result;
            }
            catch (Exception ex) 
            {
                if (ex.Message.Contains("Could not find file")) throw new NotFoundException($"No se encontró la ilustracion de la carta [{id_carta}].");

                throw ex;
            }
            
        }
    }

}
