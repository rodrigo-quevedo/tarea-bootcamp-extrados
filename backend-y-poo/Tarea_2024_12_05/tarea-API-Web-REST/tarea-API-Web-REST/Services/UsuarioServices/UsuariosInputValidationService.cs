using DAO_biblioteca_de_cases.Entidades;
using System.Text.RegularExpressions;
using tarea_API_Web_REST.Utils.Exceptions;
using tarea_API_Web_REST.Utils.RequestBodyParams;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class UsuariosInputValidationService
    {

        public void validarMail(string mail)
        {
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
                throw new InputValidationException($"\n{reqBody.username} es un username inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }



            // password
            string password_pattern = @"^[a-zA-ZñÑ0-9 ]{4,50}$";
            if (Regex.IsMatch(reqBody.password, password_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.password} es un password inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }

            // role
            if (reqBody.role != "usuario" && reqBody.role != "admin")
            {
                throw new InputValidationException($"El role '{reqBody.role}' es inválido. Solamente puede ser 'usuario' o 'admin'.");
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
                throw new InputValidationException($"\n{reqBody.password} es un password inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }
        }

        public void validarPrestamoObj(PrestamoLibro reqBody)
        {

            // username
            string username_pattern = @"^[a-zA-ZñÑ0-9 ]{4,50}$";
            if (Regex.IsMatch(reqBody.username_prestatario, username_pattern) == false)
            {
                throw new InputValidationException($"\n{reqBody.username_prestatario} es un nombre de usuario inválido. " +
                    $"\n1. Solo se permiten letras mayusculas, minusculas, numeros y espacios. " +
                    $"\n2. Debe tener un minimo de 4 y maximo de 50 caracteres." +
                    $"\n3. No se permiten caracteres especiales");
            }

            // idLibro
            if (reqBody.id < 1)
            {
                throw new InputValidationException($"'{reqBody.id}' es una Id de libro inválida. El valor mínimo es 1.");
            }

            // fechaHora_prestamo
            string fechaHora_prestamo_pattern = @"^[0-9]{4}-[0-9]{2}-[0-9]{2}T([01][0-9]|[2][0-3]):[0-5][0-9]Z$";
            if (Regex.IsMatch(reqBody.fechaHora_prestamo, fechaHora_prestamo_pattern) == false)
            {
                throw new InputValidationException($"El campo fechaHora_prestamo con valor '{reqBody.fechaHora_prestamo}' es incorrecto. " +
                    "\n1. Ingresar la fechaHora_prestamo como un string en el siguiente formato UTC: \"YYYY-MM-DDTHH:MMZ\". " +
                    "\n(Transformar a UTC la fechaHora ANTES de enviarla y respetar el formato.)" +
                    "\n(Si alguna parte tiene una sola cifra, poner un 0 delante. Por ejemplo, si se tiene el mes mayo (5) y las 3am (3), se debe usar 05 y 03: \"2024-05-20T03:30Z\". )"
            
                );
            }

            // ->Esto no importa, porque no se puede enviar DateTime por JSON.
            //fechaHora_prestamo: cuando no está en el request body, ASP.NET
            //lo inicializa con el valor "0001-01-01 00:00:00" (minValue).
            //Por lo tanto, acá tengo que validar que no sea NULL manualmente.
            //if (reqBody.fechaHora_prestamo == default)
            //{
            //    throw new InputValidationException("El campo 'fechaHora_prestamo' es obligatorio. " +
            //        "\n1. Ingresar la fechaHora en formato UTC 'YYYY-MM-DDTHH:MM:SSZ'. " +
            //        "\n(Transformar a UTC la fechaHora ANTES de enviarla)" +
            //        "\n2. Cualquier otro timezone va a ser tomado como UTC." +
            //        "\n(NO se verificará que el timezone sea UTC, así que es obligatorio enviarlo como UTC para que funcione correctamente.");
            //}




        }


    }
}
