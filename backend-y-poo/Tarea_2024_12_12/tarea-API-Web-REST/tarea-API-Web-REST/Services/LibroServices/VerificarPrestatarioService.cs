using Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.LibroServices
{
    public class VerificarPrestatarioService
    {
        JwtConfiguration _jwtConfiguration;
        public VerificarPrestatarioService(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }

        public void Verificar(string username_prestatario, string jwtString)
        {
          

            //parsear el jwt string para obtener su payload
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SecurityToken secToken;
            jwtHandler.ValidateToken(
                jwtString, 
                new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfiguration.issuer,

                    ValidateAudience = true,
                    ValidAudience = _jwtConfiguration.audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.secret))
                }, out secToken);

            //JwtSecurityToken jwtSecToken = jwtHandler.ReadJwtToken(jwtString);
            JwtSecurityToken jwtSecToken = (JwtSecurityToken)secToken;
            JwtPayload jwtPayload = jwtSecToken.Payload;

            Object username_logueado;
            jwtPayload.TryGetValue(ClaimTypes.Sid, out username_logueado);
            Console.WriteLine($"Username logueado: {username_logueado}");

            //comparar
            if ( username_logueado.ToString() != username_prestatario)
            {
                throw new SinPermisoException($"El username al que se quiere prestar el libro '{username_prestatario}' es distinto al username logeado '{username_logueado}'." +
                    $"\nEl usuario logueado SOLO tiene permiso para prestar a si mismo un libro." +
                    $"\nUn usuario logueado NO PUEDE ejecutar un prestamo de libro a otros usuarios.");
            }

            // si coinciden, entonces se continua ejecutando la logica del controller
            return;
        }
    }
}
