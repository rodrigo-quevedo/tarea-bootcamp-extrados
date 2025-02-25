using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.OficializarPartidas
{
    public class OficializarPartidaDTO
    {
        [Required(ErrorMessage = "Campo 'id_partida' es obligatorio.")]
        public int? id_partida {  get; set; }
        
        [Required(ErrorMessage = "Campo 'id_ganador' es obligatorio.")]
        public int? id_ganador { get; set; }//si hay un descalificado, el otro es ganador
        

        public int? id_descalificado { get; set; }//opcional

        [MaxLength(60, ErrorMessage = "La cantidad maxima de caracteres es 60.")]
        public string? motivo_descalificacion { get; set; }
    }
}
