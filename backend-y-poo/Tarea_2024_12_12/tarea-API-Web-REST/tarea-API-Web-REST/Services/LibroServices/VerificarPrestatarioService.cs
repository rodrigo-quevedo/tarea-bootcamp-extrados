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

        public void Verificar(string username_prestatario, string jwtUsername)
        {

            //comparar
            if (jwtUsername != username_prestatario)
            {
                throw new SinPermisoException($"El username al que se quiere prestar el libro '{username_prestatario}' es distinto al username logeado '{jwtUsername}'." +
                    $"\nEl usuario logueado SOLO tiene permiso para prestar a si mismo un libro." +
                    $"\nUn usuario logueado NO PUEDE ejecutar un prestamo de libro a otros usuarios.");
            }

            // si coinciden, entonces se continua ejecutando la logica del controller
            return;
        }
    }
}
