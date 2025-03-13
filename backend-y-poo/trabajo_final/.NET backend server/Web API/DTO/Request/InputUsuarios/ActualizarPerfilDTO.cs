using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.InputUsuarios
{
    public class ActualizarPerfilDTO
    {
        //No pueden ser [Required] por que a veces el jugador/juez va a querer
        //cambiar solamente la foto o el alias y dejar lo otro como estaba.


        public IFormFile? foto { get; set; }

        [RegularExpression("^[a-zA-Z0-9]{4,25}$",
            ErrorMessage = "Válidos: letras, números. Entre 4 y 25 caracteres.")]
        public string? alias { get; set; }
    }
}
