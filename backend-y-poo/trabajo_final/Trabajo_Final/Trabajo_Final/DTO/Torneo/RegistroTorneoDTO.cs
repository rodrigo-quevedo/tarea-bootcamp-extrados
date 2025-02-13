using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Torneo
{
    public class RegistroTorneoDTO
    {
        [Required(ErrorMessage = "Campo 'fecha_hora_inicio' es obligatorio.")]
        [RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3}Z$",
            ErrorMessage = "Ingrese la fecha_hora en formato ISO [aaaa-mm-ddThh:mm:ss.mmmZ]")]
        public string fecha_hora_inicio {  get; set; }
        
        
        [Required(ErrorMessage = "Campo 'fecha_hora_fin' es obligatorio.")]
        [RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3}Z$",
            ErrorMessage = "Ingrese la fecha_hora en formato ISO [aaaa-mm-ddThh:mm:ss.mmmZ]")]
        public string fecha_hora_fin {  get; set; }
        
        
        [Required(ErrorMessage = "Campo 'pais' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z ]{2,30}$",
            ErrorMessage = "Campo 'pais' solo permite letras y espacio ( ). Entre 2 y 30 caracteres. ")]
        public string pais {  get; set; }


        public string[] series_habilitadas {  get; set; }

        public int[] id_jueces_torneo { get; set; }
    }
}
