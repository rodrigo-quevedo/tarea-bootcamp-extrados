using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Torneos
{
    public class EditarJuezTorneoDTO
    {
        
        [Required(ErrorMessage = "Campo 'id_juez' es obligatorio")]
        public int? id_juez { get; set; } //lo hago nulleable para que funcione el [Required]


    }
}
