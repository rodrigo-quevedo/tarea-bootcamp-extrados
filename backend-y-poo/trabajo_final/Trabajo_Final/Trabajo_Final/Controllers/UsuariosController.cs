using Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        // services
        LogearUsuarioService logearUsuarioService;


        public UsuariosController(IOptions<Primer_AdminConfiguration> options)
        {
            Console.WriteLine($"Adentro de Controller, options: {options.Value.Nombre_apellido}");
            Console.WriteLine($"Adentro de Controller, options: {options.Value.Email}");
        } 




       [HttpPost]
        [Route("/login")]

        public ActionResult LoginUser(CredencialesLoginDTO credenciales)
        {
        //validacion de inputs en el DTO con DataAnnotations

           //logear usuario service

           return Ok();


        } 
    }
}
