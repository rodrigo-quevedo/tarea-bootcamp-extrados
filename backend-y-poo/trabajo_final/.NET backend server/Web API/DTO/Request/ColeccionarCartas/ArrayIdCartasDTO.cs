using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.ColeccionarCartas
{
    public class ArrayIdCartasDTO
    {
        [Required(ErrorMessage = "Campo 'Id_cartas' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Se debe ingresar al menos una ID de carta.")]
        public int[] Id_cartas { get; set; }
    }
}
