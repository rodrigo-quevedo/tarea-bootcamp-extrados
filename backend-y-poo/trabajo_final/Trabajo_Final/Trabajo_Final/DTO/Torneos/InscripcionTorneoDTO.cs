using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Torneos
{
    public class InscripcionTorneoDTO
    {
        [Required(ErrorMessage = "Campo 'id_torneo' es obligatorio")]
        public int? id_torneo {  get; set; }

        [Required(ErrorMessage = "Campo 'id_cartas_mazo' es obligatorio")]
        [MinLength(15, ErrorMessage = "Debe haber 15 IDs de cartas (coleccionadas) en el mazo.")]
        [MaxLength(15, ErrorMessage = "Debe haber 15 IDs de cartas (coleccionadas) en el mazo.")]
        public int[] id_cartas_mazo { get; set; }
    }
}
