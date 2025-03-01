using Custom_Exceptions.Exceptions.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Trabajo_Final.DTO.EditarPartidas
{
    public class EditarJugadoresPartidasDTO
    {
        [Required(ErrorMessage = "Campo 'editar_jugadores_partida' es obligatorio.")]
        [MinLength(2, ErrorMessage = "Debe haber como mínimo 2 partidas para editar sus jugadores (No se permite editar jugadores dentro de una misma partida. Tampoco se permite cambiar un jugador por otro sin cambiar ese otro jugador al mismo tiempo.)")]
        [Validar_Jugadores_Partida]//Valida que no falte ningun campo.
        [ValidarRepeticion_Jugadores_Partida]//Valida que no haya partidas a editar repetidas.
        public Jugadores_Partida[] editar_jugadores_partidas { get; set; }
    }


    public class Jugadores_Partida
    {
        public int Id_partida { get; set; }
        public int Id_jugador_1 { get; set; }
        public int Id_jugador_2 { get; set; }
    }

    public class Validar_Jugadores_PartidaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //Console.WriteLine($"Dentro de validation. Valor editar_jugadores_partidas: {JsonSerializer.Serialize(value)}");

            Jugadores_Partida[] array = (Jugadores_Partida[])value;

            IList<Campo_Mensaje_Error> errors = new List<Campo_Mensaje_Error>();
            bool isValid = true;


            for (int i = 0; i < array.Length; i++)
            {
                string campoIndex = $"editar_jugadores_partidas[{i}]";

                if (array[i].Id_partida == default)
                {
                    errors.Add(new Campo_Mensaje_Error
                    {
                        Campo = campoIndex,
                        Error = "Campo [Id_partida] es obligatorio."
                    }
                    );
                    isValid = false;
                }
                if (array[i].Id_jugador_1 == default)
                {
                    errors.Add(new Campo_Mensaje_Error
                    {
                        Campo = campoIndex,
                        Error = "Campo 'Id_jugador_1' es obligatorio."
                    }
                    );
                    isValid = false;
                }
                if (array[i].Id_jugador_2 == default)
                {
                    errors.Add(new Campo_Mensaje_Error
                    {
                        Campo = campoIndex,
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

    public class ValidarRepeticion_Jugadores_PartidaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Jugadores_Partida[] array = (Jugadores_Partida[])value;

            //Console.WriteLine(
            //    JsonSerializer.Serialize(
            //        array
            //        .GroupBy(partida => partida.Id_partida)//partida.Id_partida es la Key
            //        .Where(partidaAgrupadaPorId_partida => partidaAgrupadaPorId_partida.Count() > 1)
            //        .First().Key// Key = partida.Id_partida de esa primera partida que fue agrupada
            //    )
            //);
            //int id_partida_repetida = 0;
       
            IList<IGrouping<int, Jugadores_Partida>> partidasRepetidas_agrupadas = 
                array
                .GroupBy(partida => partida.Id_partida)
                .Where(partidaAgrupada => partidaAgrupada.Count() > 1)
                .ToList();

            if (partidasRepetidas_agrupadas.Count == 0) return true;

            //hay repetidas -> tirar exception

            IList<Campo_Mensaje_Error> errors = new List<Campo_Mensaje_Error>();

            List<Jugadores_Partida> arrayMutable = array.ToList();

            foreach (IGrouping<int, Jugadores_Partida> grupoPartidas in partidasRepetidas_agrupadas)
            {
                foreach(Jugadores_Partida partidaDelGrupo in grupoPartidas)
                {
                    int indexRepeticion =
                             arrayMutable
                             .FindIndex(//Esto agarra SOLAMENTE la primer coincidencia.
                                element => (
                                     element.Id_partida == partidaDelGrupo.Id_partida
                                     &&
                                     element.Id_jugador_1 == partidaDelGrupo.Id_jugador_1
                                     &&
                                     element.Id_jugador_2 == partidaDelGrupo.Id_jugador_2
                                  )
                             );


                    errors.Add(new Campo_Mensaje_Error() {
                        Campo = $"editar_jugadores_partidas[{indexRepeticion}]",
                        Error = $"Campo [Id_partida: {partidaDelGrupo.Id_partida}] está repetido."
                    });

                    //Console.WriteLine(JsonSerializer.Serialize(arrayMutable));  

                    //Acá hay que tener cuidado y mutarlo recién cuando se termina de usar 
                    //el elemento en ese index.
                    arrayMutable[indexRepeticion].Id_partida = 99999;//Le asigno otro valor para que no vuelva a salir.
                }
            }
            throw new MultipleInvalidInputException(errors.ToArray());

        }
    }

}
