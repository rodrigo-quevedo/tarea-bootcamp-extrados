using Configuration;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices.Login;
using Trabajo_Final.utils.Verificar_Existencia_Admin;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        // services
        ILogearUsuarioService logearUsuarioService;



        public UsuariosController(
            IVerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            ILogearUsuarioService login
        )
        {
            logearUsuarioService = login;

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
    }
}
