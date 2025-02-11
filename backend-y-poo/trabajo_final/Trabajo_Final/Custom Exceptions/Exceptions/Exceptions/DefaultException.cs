using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class DefaultException : MiExceptionBase
    {
        public DefaultException()
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public DefaultException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public DefaultException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
