using DAO_biblioteca_de_cases.Entidades;
using System.Text.RegularExpressions;

namespace tarea_API_Web_REST.Services
{
    public class UsuariosInputValidationService
    {

        public void validarMail(string mail) {
            string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
            if (Regex.IsMatch(mail, mail_pattern) == false)
            {
                throw new Exception($"\n{mail} es un email inválido. " +
                    $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                    $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                    $"\n3. Solo se permiten cuentas @gmail.com");
            }
        }

        public void validarUsuarioObj(Usuario reqBody)
        {
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
        }
    }
}
