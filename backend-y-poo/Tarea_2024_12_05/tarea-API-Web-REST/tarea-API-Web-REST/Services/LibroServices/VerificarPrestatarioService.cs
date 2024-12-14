using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.LibroServices
{
    public class VerificarPrestatarioService
    {
        public void Verificar(string username_prestatario, string jwtBearerString)
        {
            //parsear el header Authorization Bearer (viene con type string)
            string[] arrJwtBearerString = jwtBearerString.Split(" ");
            string jwtString = arrJwtBearerString[1];

            //parsear el jwt string para obtener su payload
            JwtSecurityToken jwtSecToken = new JwtSecurityTokenHandler().ReadJwtToken(jwtString);
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
