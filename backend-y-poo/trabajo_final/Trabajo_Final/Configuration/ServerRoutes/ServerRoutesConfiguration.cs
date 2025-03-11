using Configuration.ServerRoutes.RoutesParam;

namespace Configuration.ServerRoutes
{
    public class ServerRoutesConfiguration : IServerRoutesConfiguration
    {
        private Routes Routes_Inyectadas;
        public ServerRoutesConfiguration(Routes parametros)
        {
            Routes_Inyectadas = parametros;
        }

        public string GetIlustracionesRoute()
        {
            return Routes_Inyectadas.Ilustraciones;
        }
    }
}
