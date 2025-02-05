using System.ComponentModel.DataAnnotations;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.DTO
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
        public string rol {  get; set; }


        [Required(ErrorMessage = "Campo 'pais' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z ]{2,30}$", 
            ErrorMessage = "Campo 'pais' solo permite letras y numeros y espacio ( ). Entre 2 y 30 caracteres. ")]
        public string pais { get; set; }


        [Required(ErrorMessage = "Campo 'nombre_apellido' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z ]{5,60}$",
          ErrorMessage = "Campo 'nombre_apellido' solo permite letras y espacio. Entre 5 y 60 caracteres. ")]
        public string nombre_apellido { get; set; }




    }
}
