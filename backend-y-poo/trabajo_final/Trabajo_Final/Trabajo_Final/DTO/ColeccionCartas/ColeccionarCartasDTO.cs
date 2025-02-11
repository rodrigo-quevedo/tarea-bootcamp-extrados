using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.ColeccionCartas
{
    public class ColeccionarCartasDTO
    {
        [Required(ErrorMessage = "Campo 'Id_cartas' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Se debe ingresar al menos una ID de carta para coleccionar.")]
        public int[] Id_cartas {  get; set; }
    }
}
