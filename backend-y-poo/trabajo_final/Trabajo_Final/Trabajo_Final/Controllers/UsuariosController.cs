using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {

        [HttpPost]
        [Route("/login")]

        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
            //validacion de inputs en el DTO con DataAnnotations

            //verificar que email exista

            //validar password
            //(no se como hacer para hashear la password del primer admin, que en teoria va hardcodeado).
            //Capaz lo mejor es armar una funcion en Program.cs que haga eso
            return Ok();


        } 
    }
}
