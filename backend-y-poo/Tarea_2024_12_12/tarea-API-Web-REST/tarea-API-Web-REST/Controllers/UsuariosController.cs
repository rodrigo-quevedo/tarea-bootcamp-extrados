using Configuration;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tarea_API_Web_REST.Services.Authorization;
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
        AuthorizeCookieService authorizeCookieService;

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
            crearJwtService = new (_jwtConfiguration);
            usuariosInputValidationService = new ();
            buscarLibroPorIdService = new (_databaseConfiguration.connection_string);
            verificarDisponibilidadService = new (_databaseConfiguration.connection_string);
            prestarLibroService = new (_databaseConfiguration.connection_string);
            verificarPrestatarioService = new (_jwtConfiguration);
            crearRefreshTokenService = new (_jwtConfiguration, _databaseConfiguration.connection_string);
            authorizeCookieService = new(_jwtConfiguration, _databaseConfiguration.connection_string);

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
                string jwt = crearJwtService.CrearJwt(usuarioValidado, Response.Cookies);

                //crear refresh token
                string refreshToken = crearRefreshTokenService.CrearRefreshToken(usuarioValidado, Response.Cookies);

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
                //jwt y refreshToken cookie validation
                string jwtUsername = authorizeCookieService.AuthorizeJWT(Request.Cookies, Response.Cookies);
                authorizeCookieService.AuthorizeRoles(new string[] {"usuario"}, Request.Cookies);

                // input validation:
                usuariosInputValidationService.validarPrestamoObj(prestamo);

                // comprobar que el id del libro y el username existen:
                Usuario usuarioEncontrado = buscarUsuarioByUsernameService.BuscarUsuario(prestamo.username_prestatario);
                Libro libroEncontrado = buscarLibroPorIdService.BuscarLibro(prestamo.id);

                // comprobar que el libro no está prestado:
                Libro libroDisponible = verificarDisponibilidadService.Verificar(libroEncontrado);

                // comparar que el prestatario y el usuario logueado sean el mismo:
                verificarPrestatarioService.Verificar(prestamo.username_prestatario, jwtUsername);

                // parsear date (el string prestamo.fechaHora_prestamo ya fue validado en validarPrestamoObj() ):
                DateTime fechaHora_prestamo = DateTime.Parse(prestamo.fechaHora_prestamo);

                // prestar libro:
                prestarLibroService.PrestarLibro(prestamo, fechaHora_prestamo);

                // buscar libro prestado:
                Libro libroPrestado = buscarLibroPorIdService.BuscarLibro(prestamo.id);

                // exito:
                return Ok(libroPrestado);

            }
            catch (InvalidRefreshTokenException invalidRefreshTokenEx) { return exHandler.InvalidRefreshTokenExceptionHandler(invalidRefreshTokenEx); }
            catch (SinPermisoException sinPermisoEx) { return exHandler.SinPermisoExceptionHandler(sinPermisoEx); }
            catch (InputValidationException inputEx) { return exHandler.InputValidationExceptionHandler(inputEx); }
            catch (NotFoundException notFoundEx) { return exHandler.NotFoundExceptionHandler(notFoundEx); }
            catch (Exception ex) { return exHandler.DefaultExceptionHandler(ex); }

        }

        [HttpGet("logout")]

        public ActionResult LogoutUsuario()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now
            });

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now
            });

            return Ok(new
            {
                message = "Usuario logged out."
            });
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
