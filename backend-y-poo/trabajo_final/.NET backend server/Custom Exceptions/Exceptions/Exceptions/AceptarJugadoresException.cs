using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class AceptarJugadoresException : MiExceptionBase
    {
        public AceptarJugadoresException(): base("El jugador se inscribió con éxito pero no se aceptaron todos los jugadores que deberían haberse aceptado.")
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public AceptarJugadoresException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public AceptarJugadoresException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
