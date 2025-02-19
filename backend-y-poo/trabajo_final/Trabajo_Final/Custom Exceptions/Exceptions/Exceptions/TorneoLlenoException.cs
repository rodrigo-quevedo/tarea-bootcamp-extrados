using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class TorneoLlenoException : MiExceptionBase
    {
        public TorneoLlenoException(): base ("El torneo ya está lleno.")
        {
            ExceptionStatusCode = StatusCodes.Status409Conflict;
        }

        public TorneoLlenoException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status409Conflict;
        }

        public TorneoLlenoException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status409Conflict;
        }
    }
}
