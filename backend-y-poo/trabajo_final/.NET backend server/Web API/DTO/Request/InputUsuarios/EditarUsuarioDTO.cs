using Constantes.Constantes;
using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.InputUsuarios
{
    public class EditarUsuarioDTO
    {
        [RegularExpression(@"^[a-zA-Z ]{5,60}$", ErrorMessage = "Campo 'nombre_apellido' solo permite letras y espacio. Entre 5 y 60 caracteres. ")]
        public string? Nombre_apellido { get; set; }


        public string? Password { get; set; }

        [RegularExpression(@"^[a-zA-Z +-:]{2,30}$", ErrorMessage = "Campo 'pais' solo permite letras, numeros, espacio ( ), +, -, y los dos puntos (:). Entre 2 y 30 caracteres. ")]
        public string? Pais { get; set; }

        //public IFormFile? Foto { get; set; }
        [Url]
        public string? Foto { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{5,25}$",
          ErrorMessage = "Campo 'alias' solo permite letras y numeros. Entre 5 y 25 caracteres. ")]
        public string? Alias { get; set; }


        //Esto no se si conviene que se pueda editar:
        //[RegularExpression(@$"^{Roles.ADMIN}|{Roles.ORGANIZADOR}|{Roles.JUEZ}|{Roles.JUGADOR}$", ErrorMessage = $"Rol invalido. Roles permitidos: {Roles.ADMIN} | {Roles.ORGANIZADOR} | {Roles.JUEZ} | {Roles.JUGADOR}")]
        //public string? Rol { get; set; }
        //[EmailAddress]
        //public string? Email { get; set; }
    }
}
