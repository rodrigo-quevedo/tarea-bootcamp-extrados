using Configuration;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trabajo_Final.DTO;
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
        private IJugadorAutoregistroService jugadorAutoregistroService;
        //private IRegistrarUsuarioService registrarUsuarioService;



        public UsuariosController(
            IVerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            
            ILogearUsuarioService login,
            IJugadorAutoregistroService autoregistro
            //IRegistrarUsuarioService registrar,
        )
        {
            logearUsuarioService = login;
            jugadorAutoregistroService = autoregistro;
            //registrarUsuarioService = registrar;

        } 




        [HttpPost]
        [Route("/login")]

        //validacion de inputs en el DTO con DataAnnotations
        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
            Console.WriteLine("POST /login");

            //logear usuario service
            Usuario usuarioLogeado = logearUsuarioService.LogearUsuario(credenciales);



           return Ok();


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
                if (datos.rol != Roles.JUGADOR) { throw new SinPermisoException($"Intentó registrar un {datos.rol}, pero no esta logeado. Debe logearse como admin u organizador para registrar usuarios."); }

                Usuario usuarioRegistrado = jugadorAutoregistroService.AutoregistroJugador(datos);


                return Ok(new { message = $"Usuario '{usuarioRegistrado.Email}' registrado con éxito." });
            }


            //si está logeado, registrar otro usuario
            
            



            return Ok();


        }
    }
}
