using Configuration;
using Configuration.DI;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.Services.UsuarioServices.Login;
using Trabajo_Final.Services.UsuarioServices.RefreshToken;
using Trabajo_Final.Services.UsuarioServices.Registro;
using Trabajo_Final.utils.Constantes;
using Trabajo_Final.utils.Exceptions.Exceptions;
using Trabajo_Final.utils.Verificar_Existencia_Admin;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        // services
        private ILogearUsuarioService logearUsuarioService;
        private ICrearJwtService crearJwtService;
        private ICrearRefreshTokenService crearRefreshTokenService;
        private IJwtConfiguration jwtConfiguration;
        
        private IRegistroUsuarioService registroUsuarioService;
        private IValidarRegistroUsuarioService validarRegistroService;


        //private IRegistrarUsuarioService registrarUsuarioService;



        public UsuariosController(
            IVerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            
            ILogearUsuarioService login,
            ICrearJwtService jwt,
            ICrearRefreshTokenService  refreshToken,
            IJwtConfiguration jwtConfig,

            IRegistroUsuarioService registro,
            IValidarRegistroUsuarioService validarRegistro
        )
        {
            logearUsuarioService = login;
            crearJwtService = jwt;
            crearRefreshTokenService = refreshToken;
            jwtConfiguration = jwtConfig;

            registroUsuarioService = registro;
            validarRegistroService = validarRegistro;

        }




        [HttpPost]
        [Route("/login")]

        //validacion de inputs en el DTO con DataAnnotations
        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
            Console.WriteLine("POST /login");

            //Verificar que no esté logeado
            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();

            if (
                (authorizationHeaderValue != null  && authorizationHeaderValue != "" )
                || 
                (Request.Cookies["refreshToken"] != null && Request.Cookies["refreshToken"] != "")
            )
            {
                throw new AlreadyLoggedInException("Ya está logeado. Cierre su sesión actual para poder loguearse (ir a /logout).");
            }


            //Verificar credenciales
            Usuario usuarioVerificado = logearUsuarioService.LogearUsuario(credenciales);

            //Crear jwt y refresh token
            string jwt = crearJwtService.CrearJwt(usuarioVerificado);
            string refreshToken = crearRefreshTokenService.CrearRefreshToken(usuarioVerificado);

            //Respuesta servidor
            this.HttpContext.Items["Authorization_value"] = $"Bearer {jwt}"; //fix para Exceptions
            
            Response.Headers.Authorization = new StringValues($"Bearer {jwt}");

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now.AddDays(120)
            });
            this.HttpContext.Items["RefreshToken_value"] = refreshToken; //fix para Exceptions


            return Ok(new { message = $"Usuario {usuarioVerificado.Email} logeado con éxito."});

        }

        [HttpPost]
        [Route("/registro")]

        //validacion de inputs en el DTO con DataAnnotations

        //Consideraciones: (Rol usuario -> rol que puede registrar)
        //admin -> admin, organizador, juez, jugador
        //organizador -> juez
        //juez -> ninguno
        //jugador -> ninguno
        //usuario no logeado -> jugador
        public ActionResult RegistrarUser(DatosRegistroDTO datosUsuarioARegistrar)
        {
            Console.WriteLine("POST /registro");

            //si usuario no está logeado, hacer autoregistro de JUGADOR:
            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            
            if (
                (authorizationHeaderValue == null || authorizationHeaderValue == "")
                &&
                (Request.Cookies["refreshToken"] == null || Request.Cookies["refreshToken"] == "")
            )
            {
                if (datosUsuarioARegistrar.rol != Roles.JUGADOR) { throw new SinPermisoException($"Se intentó registrar un [{datosUsuarioARegistrar.rol}], pero no esta logeado. Realize el login como admin u organizador e intente nuevamente. (O puede crear un usuario con rol [jugador] sin logearse. ADVERTENCIA: Solo se permite 1 rol por cuenta,es decir, por email)."); }

                Usuario usuarioAutoregistrado = registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar);   
                return Ok(new { message = $"Usuario '{usuarioAutoregistrado.Email}' se autoregistró con éxito." });
            }


            //Si está logeado, registrar a otro usuario:

            //-->Parsear header y obtener jwt
            string jwt = authorizationHeaderValue.Replace("Bearer ", "");

            //-->Verificar que usuario logeado tenga rol ADMIN u ORGANIZADOR (si es ORGANIZADOR, que solo pueda crear un JUEZ)
            Usuario usuarioRegistrado = validarRegistroService.ValidarRegistroUsuario(datosUsuarioARegistrar, jwt);
            if (usuarioRegistrado != null) return Ok(new { message = $"Usuario '{usuarioRegistrado.Email}' registrado con éxito." });
            

            //Cualquier otro caso significa que no se tiene el permiso necesario:
            throw new SinPermisoException($"Intentó registrar un [{datosUsuarioARegistrar.rol}], pero no tiene el rol requerido. Realize el login como admin u organizador e intente nuevamente. (O puede crear un usuario con rol [jugador] sin logearse. ADVERTENCIA: Solo se permite 1 rol por cuenta,es decir, por email).");

        }

        [HttpGet]
        [Route("/logout")]
        public ActionResult LogoutUser()
        {
            Response.Headers.Authorization = "";
            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now
            });

            //fix para Exceptions
            this.HttpContext.Items["Authorization_value"] = "";
            this.HttpContext.Items["RefreshToken_value"] = "";
            
            return Ok(new { message = "Sesión cerrada con éxito." });
        }


        //test authorize
        [HttpGet]
        [Route("/datos")]
        [Authorize]
        public ActionResult mostrarDatosUsuario()
        {
            Console.WriteLine("GET /datos");

            return Ok(new {message="[autorización con éxito]"});
        }
    }
}
