using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Torneos
{
    public class CancelarTorneoDTO
    {
        [Required(ErrorMessage = "Campo 'Id_torneo' es obligatorio.")]
        public int? Id_torneo {  get; set; }
        public string? motivo { get; set; }
    }
}
