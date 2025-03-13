using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.BuscarPartidas
{
    public class BuscarPartidasDTO
    {

        [Required(ErrorMessage = "Campo 'id_partidas' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Debe ingresar al menos 1 partida para la búsqueda.")]
        [MaxLength(200, ErrorMessage = "Hay un maximo de 200 partidas por búsqueda.")]
        public int[] id_partidas { get; set; }
        

    }
}
