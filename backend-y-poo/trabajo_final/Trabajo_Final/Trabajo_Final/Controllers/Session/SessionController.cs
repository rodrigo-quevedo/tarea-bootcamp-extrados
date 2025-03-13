using DAO.Entidades.UsuarioEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.Services.UsuarioServices.Login;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.AsignarRefreshToken;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Crear;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Desactivar;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Validar;
using Custom_Exceptions.Exceptions.Exceptions;
using Trabajo_Final.utils.Verificar_Existencia_Admin;
using Trabajo_Final.DTO.Request.InputLogin;
using Configuration.Jwt;

namespace Trabajo_Final.Controllers.Session
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        // services
        private readonly IJwtConfiguration jwtConfiguration;
        private readonly ICrearJwtService crearJwtService;
        private readonly ILogearUsuarioService logearUsuarioService;
        private readonly IActualizarJWTService actualizarJWTService;
        private readonly IAsignarRefreshTokenService asignarRefreshTokenService;
        private readonly ICrearRefreshTokenService crearRefreshTokenService;
        private readonly IDesactivarRefreshTokenService desactivarRefreshTokenService;


        public SessionController(
            VerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática. Está en UsuariosController porque sí o sí los usuarios se deben loguear/hacer refresh.

            IJwtConfiguration jwtConfiguration,
            ICrearJwtService crearJwtService,
            ILogearUsuarioService logearUsuarioService,
            IActualizarJWTService actualizarJWTService,
            IAsignarRefreshTokenService asignarRefreshTokenService,
            ICrearRefreshTokenService crearRefreshTokenService,
            IDesactivarRefreshTokenService desactivarRefreshTokenService
        )
        {
            this.jwtConfiguration = jwtConfiguration;
            this.crearJwtService = crearJwtService;
            this.logearUsuarioService = logearUsuarioService;
            this.actualizarJWTService = actualizarJWTService;
            this.asignarRefreshTokenService = asignarRefreshTokenService;
            this.crearRefreshTokenService = crearRefreshTokenService;
            this.desactivarRefreshTokenService = desactivarRefreshTokenService;
        }



        //Endpoints para manejo de sesiones:

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> LoginUser(CredencialesLoginDTO credenciales)
        {
            Console.WriteLine("POST /login");

            //Verificar que no esté logeado
            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();

            if (authorizationHeaderValue != null && authorizationHeaderValue != "")
                throw new AlreadyLoggedInException("Ya está logeado. Cierre su sesión actual para poder loguearse (ir a /logout).");

            //Verificar credenciales
            Usuario usuarioVerificado = await logearUsuarioService.LogearUsuario(credenciales);

            //Si actualmente el usuario tiene un refreshToken, se intentará desactivarlo en DB
            //(si falla sigue de largo)
            string refreshTokenExistente = Request.Cookies["refreshToken"];
            desactivarRefreshTokenService.DesactivarRefreshToken(usuarioVerificado.Id, refreshTokenExistente);

            //Crear jwt y refresh token
            string jwt = crearJwtService.CrearJwt(usuarioVerificado);
            string refreshToken = crearRefreshTokenService.CrearRefreshToken(usuarioVerificado);

            //Asignar refresh token en db
            await asignarRefreshTokenService.AsignarRefreshToken(usuarioVerificado, refreshToken);


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

            return Ok(new
            {
                message = $"Usuario {usuarioVerificado.Email} logeado con éxito.",
                jwt
            });

        }


        [HttpGet]
        [Route("/refresh")]
        public async Task<ActionResult> RefreshToken()
        {
            string refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null || refreshToken == "") throw new SinPermisoException("No puede actualizar el token porque no está logeado.");

            string jwtActualizado = await actualizarJWTService.ActualizarJWT(refreshToken);


            return Ok(new
            {
                message = "Se actualizó el token correctamente.",
                jwt = jwtActualizado
            });
        }


        [HttpGet]
        [Route("/logout")]
        [Authorize]
        public async Task<ActionResult> LogoutUser()
        {
            //Desactivar refreshToken en db (borrado logico):
            string id_usuario_string = User.FindFirst(ClaimTypes.Sid).Value;
            int.TryParse(id_usuario_string, out int id_usuario);

            string refreshToken = Request.Cookies["refreshToken"];

            bool sesion_cerrada = await desactivarRefreshTokenService.DesactivarRefreshToken(id_usuario, refreshToken);
            if (!sesion_cerrada) throw new DefaultException("No se pudo cerrar la sesión.");

            //Server response:

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now
            });


            return Ok(new
            {
                message = "Sesión cerrada con éxito.",
                deleteJWT = true
            });
        }

    }
}
