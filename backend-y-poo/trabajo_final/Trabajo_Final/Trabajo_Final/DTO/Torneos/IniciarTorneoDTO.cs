using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Torneos
{
    public class IniciarTorneoDTO
    {
        [Required(ErrorMessage = "Campo 'id_torneo' es obligatorio.")]
        public int? id_torneo { get; set; }
    }
}
