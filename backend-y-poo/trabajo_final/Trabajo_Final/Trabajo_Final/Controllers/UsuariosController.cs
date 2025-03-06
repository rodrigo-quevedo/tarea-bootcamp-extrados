using Configuration.DI;
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
using Trabajo_Final.Services.UsuarioServices.Registro;
using Custom_Exceptions.Exceptions.Exceptions;
using Trabajo_Final.utils.Verificar_Existencia_Admin;
using Trabajo_Final.Services.UsuarioServices.Perfil;
using Trabajo_Final.Services.UsuarioServices.Eliminar;
using Trabajo_Final.Services.UsuarioServices.Editar;
using Trabajo_Final.Services.UsuarioServices.Buscar;
using Constantes.Constantes;
using Trabajo_Final.DTO.Request.InputUsuarios;
using Trabajo_Final.DTO.Request.InputLogin;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        // services
        private IJwtConfiguration jwtConfiguration;

        private ICrearJwtService crearJwtService;

        private ILogearUsuarioService logearUsuarioService;

        private IActualizarJWTService actualizarJWTService;
        private IAsignarRefreshTokenService asignarRefreshTokenService;
        private ICrearRefreshTokenService crearRefreshTokenService;
        private IDesactivarRefreshTokenService desactivarRefreshTokenService;


        private IRegistroUsuarioService registroUsuarioService;

        private IActualizarPerfilService actualizarPerfilService;

        private IEliminarUsuarioService eliminarUsuarioService;
        private IEditarUsuarioService editarUsuarioService;

        private IBuscarUsuarioService buscarUsuarioService;

        public UsuariosController(
            VerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.

            IJwtConfiguration jwtConfig,

            ICrearJwtService jwt,

            ILogearUsuarioService login,

            IActualizarJWTService actualizarJWT,
            IAsignarRefreshTokenService asignarRefreshToken,
            ICrearRefreshTokenService refreshToken,
            IDesactivarRefreshTokenService desactivarRefreshToken,

            IRegistroUsuarioService registro,

            IActualizarPerfilService actualizarPerfil,

            IEliminarUsuarioService eliminarUsuario,
            IEditarUsuarioService editarUsuario,

            IBuscarUsuarioService buscarUsuario
        )
        {
            jwtConfiguration = jwtConfig;

            crearJwtService = jwt;

            logearUsuarioService = login;

            actualizarJWTService = actualizarJWT;
            asignarRefreshTokenService = asignarRefreshToken;
            crearRefreshTokenService = refreshToken;
            desactivarRefreshTokenService = desactivarRefreshToken;

            registroUsuarioService = registro;

            actualizarPerfilService = actualizarPerfil;

            eliminarUsuarioService = eliminarUsuario;
            editarUsuarioService = editarUsuario;

            buscarUsuarioService = buscarUsuario;
        }




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

            return Ok(new {
                message = $"Usuario {usuarioVerificado.Email} logeado con éxito.",
                jwt = jwt
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



        //Consideraciones al registrar usuarios: (Rol usuario -> rol que puede registrar)
        //admin -> admin, organizador, juez, jugador
        //organizador -> juez
        //juez -> ninguno
        //jugador -> ninguno
        //usuario no logeado -> jugador


        //Si usuario no está logeado, hacer autoregistro de JUGADOR:
        [HttpPost]
        [Route("/registro")]
        public async Task<ActionResult> AutoRegistrarJugador(DatosRegistroDTO datosUsuarioARegistrar)
        {
            Console.WriteLine("POST /registro");

            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();

            if (authorizationHeaderValue != null && authorizationHeaderValue != "")
                throw new SinPermisoException($"Debe cerrar sesión para registrarse como jugador.");

            if (datosUsuarioARegistrar.rol != Roles.JUGADOR)
                throw new SinPermisoException("Solo se crean usuarios con rol JUGADOR en /registro");


            await registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar, null);

            return Ok(new { message = $"Usuario [{datosUsuarioARegistrar.email}] se autoregistró con éxito." });
        }


        //Si está logeado, registrar a otro usuario:
        [HttpPost]
        [Route("/crear")]
        [Authorize(Roles = $"{Roles.ADMIN},{Roles.ORGANIZADOR}")]
        public async Task<ActionResult> RegistrarUsuario(DatosRegistroDTO datosUsuarioARegistrar)
        {
            //verificar que organizador crea juez, ya que admin puede crear todo
            string rol_usuario_creador = User.FindFirst(ClaimTypes.Role).Value;

            if (rol_usuario_creador == Roles.ORGANIZADOR
                &&
                datosUsuarioARegistrar.rol != Roles.JUEZ
            )
                throw new SinPermisoException($"Intentó crear un '{datosUsuarioARegistrar.rol}' pero no tiene permisos. Está logeado como Organizador y solo puede crear usuarios con rol JUEZ.");


            //registrar            
            string id_string = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(id_string, out int id_usuario_creador);

            await registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar, id_usuario_creador);

            return Ok(new { message = $"Usuario [{datosUsuarioARegistrar.email}] se registró con éxito." });
        }



        [HttpGet]
        [Route("/logout")]
        [Authorize]
        public async Task<ActionResult> LogoutUser()
        {
            //Desactivar refreshToken en db (borrado logico):
            string id_usuario_string = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(id_usuario_string, out int id_usuario);

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


            return Ok(new {
                message = "Sesión cerrada con éxito.",
                deleteJWT = true
            });
        }


        [HttpPut]
        [Route("/perfil")]
        [Authorize(Roles = $"{Roles.JUGADOR},{Roles.JUEZ}")]
        public async Task<ActionResult> ActualizarPerfil(ActualizarPerfilDTO dto)
        {
            //id usuario
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_usuario);

            //manejo de string vacio "", ya que data annotation [RegEx] lo deja pasar como valido:
            if (dto.alias == "") throw new InvalidInputException("Alias incorrecto. Caracteres válidos: letras, números. Entre 4 y 25 caracteres.");

            string response = await actualizarPerfilService.ActualizarPerfil(
                id_usuario, dto.url_foto, dto.alias);

            return Ok(new { message = response });
        }

        [HttpDelete]
        [Route("/usuarios/{id_usuario}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> EliminarUsuario([FromRoute] int id_usuario)
        {
            await eliminarUsuarioService.EliminarUsuario(id_usuario);

            return Ok(new { message = $"Se eliminó al usuario [{id_usuario}] con éxito." });
        }

        [HttpPut]
        [Route("/usuarios/{id_usuario}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> EditarUsuario([FromRoute] int id_usuario, EditarUsuarioDTO dto)
        {
            await editarUsuarioService.EditarUsuario(id_usuario, dto);

            return Ok(new { message = $"Se editó al usuario [{id_usuario}] con éxito." });
        }

        [HttpGet]
        [Route("/usuarios/{id_usuario}")]
        [Authorize]
        public async Task<ActionResult> BuscarDatosUsuario([FromRoute] int id_usuario)
        {
            //datos usurio logueado (necesarios para saber sus permisos)
            string rol_logueado = User.FindFirst(ClaimTypes.Role).Value;
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_logeado);


            if (rol_logueado == Roles.ADMIN || rol_logueado == Roles.ORGANIZADOR)
                return Ok(new {
                    usuario = await buscarUsuarioService.BuscarDatosCompletosUsuario(
                        id_logeado, rol_logueado, id_usuario)
                });


            else return Ok(new {
                usuario = await buscarUsuarioService.BuscarPerfilUsuario(
                    id_logeado, rol_logueado, id_usuario)
            });

          }


    }
}
