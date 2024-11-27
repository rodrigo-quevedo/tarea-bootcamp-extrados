using DAO_biblioteca_de_cases.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tarea_API_Web_REST.Services;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        //services
        BuscarUsuarioByMailService buscarUsuarioByMailService;
        CrearUsuarioService crearUsuarioService;
        ActualizarUsuarioService actualizarUsuarioService;

        UsuariosInputValidationService usuariosInputValidationService;

        public UsuariosController() {
            buscarUsuarioByMailService = new ();
            crearUsuarioService = new ();
            actualizarUsuarioService = new ();
            
            usuariosInputValidationService = new ();
        }



        [HttpGet]
        public ActionResult<Usuario> BuscarUsuarioPorMail(string mail)
        {
            Console.WriteLine($"GET en /usuarios: {DateTime.Now}");

            try
            {
                //validacion de input
                usuariosInputValidationService.validarMail(mail);

                //buscar usuario
                return buscarUsuarioByMailService.BuscarUsuarioByMail(mail);

            }
            catch(InputValidationException inputEx)
            {
                Console.WriteLine(inputEx.Message);
                return BadRequest(inputEx.Message);
            }
            catch (Exception ex)
            {
            //si no es error de input, seguro es error del servidor
                Console.WriteLine(ex.Message);
                return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { message = ex.Message }
                );
            }

        }


        [HttpPost]
        public ActionResult<Usuario> AgregarUsuario(Usuario reqBody)//en el caso del request body, hay que leerlo como un objeto
        {
            Console.WriteLine($"POST en /usuarios: {DateTime.Now}");


            try
            {
                //validacion de input
                usuariosInputValidationService.validarUsuarioObj(reqBody);

                //crear usuario (el service crea el usuario, y luego lo busca y devuelve)
                return crearUsuarioService.CrearUsuario(reqBody);
                

            }
            catch (InputValidationException inputEx)
            {
                Console.WriteLine(inputEx.Message);
                return BadRequest(inputEx.Message);
            }
            catch (Exception ex)
            {
                //si no es error de input, seguro es error del servidor
                Console.WriteLine(ex.Message);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = ex.Message }
                );
            }
        }


        [HttpPut]
        public ActionResult<Usuario> ActualizarUsuario(Usuario reqBody)
        {
            Console.WriteLine($"PUT en /usuarios: {DateTime.Now}");


            try
            {
                //validacion de input
                usuariosInputValidationService.validarUsuarioObj(reqBody);

                //actualizar usuario (el service actualiza el usuario, luego lo busca y lo devuelve)
                return actualizarUsuarioService.ActualizarUsuario(reqBody);
                
            }
            catch (InputValidationException inputEx)
            {
                Console.WriteLine(inputEx.Message);
                return BadRequest(inputEx.Message);
            }
            catch (Exception ex)
            {
                //si no es error de input, seguro es error del servidor
                Console.WriteLine(ex.Message);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = ex.Message }
                );
            }
        }
    }
}
