using Configuration;

namespace tarea_API_Web_REST.Services.Authorization
{
    public class AuthorizeCookieService
    {
        JwtConfiguration Configuration { get; set; }
        public AuthorizeCookieService(JwtConfiguration jwtConfiguration)
        {
            Configuration = jwtConfiguration;
        }

        public void AuthorizeJWT(IRequestCookieCollection cookies)
        {
            //obtener el jwt
            string jwt = cookies["jwt"];

            //validar jwt
            if (jwt == null) AuthorizeRefreshToken(cookies);


        }

        public void AuthorizeRefreshToken(IRequestCookieCollection cookies)
        {

        }


    }
}
