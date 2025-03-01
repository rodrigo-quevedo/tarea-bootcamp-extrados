using Custom_Exceptions.Exceptions.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Trabajo_Final.DTO.EditarPartidas
{
    public class EditarJugadoresPartidasDTO
    {
        [Required(ErrorMessage = "Campo 'editar_jugadores_partida' es obligatorio.")]
        [MinLength(2, ErrorMessage = "Debe haber como mínimo 2 partidas para editar sus jugadores (No se permite editar jugadores dentro de una misma partida. Tampoco se permite cambiar un jugador por otro sin cambiar ese otro jugador al mismo tiempo.)")]
        [Validar_Jugadores_Partida]
        public Jugadores_Partida[] editar_jugadores_partidas { get; set; }
    }


    public class Jugadores_Partida
    {
        //[Required(ErrorMessage = "Campo 'Id_partida' es obligatorio para cada elemento del array 'editar_jugadores_partidas'.")]
        public int Id_partida { get; set; }
        
        //[Required(ErrorMessage = "Campo 'Id_jugador_1' es obligatorio para cada elemento del array 'editar_jugadores_partidas'.")]
        public int Id_jugador_1 { get; set; }

        //[Required(ErrorMessage = "Campo 'Id_jugador_2' es obligatorio para cada elemento del array 'editar_jugadores_partidas'.")]
        public int Id_jugador_2 { get; set; }
    }

    public class Validar_Jugadores_PartidaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Console.WriteLine($"Dentro de validation. Valor editar_jugadores_partidas: {JsonSerializer.Serialize(value)}");

            Jugadores_Partida[] array = (Jugadores_Partida[]) value;

            IList<Campo_Mensaje_Error> errors = new List<Campo_Mensaje_Error>();
            bool isValid = true;


            for (int i=0; i<array.Length; i++)
            {
                if (array[i].Id_partida == default)
                {
                    errors.Add(new Campo_Mensaje_Error{ 
                        Campo = $"editar_jugadores_partidas index [{i}]", 
                        Error = "Campo [Id_partida] es obligatorio." }
                    );
                    isValid = false;
                }
                if (array[i].Id_jugador_1 == default)
                {
                    errors.Add(new Campo_Mensaje_Error
                    {
                        Campo = $"editar_jugadores_partidas index [{i}]",
                        Error = "Campo 'Id_jugador_1' es obligatorio."
                    }
                    );
                    isValid = false;
                }
                if (array[i].Id_jugador_2 == default)
                {
                    errors.Add(new Campo_Mensaje_Error
                    {
                        Campo = $"editar_jugadores_partidas index [{i}]",
                        Error = "Campo [Id_jugador_2] es obligatorio."
                    }
                    );
                    isValid = false;
                }
            }

            if (!isValid) throw new MultipleInvalidInputException(errors.ToArray());

            return isValid;
        }
    }





}
