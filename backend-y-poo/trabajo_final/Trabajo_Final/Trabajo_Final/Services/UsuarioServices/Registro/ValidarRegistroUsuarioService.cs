using Configuration;
using Configuration.DI;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trabajo_Final.DTO;
using Trabajo_Final.utils.Constantes;
using Trabajo_Final.utils.Exceptions.Exceptions;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public class ValidarRegistroUsuarioService : IValidarRegistroUsuarioService
    {
        private IJwtConfiguration jwtConfiguration;
        private IUsuarioDAO usuarioDAO;
        private IRegistroUsuarioService registroUsuarioService;

        public ValidarRegistroUsuarioService(IJwtConfiguration jwtConf, IUsuarioDAO usuarioDAO, IRegistroUsuarioService registroUsuario)
        {
            this.jwtConfiguration = jwtConf;
            this.usuarioDAO = usuarioDAO;
            this.registroUsuarioService = registroUsuario;
        }

        //Consideraciones: (Rol usuario -> rol que puede registrar)
        //admin -> admin, organizador, juez, jugador
        //organizador -> juez
        //juez -> ninguno
        //jugador -> ninguno
        //usuario no logeado -> jugador
        public Usuario ValidarRegistroUsuario(DatosRegistroDTO datosUsuarioARegistrar, string jwt)
        {
            //-->validar jwt
            JwtSecurityTokenHandler jwtHandler = new();
            ClaimsPrincipal jwt_claims = jwtHandler.ValidateToken(
                jwt,
                new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfiguration.issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.jwt_secret))
                },
                out SecurityToken secToken
            );

            //-->obtener ClaimTypes.Role
            Claim claim_rol_verificado = jwt_claims.FindFirst(ClaimTypes.Role);
            string rol_verificado = claim_rol_verificado.Value;

            //rol = juez o rol = jugador -> no pueden registrar usuarios
            if (rol_verificado == Roles.JUEZ || rol_verificado == Roles.JUGADOR)
                throw new SinPermisoException($"Intentó registrar un [{datosUsuarioARegistrar.rol}], pero no tiene el rol requerido. Realize el login como admin u organizador e intente nuevamente. (O puede crear un usuario con rol [jugador] sin logearse. ADVERTENCIA: Solo se permite 1 rol por cuenta,es decir, por email).");


            //rol = admin: puede crear cualquier cosa
            if (rol_verificado == Roles.ADMIN)
            {
                return registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar);

                
            }


            //rol = organizador: chequear datos.rol = juez
            if (rol_verificado == Roles.ORGANIZADOR && datosUsuarioARegistrar.rol == Roles.JUEZ)
            {
                return registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar);
            }

            return null;
        }
    }
}
