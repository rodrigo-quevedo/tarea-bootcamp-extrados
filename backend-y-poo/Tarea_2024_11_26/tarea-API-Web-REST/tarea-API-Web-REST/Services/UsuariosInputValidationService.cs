using DAO_biblioteca_de_cases.Entidades;
using System.Text.RegularExpressions;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services
{
    public class UsuariosInputValidationService
    {

        public void validarMail(string mail) {
            string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
            if (Regex.IsMatch(mail, mail_pattern) == false)
            {
                throw new InputValidationException($"\n{mail} es un email inválido. " +
                    $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                    $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                    $"\n3. Solo se permiten cuentas @gmail.com");
            }
        }

        public void validarUsuarioObj(Usuario reqBody)
        {

            // edad
            if (reqBody.edad <= 14)
            {
                throw new InputValidationException($"{reqBody.edad} es una Edad de usuario inválida. " +
                    $"\n1. Debe ser mayor a 14.");
            }

            // nombre
            string nombre_pattern = @"^[a-zA-ZñÑ ]{1,50}$";
            if (Regex.IsMatch(reqBody.nombre, nombre_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.nombre} es un nombre de usuario inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas y espacio. " +
                    $"\n2. Debe tener un minimo de 1 y maximo de 50 caracteres.");
            }

            // mail
            string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
            if (Regex.IsMatch(reqBody.mail, mail_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.mail} es un email inválido. " +
                    $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                    $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                    $"\n3. Solo se permiten cuentas @gmail.com");
            }

            // username
            string username_pattern = @"^[a-zA-ZñÑ0-9 ]{4,50}$";
            if (Regex.IsMatch(reqBody.username, username_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.username} es un nombre de usuario inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }



            // password
            string password_pattern = @"^[a-zA-ZñÑ0-9 ]{4,50}$";
            if (Regex.IsMatch(reqBody.password, password_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.password} es un nombre de usuario inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }



        }

        public void validarCredencialesObj(Credenciales reqBody)
        {
            // mail
            string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
            if (Regex.IsMatch(reqBody.mail, mail_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.mail} es un email inválido. " +
                    $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                    $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                    $"\n3. Solo se permiten cuentas @gmail.com");
            }

            // username
            string username_pattern = @"^[a-zA-ZñÑ0-9 ]{4,50}$";
            if (Regex.IsMatch(reqBody.username, username_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.username} es un nombre de usuario inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }

            // password
            string password_pattern = @"^[a-zA-ZñÑ0-9 ]{4,50}$";
            if (Regex.IsMatch(reqBody.password, password_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.password} es un nombre de usuario inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }
        }
    }
}
