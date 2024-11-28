using DAO_biblioteca_de_cases.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tarea_API_Web_REST.Services;
using tarea_API_Web_REST.Utils.ExceptionHandler;
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

        LogearUsuarioService logearUsuarioService;

        ExceptionHandler exHandler;

        public UsuariosController() {
            buscarUsuarioByMailService = new ();
            crearUsuarioService = new ();
            actualizarUsuarioService = new ();
            
            usuariosInputValidationService = new ();

            exHandler = new (this);

            logearUsuarioService = new ();
        }


        //manejo de metodos HTTP

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
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }
            
            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }
            
            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }
            
        }


        [HttpPost("registro")]
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
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }

            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }

            catch (AlreadyExistsException alreadyExistsEx) { return exHandler.AlreadyExistsExceptionHandler(alreadyExistsEx); }

            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }
        }

        [HttpPost("login")]
        public ActionResult<Usuario> LogearUsuario(Credenciales reqBody)//en el caso del request body, hay que leerlo como un objeto
        {
            try
            {
                //validar inputs
                usuariosInputValidationService.validarCredencialesObj(reqBody);

                //logear usuario
                return logearUsuarioService.LogearUsuario(reqBody);
            }
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }

            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }

            catch (InvalidCredentialsException invalidCredsEx) { return exHandler.InvalidCredentialsExceptionHandler(invalidCredsEx); }

            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }
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
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }

            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }

            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }
        }
    }
}
