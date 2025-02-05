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

        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
            Console.WriteLine("POST /login");

            //Verificar que no esté logeado
            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            
            if (authorizationHeaderValue != null  && authorizationHeaderValue != "" )
                throw new AlreadyLoggedInException("Ya está logeado. Cierre su sesión actual para poder loguearse (ir a /logout).");

            //Verificar credenciales
            Usuario usuarioVerificado = logearUsuarioService.LogearUsuario(credenciales);

            //Crear jwt y refresh token
            string jwt = crearJwtService.CrearJwt(usuarioVerificado);
            string refreshToken = crearRefreshTokenService.CrearRefreshToken(usuarioVerificado);

        //Respuesta servidor:

            //Para cada dispositivo nuevo, se va a crear un nuevo refreshToken.
            //Si ya hay una cookie refreshToken (mismo dispositivo), se va a sobreescribir
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now.AddDays(120)
            });

            return Ok(new { 
                message = $"Usuario {usuarioVerificado.Email} logeado con éxito.",
                jwt = jwt
            });

        }

        //Consideraciones al registrar usuarios: (Rol usuario -> rol que puede registrar)
        //admin -> admin, organizador, juez, jugador
        //organizador -> juez
        //juez -> ninguno
        //jugador -> ninguno
        //usuario no logeado -> jugador


        //Si usuario no está logeado, hacer autoregistro de JUGADOR:
        [HttpPost]
        [Route("/registro")]
        public ActionResult AutoRegistrarJugador(DatosRegistroDTO datosUsuarioARegistrar)
        {
            Console.WriteLine("POST /registro");

            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            
            if (authorizationHeaderValue != null && authorizationHeaderValue != "")
                throw new SinPermisoException($"Debe cerrar sesión para registrarse como jugador.");

            if (datosUsuarioARegistrar.rol != Roles.JUGADOR)
                throw new SinPermisoException("Solo se crean usuarios con rol JUGADOR en /registro");
       
            
            registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar);   
                
            return Ok(new { message = $"Usuario [{datosUsuarioARegistrar.email}] se autoregistró con éxito." });
        }


        //Si está logeado, registrar a otro usuario:
        [HttpPost]
        [Route("/crear")]
        [Authorize(Roles = $"{Roles.ADMIN},{Roles.ORGANIZADOR}")] 
        public ActionResult RegistrarUsuario(DatosRegistroDTO datosUsuarioARegistrar)
        {
            //validar permiso del usuario logeado (solo falta verificar que organizador crea juez, ya que admin puede crear todo)
            string rol_usuario_creador = User.FindFirst(ClaimTypes.Role).Value;

            if (
                rol_usuario_creador == Roles.ORGANIZADOR 
                &&
                datosUsuarioARegistrar.rol != Roles.JUEZ
            )
                throw new SinPermisoException($"Intentó crear un '{datosUsuarioARegistrar.rol}' pero no tiene permisos. Está logeado como Organizador y solo puede crear usuarios con rol JUEZ.")


            //registrar            
            string id_string = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(id_string, out int id_usuario_creador);

            registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar, id_usuario_creador);

            return Ok(new { message = $"Usuario [{datosUsuarioARegistrar.email}] se registró con éxito." });
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

            
            return Ok(new { message = "Sesión cerrada con éxito." });
        }


        //test authorize
        //[HttpGet]
        //[Route("/datos")]
        //[Authorize]
        //public ActionResult mostrarDatosUsuario()
        //{
        //    Console.WriteLine("GET /datos");

        //    return Ok(new {message="[autorización con éxito]"});
        //}


        //crear torneo (organizador)
        [HttpGet]
        [Route("/torneo")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public ActionResult crearTorneo()
        {
            //Console.WriteLine(User.Claims.ToString());


            return Ok(new {rol_del_usuario = User.FindFirst(ClaimTypes.Role).Value});
        }

    }
}
