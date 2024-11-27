using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace tarea_API_Web_REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        //usuario DAO
        UsuarioDAO usuarioDAO = new UsuarioDAO();


        [HttpGet]
        public Usuario BuscarUsuarioPorMail(string mail)
        {
            Console.WriteLine($"GET en /usuarios: {DateTime.Now}");

            try
            {
                //validacion de input
                string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
                if (Regex.IsMatch(mail, mail_pattern) == false)
                {
                    throw new Exception($"\n{mail} es un email inválido. " +
                        $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                        $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                        $"\n3. Solo se permiten cuentas @gmail.com");
                }

                //DAO
                Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(mail);

                if (usuarioEncontrado != null) usuarioEncontrado.mostrarDatos();
                else Console.WriteLine("usuarioEncontrado is null");

                return usuarioEncontrado;

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
                if (reqBody.edad <= 14)
                {
                    throw new Exception($"{reqBody.edad} es una Edad de usuario inválida. " +
                        $"\n1. Debe ser mayor a 14.");
                }

                string nombre_pattern = @"^[a-zA-ZñÑ ]{1,50}$";
                if (Regex.IsMatch(reqBody.nombre, nombre_pattern) == false)
                {
                    throw new Exception($"\n{reqBody.nombre} es un nombre de usuario inválido. " +
                        $"\n1. Solo se permiten letras mayusculas, minusculas y espacio. " +
                        $"\n2. Debe tener un minimo de 1 y maximo de 50 caracteres.");
                }

                string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
                if (Regex.IsMatch(reqBody.mail, mail_pattern) == false)
                {
                    throw new Exception($"\n{reqBody.mail} es un email inválido. " +
                        $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                        $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                        $"\n3. Solo se permiten cuentas @gmail.com");
                }

                //DAO
                Usuario usuarioCreado = usuarioDAO.CrearUsuario(reqBody.mail, reqBody.nombre, reqBody.edad);

                if (usuarioCreado != null) usuarioCreado.mostrarDatos();
                else Console.WriteLine("usuarioCreado es null");

                return usuarioCreado;

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
                if (reqBody.edad <= 14)
                {
                    throw new Exception($"{reqBody.edad} es una Edad de usuario inválida. " +
                        $"\n1. Debe ser mayor a 14.");
                }

                string nombre_pattern = @"^[a-zA-ZñÑ ]{1,50}$";
                if (Regex.IsMatch(reqBody.nombre, nombre_pattern) == false)
                {
                    throw new Exception($"\n{reqBody.nombre} es un nombre de usuario inválido. " +
                        $"\n1. Solo se permiten letras mayusculas, minusculas y espacio. " +
                        $"\n2. Debe tener un minimo de 1 y maximo de 50 caracteres.");
                }

                string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
                if (Regex.IsMatch(reqBody.mail, mail_pattern) == false)
                {
                    throw new Exception($"\n{reqBody.mail} es un email inválido. " +
                        $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                        $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                        $"\n3. Solo se permiten cuentas @gmail.com");
                }

                //DAO
                Usuario usuarioActualizado = usuarioDAO.ActualizarUsuario(reqBody.mail, reqBody.nombre, reqBody.edad);

                if (usuarioActualizado != null) usuarioActualizado.mostrarDatos();
                else Console.WriteLine("usuarioActualizado es null");

                return usuarioActualizado;

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
