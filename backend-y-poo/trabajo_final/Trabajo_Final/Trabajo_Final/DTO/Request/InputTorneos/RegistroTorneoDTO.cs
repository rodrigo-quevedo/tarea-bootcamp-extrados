using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.InputTorneos
{
    public class RegistroTorneoDTO
    {
        [Required(ErrorMessage = "Campo 'fecha_hora_inicio' es obligatorio.")]
        [RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3}Z$",
            ErrorMessage = "Ingrese la fecha_hora en formato ISO [aaaa-mm-ddThh:mm:ss.mmmZ]")]
        public string fecha_hora_inicio { get; set; }


        [Required(ErrorMessage = "Campo 'fecha_hora_fin' es obligatorio.")]
        [RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}.[0-9]{3}Z$",
            ErrorMessage = "Ingrese la fecha_hora en formato ISO [aaaa-mm-ddThh:mm:ss.mmmZ]")]
        public string fecha_hora_fin { get; set; }


        [Required(ErrorMessage = "Campo 'horario_inicio' es obligatorio.")]
        [RegularExpression(@"^(([01][0-9])|(2[0-3])):[0-5][0-9]$",
            ErrorMessage = "Introducir horario en formato HH:MM. Horas van de 0 a 23.")]
        public string horario_diario_inicio { get; set; }


        //hora maxima a la que pueden terminar los juegos de ese dia
        [Required(ErrorMessage = "Campo 'horario_fin' es obligatorio.")]
        [RegularExpression(@"^(([01][0-9])|(2[0-3])):[0-5][0-9]$",
            ErrorMessage = "Introducir horario en formato HH:MM. Horas van de 0 a 23.")]
        public string horario_diario_fin { get; set; }


        [Required(ErrorMessage = "Campo 'pais' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z ]{2,30}$",
            ErrorMessage = "Campo 'pais' solo permite letras y espacio ( ). Entre 2 y 30 caracteres. ")]
        public string pais { get; set; }

        [Required(ErrorMessage = "Campo 'series_habilitadas' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Debe haber al menos una serie en 'series_habilitadas'.")]
        public string[] series_habilitadas { get; set; }

        [Required(ErrorMessage = "Campo 'id_jueces_torneo' es obligatorio.")]
        [MinLength(1, ErrorMessage = "Debe haber al un ID en 'id_jueces_torneo'.")]
        public int[] id_jueces_torneo { get; set; }
    }
}
