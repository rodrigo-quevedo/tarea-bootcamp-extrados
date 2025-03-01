using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.EditarPartidas
{
    public class EditarJuezPartidaDTO
    {
        [Required(ErrorMessage = "Campo 'Id_partida' es obligatorio.")]
        public int? Id_partida {  get; set; }


        [Required(ErrorMessage = "Campo 'Id_juez' es obligatorio.")]
        public int? Id_juez {  get; set; }
    }
}
