using Configuration;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tarea_API_Web_REST.Services.UsuarioServices;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.Authorization
{
    public class AuthorizeCookieService
    {
        JwtConfiguration Configuration { get; set; }
        JwtSecurityTokenHandler jwtHandler { get; set; }
        CrearJwtService crearJwtService { get; set; }
        BuscarUsuarioByUsernameService buscarUsuarioByUsernameService { get; set; }
        public AuthorizeCookieService(JwtConfiguration jwtConfiguration, string connection_string)
        {
            Configuration = jwtConfiguration;
            jwtHandler = new();
            crearJwtService = new(Configuration);
            buscarUsuarioByUsernameService = new (connection_string);
        }

        // Devuelve string o Exception
        public string AuthorizeJWT(IRequestCookieCollection cookies, IResponseCookies resCookies)
        {
            //obtener el jwt
            string jwt = cookies["jwt"];


            //validar jwt
            try
            {
                SecurityToken secToken;
                
                //si es invalido, tira una Exception:
                jwtHandler.ValidateToken(
                    jwt,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.issuer,

                        ValidateAudience = true,
                        ValidAudience = Configuration.audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.jwt_secret))
                    },
                    out secToken
                );

                //parse jwt:
                JwtSecurityToken jwtSecToken = (JwtSecurityToken)secToken;
                JwtPayload jwtPayload = jwtSecToken.Payload;
                Object username_logueado;
                jwtPayload.TryGetValue(ClaimTypes.Sid, out username_logueado);
                Console.WriteLine($"Username logueado: {username_logueado}");

                //devolver jwt string:
                return username_logueado.ToString();


            }
            
            //si no es null pero tira una Exception, se pasa a revisar refreshToken:
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                // --> revisar refreshToken:
                return AuthorizeRefreshToken(cookies, resCookies);
            }

        }

        // AuthorizeRefreshToken: Comprueba la cookie refreshToken y devuelve una cookie jwt.
        private string AuthorizeRefreshToken(IRequestCookieCollection cookies, IResponseCookies resCookies)
        {
            //obtener el refreshToken
            string refreshToken = cookies["refreshToken"];

            //chequear que no sea null
            if (refreshToken == null) throw new InvalidRefreshTokenException("Token inválido o expirado. Logear usuario nuevamente.");

            //parse refreshToken:
            JwtSecurityToken jwtSecToken = jwtHandler.ReadJwtToken(refreshToken);//ReadJwtToken() no hace validacion, pero no la necesito porque estoy comparando contra el que tengo en la base de datos
            
            //obtener username dentro del refreshToken payload:
            Object username_logueado;
            jwtSecToken.Payload.TryGetValue(ClaimTypes.Sid, out username_logueado);
            Console.WriteLine($"Username logueado: {username_logueado}");

            //buscar usuario con el username del refreshToken en la base de datos:
            Usuario usuarioEncontrado = buscarUsuarioByUsernameService.BuscarUsuario(username_logueado.ToString() );

            //validacion guard-> comprarar refreshToken de la base de datos contra el de la cookie:
            if (usuarioEncontrado.refresh_token != refreshToken) throw new InvalidRefreshTokenException("Token inválido. Logear usuario nuevamente.");

            //si es valido, crear un jwt (y agregarlo al Cookie del server response):
            string jwtCreado = crearJwtService.CrearJwt(usuarioEncontrado, resCookies);

            return usuarioEncontrado.username;


        }

        // se asume un jwt validado (usar este metodo SOLO despues de validar el jwt)
        public void AuthorizeRoles(string[] Roles, IRequestCookieCollection cookies)
        {
            string jwt = cookies["jwt"];

            //parse refreshToken:
            JwtSecurityToken jwtSecToken = jwtHandler.ReadJwtToken(jwt);

            //obtener Role dentro del refreshToken payload:
            Object role_jwt;
            jwtSecToken.Payload.TryGetValue(ClaimTypes.Role, out role_jwt);
            Console.WriteLine($"Rol del username en jwt: {role_jwt.ToString()}");

            Object username_jwt;
            jwtSecToken.Payload.TryGetValue(ClaimTypes.Sid, out username_jwt);
            Console.WriteLine($"Username en jwt: {username_jwt.ToString()}");

            //comparar role del jwt con roles permitidos:
            bool rolEncontrado = false;
            foreach (string Role in Roles)
            {
                if (Role == role_jwt.ToString()) rolEncontrado = true;
            }

            if (!rolEncontrado) throw new SinPermisoException($"El usuario '{username_jwt}'con rol '{role_jwt}' no tiene permiso para hacer esta accion.");

            return;
        }

    }
}
