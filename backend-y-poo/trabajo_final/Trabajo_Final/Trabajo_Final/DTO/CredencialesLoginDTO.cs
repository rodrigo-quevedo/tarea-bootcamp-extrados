using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO
{
    public class CredencialesLoginDTO
    {
        //aca en teoria hay UN SOLO email por rol.

        [Required(ErrorMessage = "Campo 'email' es obligatorio.")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "Campo 'password' es obligatorio.")]
        public string password { get; set; }
    }
}
