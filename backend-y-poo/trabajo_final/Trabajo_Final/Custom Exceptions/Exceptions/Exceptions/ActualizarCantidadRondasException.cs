using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class ActualizarCantidadRondasException : MiExceptionBase
    {
        public ActualizarCantidadRondasException(): base("El jugador se inscribió con éxito pero no se pudo actualizar la cantidad de rondas del torneo.")
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public ActualizarCantidadRondasException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public ActualizarCantidadRondasException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
