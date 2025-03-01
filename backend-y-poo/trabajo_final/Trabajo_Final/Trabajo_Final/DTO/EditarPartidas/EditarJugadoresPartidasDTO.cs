using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Entidades.Custom.JugadoresPartidas;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Trabajo_Final.DTO.EditarPartidas
{
    public class EditarJugadoresPartidasDTO
    {
        [Required(ErrorMessage = "Campo 'editar_jugadores_partida' es obligatorio.")]
        [MinLength(2, ErrorMessage = "Debe haber como mínimo 2 partidas para editar sus jugadores (No se permite editar jugadores dentro de una misma partida. Tampoco se permite cambiar un jugador por otro sin cambiar ese otro jugador al mismo tiempo.)")]
        [ValidarCampos_JugadoresPartida]//Valida que no falte ningun campo.
        [ValidarRepeticionPartida_JugadoresPartida]//Valida que no haya partidas a editar repetidas.
        [ValidarRepeticionJugadores_JugadoresPartida]//Valida que no haya jugadores repetidos
        public JugadoresPartida[] editar_jugadores_partidas { get; set; }
    }


    public class ValidarCampos_JugadoresPartidaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //Console.WriteLine($"Dentro de validation. Valor editar_jugadores_partidas: {JsonSerializer.Serialize(value)}");

            JugadoresPartida[] array = (JugadoresPartida[])value;

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

    public class ValidarRepeticionPartida_JugadoresPartidaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            JugadoresPartida[] array = (JugadoresPartida[])value;

            //Console.WriteLine(
            //    JsonSerializer.Serialize(
            //        array
            //        .GroupBy(partida => partida.Id_partida)//partida.Id_partida es la Key
            //        .Where(partidaAgrupadaPorId_partida => partidaAgrupadaPorId_partida.Count() > 1)
            //        .First().Key// Key = partida.Id_partida de esa primera partida que fue agrupada
            //    )
            //);
            //int id_partida_repetida = 0;
       
            IList<IGrouping<int, JugadoresPartida>> partidasRepetidas_agrupadas = 
                array
                .GroupBy(partida => partida.Id_partida)
                .Where(partidaAgrupada => partidaAgrupada.Count() > 1)
                .ToList();

            if ( ! partidasRepetidas_agrupadas.Any()) return true;

            //hay repetidas -> tirar exception

            IList<Campo_Mensaje_Error> errors = new List<Campo_Mensaje_Error>();

            List<JugadoresPartida> arrayMutable = array.ToList();

            foreach (IGrouping<int, JugadoresPartida> grupoPartidas in partidasRepetidas_agrupadas)
            {
                foreach(JugadoresPartida partidaDelGrupo in grupoPartidas)
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
                    arrayMutable[indexRepeticion].Id_partida = Int32.MaxValue;//Le asigno otro valor para que no vuelva a salir.
                }
            }
            throw new MultipleInvalidInputException(errors.ToArray());
        }

    }


    public class ValidarRepeticionJugadores_JugadoresPartidaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            JugadoresPartida[] array = (JugadoresPartida[])value;

            IList<int> id_jugadores =
                array.Select(j => j.Id_jugador_1) //jugadores_1
                .Concat(//le adjunto los jugadores_2
                    array.Select(j => j.Id_jugador_2)
                )
                .ToList();


            IEnumerable<IGrouping<int, int>> idRepetidas_agrupadas =
                id_jugadores
                .GroupBy(id => id)
                .Where(grupo => grupo.Count() > 1);

            if (!idRepetidas_agrupadas.Any()) return true;

            //armar exception:

            IList<Campo_Mensaje_Error> errors = new List<Campo_Mensaje_Error>();

            List<JugadoresPartida> arrayMutable = array.ToList();

            foreach (IGrouping<int, int> grupo in idRepetidas_agrupadas)
            {
                foreach(int id_repetida in grupo)//para cada id repetida
                {
                    int indexRepeticion;
                       indexRepeticion = //la quiero encontrar en el primer array
                        arrayMutable
                        //primero voy a buscar las repeticiones en Id_jugador_1
                        .FindIndex(partida => partida.Id_jugador_1 == id_repetida);


                    //no hay Id_jugador_1 con repeticion
                    if (indexRepeticion == -1)
                    {
                        indexRepeticion =
                            arrayMutable
                            //si no hay Id_jugador_1 con repeticion, busco en los Id_jugador_2
                            .FindIndex(partida => partida.Id_jugador_2 == id_repetida);

                        errors.Add(new Campo_Mensaje_Error() {
                            Campo = $"editar_jugadores_partidas[{indexRepeticion}]",
                            Error = $"Campo [Id_jugador_2: {id_repetida}] está repetido."
                        });

                        arrayMutable[indexRepeticion].Id_jugador_2 = Int32.MaxValue;
                        continue;
                    }

                    //hay Id_jugador_1 con repeticion:
                    errors.Add(new Campo_Mensaje_Error()
                    {
                        Campo = $"editar_jugadores_partidas[{indexRepeticion}]",
                        Error = $"Campo [Id_jugador_1: {id_repetida}] está repetido."
                    });

                    arrayMutable[indexRepeticion].Id_jugador_1 = Int32.MaxValue;

                }
            }


            throw new MultipleInvalidInputException(errors.ToArray());
        }
    }

}
