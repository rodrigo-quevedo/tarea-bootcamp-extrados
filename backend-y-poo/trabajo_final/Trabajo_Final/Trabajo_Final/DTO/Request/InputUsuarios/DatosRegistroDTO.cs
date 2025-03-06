using Constantes.Constantes;
using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.InputUsuarios
{
    public class DatosRegistroDTO
    {
        //aca en teoria hay UN SOLO email por rol.

        [Required(ErrorMessage = "Campo 'email' es obligatorio.")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "Campo 'password' es obligatorio.")]
        public string password { get; set; } //confirmPassword lo debe manejar el front


        [Required(ErrorMessage = "Campo 'rol' es obligatorio.")]
        [RegularExpression(@$"^{Roles.ADMIN}|{Roles.ORGANIZADOR}|{Roles.JUEZ}|{Roles.JUGADOR}$",
            ErrorMessage = $"Rol invalido. Roles permitidos: {Roles.ADMIN} | {Roles.ORGANIZADOR} | {Roles.JUEZ} | {Roles.JUGADOR}")]
        public string rol { get; set; }


        [Required(ErrorMessage = "Campo 'pais' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z +-:]{2,30}$",
            ErrorMessage = "Campo 'pais' solo permite letras, numeros, espacio ( ), +, -, y los dos puntos (:). Entre 2 y 30 caracteres. ")]
        public string pais { get; set; }


        [Required(ErrorMessage = "Campo 'nombre_apellido' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z ]{5,60}$",
          ErrorMessage = "Campo 'nombre_apellido' solo permite letras y espacio. Entre 5 y 60 caracteres. ")]
        public string nombre_apellido { get; set; }



        [Url]
        public string? foto { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{5,25}$",
          ErrorMessage = "Campo 'alias' solo permite letras y numeros. Entre 5 y 25 caracteres. ")]
        public string? alias { get; set; }


    }
}
