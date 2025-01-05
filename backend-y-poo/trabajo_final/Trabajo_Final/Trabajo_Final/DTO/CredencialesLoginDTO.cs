using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO
{
    public class CredencialesLoginDTO
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
    }
}
