using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.BuscarCartas
{
    public class BuscarCartasDTO
    {
        [Required(ErrorMessage = "Campo 'id_cartas' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Debe ingresar al menos 1 carta para la búsqueda.")]
        [MaxLength(100, ErrorMessage = "Maximo de 100 cartas por búsqueda.")]
        public int[] id_cartas {  get; set; }
    }
}
