using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using tarea_API_Web_REST.Services;

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
        public Usuario BuscarUsuarioPorMail(string mail)
        {
            Console.WriteLine($"GET en /usuarios: {DateTime.Now}");

            try
            {
                //validacion de input
                usuariosInputValidationService.validarMail(mail);

                //buscar usuario
                return buscarUsuarioByMailService.BuscarUsuarioByMail(mail);

            }
            catch (Exception ex)
            {
                //En teoría acá debería responder con un error 400 o 500, pero no se como se hace
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        [HttpPost]
        public Usuario AgregarUsuario(Usuario reqBody)//en el caso del request body, hay que leerlo como un objeto
        {
            Console.WriteLine($"POST en /usuarios: {DateTime.Now}");


            try
            {
                //validacion de input
                usuariosInputValidationService.validarUsuarioObj(reqBody);

                //crear usuario (el service crea el usuario, y luego lo busca y devuelve)
                return crearUsuarioService.CrearUsuario(reqBody.mail, reqBody.nombre, reqBody.edad);
                

            }
            catch (Exception ex)
            {
                //Aca el servidor debería responder con un error 400 o 500
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        [HttpPut]
        public Usuario ActualizarUsuario(Usuario reqBody)
        {
            Console.WriteLine($"PUT en /usuarios: {DateTime.Now}");


            try
            {
                //validacion de input
                usuariosInputValidationService.validarUsuarioObj(reqBody);

                //actualizar usuario (el service actualiza el usuario, luego lo busca y lo devuelve)
                return actualizarUsuarioService.ActualizarUsuario(reqBody.mail, reqBody.nombre, reqBody.edad);
                
            }
            catch (Exception ex)
            {
                //Aca el servidor debería responder con un error 400 o 500
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
