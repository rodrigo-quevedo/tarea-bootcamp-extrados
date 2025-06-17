using System.ComponentModel.DataAnnotations;

namespace Web_API.DTO.Request.Mazos;

public class ArrayIdCartasMazoDTO
{
    [Required(ErrorMessage = "Campo 'Id_cartas' es obligatorio.")]
    [MinLength(8, ErrorMessage = "Se debe ingresar al menos 8 IDs de carta.")]
    [MaxLength(15, ErrorMessage = "Se debe ingresar maximo 15 IDs de carta.")]
    public int[] Id_cartas { get; set; }
}
