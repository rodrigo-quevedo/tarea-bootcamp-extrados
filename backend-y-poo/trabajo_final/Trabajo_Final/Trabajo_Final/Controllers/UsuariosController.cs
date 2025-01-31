using Configuration;
using DAO.DAOs.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices;
using Trabajo_Final.utils.Verificar_Existencia_Admin;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        // services
        LogearUsuarioService logearUsuarioService;



        public UsuariosController(
            IVerificarExistenciaAdmin verificarAdmin //Cuando se crea el controller, se hace una verificación automática.
        )
        {


        } 




       [HttpPost]
        [Route("/login")]

        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
            //validacion de inputs en el DTO con DataAnnotations

            //logear usuario service
            Console.WriteLine("POST /login");


           return Ok();


        } 
    }
}
