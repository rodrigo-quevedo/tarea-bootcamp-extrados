using Microsoft.AspNetCore.Http;
using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class DefaultException : MiExceptionBase
    {
        public DefaultException()
        {
            this.ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public DefaultException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public DefaultException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
