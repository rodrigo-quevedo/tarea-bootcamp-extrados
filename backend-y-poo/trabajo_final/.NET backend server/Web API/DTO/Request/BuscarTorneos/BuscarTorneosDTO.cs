﻿using Constantes.Constantes;
using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Request.BuscarTorneos
{
    public class BuscarTorneosDTO
    {

        [ArrayFases(ErrorMessage = $"Fases validas: {FasesTorneo.REGISTRO}|{FasesTorneo.TORNEO}|{FasesTorneo.FINALIZADO}")]
        public string[]? fases { get; set; }
    }


    public class ArrayFasesAttribute : ValidationAttribute
    {
        public ArrayFasesAttribute() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (value == null) return ValidationResult.Success;

                if (value is string[] stringArray)
                {
                    //Console.WriteLine($"Validando array. Elemento actual: {value}");
                    foreach (string item in stringArray)
                    {
                        if (!FasesTorneo.fases.Contains(item))
                            return new ValidationResult($"'{item}' no es una fase válida.")
                            {
                                ErrorMessage = $"'{item}' no es una fase válida."
                            };
                    }
                    return ValidationResult.Success;
                }

            }
            catch (Exception ex)
            {
                return new ValidationResult($"Debe ingresar un array string[]. Detalle:{ex.Message}") { ErrorMessage = $"Debe ingresar un array string[]. Detalle:{ex.Message}" };
            }

            return new ValidationResult($"Debe ingresar un array string[].");
        }
    }
}
