using System.ComponentModel.DataAnnotations;


namespace Trabajo_Final.DTO
{
    public class DatosTorneoDTO
    {

        [Required(ErrorMessage = "Campo 'pais' es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9 :+-]{2,30}$",
            ErrorMessage = "Campo 'pais' solo permite letras y numeros, espacio ( ), +,-, y dos puntos (:). Entre 2 y 30 caracteres. ")]
        public string pais { get; set; }

        public DateTime fecha_hora_inicio { get; set; }
        public DateTime fecha_hora_fin {  get; set; }




    }
}
