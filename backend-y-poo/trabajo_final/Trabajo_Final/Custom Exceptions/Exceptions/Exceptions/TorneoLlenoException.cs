using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class TorneoLlenoException : MiExceptionBase
    {
        public TorneoLlenoException()
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
