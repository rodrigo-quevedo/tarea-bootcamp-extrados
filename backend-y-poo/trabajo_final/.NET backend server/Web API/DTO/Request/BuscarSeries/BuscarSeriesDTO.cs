using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.BuscarSeries
{
    public class BuscarSeriesDTO
    {
        [Required(ErrorMessage = "Campo 'nombres_series' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Debe ingresar al menos 1 serie para la búsqueda.")]
        [MaxLength(200, ErrorMessage = "Hay un maximo de 200 series por búsqueda.")]
        public string[] nombres_series {  get; set; }
    }

}
