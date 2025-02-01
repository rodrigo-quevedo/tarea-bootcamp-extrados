using System.ComponentModel.DataAnnotations;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.DTO
{
    public class DatosRegistroDTO
    {
        //aca en teoria hay UN SOLO email por rol.

        [Required(ErrorMessage = "Campo 'email' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,20}@gmail\.com$", 
            ErrorMessage = "Campo 'email' debe terminar en '@gmail.com'. " +
            "Antes del '@gmail.com' se permiten SOLO letras y numeros, ademas entre 3 y 20 caracteres.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Campo 'password' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9]{4,20}$", 
            ErrorMessage = "Campo 'password' solo permite letras y numeros, entre 4 y 20 caracteres. ")]
        public string password { get; set; }


        [Required(ErrorMessage = "Campo 'confirmPassword' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9]{4,20}$",
            ErrorMessage = "Campo 'confirmPassword' solo permite letras y numeros, entre 4 y 20 caracteres. ")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden. Intente nuevamente.")]
        public string confirmPassword { get; set; }


        [Required(ErrorMessage = "Campo 'rol' es obligatorio.")]
        [RegularExpression(@$"^{Roles.ADMIN}|{Roles.ORGANIZADOR}|{Roles.JUEZ}|{Roles.JUGADOR}$", 
            ErrorMessage = $"Rol invalido. Roles permitidos: {Roles.ADMIN} | {Roles.ORGANIZADOR} | {Roles.JUEZ} | {Roles.JUGADOR}")]
        public string rol {  get; set; }


        [Required(ErrorMessage = "Campo 'pais' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9 :+-]{2,30}$",
            ErrorMessage = "Campo 'pais' solo permite letras y numeros, espacio ( ), +,-, y dos puntos (:). Entre 2 y 30 caracteres. ")]
        public string pais { get; set; }


        [Required(ErrorMessage = "Campo 'nombre_apellido' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]{5,60}$",
          ErrorMessage = "Campo 'nombre_apellido' solo permite letras. Entre 5 y 60 caracteres. ")]
        public string nombre_apellido { get; set; }




    }
}
