using Configuration;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tarea_API_Web_REST.Services.LibroServices;
using tarea_API_Web_REST.Services.UsuarioServices;
using tarea_API_Web_REST.Utils.ExceptionHandler;
using tarea_API_Web_REST.Utils.Exceptions;
using tarea_API_Web_REST.Utils.RequestBodyParams;

namespace tarea_API_Web_REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        //services
        BuscarUsuarioByMailService buscarUsuarioByMailService;
        BuscarUsuarioByUsernameService buscarUsuarioByUsernameService;
        CrearUsuarioService crearUsuarioService;
        ActualizarUsuarioService actualizarUsuarioService;
        ValidarUsuarioService logearUsuarioService;
        CrearJwtService crearJwtService;
        UsuariosInputValidationService usuariosInputValidationService;
        BuscarLibroPorIdService buscarLibroPorIdService;
        VerificarDisponibilidadService verificarDisponibilidadService;
        PrestarLibroService prestarLibroService;
        VerificarPrestatarioService verificarPrestatarioService;
        CrearRefreshTokenService crearRefreshTokenService;

        ExceptionHandler exHandler;

        private JwtConfiguration _jwtConfiguration;
        private DatabaseConfiguration _databaseConfiguration;

        public UsuariosController(IOptions<JwtConfiguration> jwtConfig, IOptions<DatabaseConfiguration> dbConfig) {
            this._jwtConfiguration = jwtConfig.Value;
            this._databaseConfiguration = dbConfig.Value;

            buscarUsuarioByMailService = new (_databaseConfiguration.connection_string);
            buscarUsuarioByUsernameService = new (_databaseConfiguration.connection_string);
            crearUsuarioService = new (_databaseConfiguration.connection_string);
            actualizarUsuarioService = new (_databaseConfiguration.connection_string);
            logearUsuarioService = new (_databaseConfiguration.connection_string);
            crearJwtService = new ();
            usuariosInputValidationService = new ();
            buscarLibroPorIdService = new (_databaseConfiguration.connection_string);
            verificarDisponibilidadService = new (_databaseConfiguration.connection_string);
            prestarLibroService = new (_databaseConfiguration.connection_string);
            verificarPrestatarioService = new (_jwtConfiguration);
            crearRefreshTokenService = new ();

            exHandler = new (this);

        }

        //endpoints

        [HttpGet] //http://localhost:5176/usuarios?mail=****@gmail.com
        [Authorize]
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


        [HttpPost("registro")] //http://localhost:5176/usuarios/registro
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

        [HttpPost("login")] //http://localhost:5176/usuarios/login
        public ActionResult LogearUsuario(Credenciales reqBody)//en el caso del request body, hay que leerlo como un objeto
        {
            try
            {
                //validar inputs
                usuariosInputValidationService.validarCredencialesObj(reqBody);

                //logear usuario (corroborar credenciales)
                Usuario usuarioValidado = logearUsuarioService.ValidarUsuario(reqBody);

                //crear JWT
                string jwt = crearJwtService.CrearJwt(usuarioValidado, _jwtConfiguration);

                //crear refresh token
                string refreshToken = crearRefreshTokenService.CrearRefreshToken(usuarioValidado, _jwtConfiguration);

                //devolver una cookie con jwt y otra con refreshToken
                Response.Cookies.Append("jwt", jwt, new CookieOptions { 
                        HttpOnly = false, 
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires = DateTime.Now.AddMinutes(5)
                    }
                );
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { 
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Expires= DateTime.Now.AddDays(120)
                    }
                );


                return Ok(
                    new { 
                        message = $"Usuario '{usuarioValidado.username}' logueado con éxito." 
                    }
                );

            }
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }

            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }

            catch (InvalidCredentialsException invalidCredsEx) { return exHandler.InvalidCredentialsExceptionHandler(invalidCredsEx); }

            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }
        }


        [HttpPost("prestamo_libro")]
        //[Authorize(Roles = "usuario")]
        

        public ActionResult<Libro> PrestarLibro(PrestamoLibro prestamo)
        {
            try
            {
                // input validation:
                usuariosInputValidationService.validarPrestamoObj(prestamo);

                // comprobar que el id del libro y el username existen:
                Usuario usuarioEncontrado = buscarUsuarioByUsernameService.BuscarUsuario(prestamo.username_prestatario);
                Libro libroEncontrado = buscarLibroPorIdService.BuscarLibro(prestamo.id);

                // comprobar que el libro no está prestado:
                //Libro libroDisponible = verificarDisponibilidadService.Verificar(libroEncontrado);

                // comparar que el prestatario y el usuario logueado sean el mismo:
                string jwt = Request.Cookies["jwt"];
                verificarPrestatarioService.Verificar(prestamo.username_prestatario, jwt);

                // parsear date (el string prestamo.fechaHora_prestamo ya fue validado en validarPrestamoObj() ):
                DateTime fechaHora_prestamo = DateTime.Parse(prestamo.fechaHora_prestamo);

                // prestar libro:
                prestarLibroService.PrestarLibro(prestamo, fechaHora_prestamo);

                // buscar libro prestado:
                Libro libroPrestado = buscarLibroPorIdService.BuscarLibro(prestamo.id);

                // exito:
                return Ok(libroPrestado);

            }
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }
            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }
            catch (SinPermisoException sinPermisoEx) { return exHandler.SinPermisoExceptionHandler(sinPermisoEx); }
            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }


        }


        //[HttpPut]
        //public ActionResult<Usuario> ActualizarUsuario(Usuario reqBody)
        //{
        //    Console.WriteLine($"PUT en /usuarios: {DateTime.Now}");


        //    try
        //    {
        //        //validacion de input
        //        usuariosInputValidationService.validarUsuarioObj(reqBody);

        //        //actualizar usuario (el service actualiza el usuario, luego lo busca y lo devuelve)
        //        return actualizarUsuarioService.ActualizarUsuario(reqBody);
                
        //    }
        //    catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }

        //    catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }

        //    catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }
        //}


    }
}
