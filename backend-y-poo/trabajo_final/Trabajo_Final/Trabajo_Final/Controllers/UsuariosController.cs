using Configuration;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.Services.UsuarioServices.Login;
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
        
        private IJugadorAutoregistroService jugadorAutoregistroService;


        //private IRegistrarUsuarioService registrarUsuarioService;



        public UsuariosController(
            IVerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            
            ILogearUsuarioService login,
            ICrearJwtService jwt,

            IJugadorAutoregistroService autoregistro
            //IRegistrarUsuarioService registrar,
        )
        {
            logearUsuarioService = login;
            crearJwtService = jwt;

            jugadorAutoregistroService = autoregistro;
            //registrarUsuarioService = registrar;

        }




        [HttpPost]
        [Route("/login")]

        //validacion de inputs en el DTO con DataAnnotations
        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
            Console.WriteLine("POST /login");

            //Primero, verificar que no esté logeado
            if (Request.Cookies["jwt"] != null || Request.Cookies["refreshToken"] != null)
            {
                throw new SinPermisoException("Ya está logeado. Cierre su sesión actual para poder loguearse (ir a /logout).");
            }


            //logear usuario service
            Usuario usuarioVerificado = logearUsuarioService.LogearUsuario(credenciales);

            //crear jwt
            crearJwtService.CrearJwt(usuarioVerificado, Response.Cookies);
            //(no hace falta verificar, los metodos internos tiran Exceptions si fallan)

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
        public ActionResult RegistrarUser(DatosRegistroDTO datos)
        {
            Console.WriteLine("POST /registro");

            //verificar si usuario esta logeado
            string jwt;
            bool usuarioLogueado = Request.Cookies.TryGetValue("jwt", out jwt);

            //si no está logeado, hacer autoregistro de jugador
            if (!usuarioLogueado)
            {
                if (datos.rol != Roles.JUGADOR) { throw new SinPermisoException($"Se intentó registrar un [{datos.rol}], pero no esta logeado. Realize el login como admin u organizador e intente nuevamente. (O puede crear un usuario con rol [jugador] sin logearse. ADVERTENCIA: Solo se permite 1 rol por cuenta,es decir, por email)."); }

                Usuario usuarioRegistrado = jugadorAutoregistroService.AutoregistroJugador(datos);


                return Ok(new { message = $"Usuario '{usuarioRegistrado.Email}' registrado con éxito." });
            }


            //si está logeado, registrar a otro usuario
            //si admin, puede crear cualquier cosa

            //si organizador, chequear juez
            



            return Ok();


        }
    }
}
