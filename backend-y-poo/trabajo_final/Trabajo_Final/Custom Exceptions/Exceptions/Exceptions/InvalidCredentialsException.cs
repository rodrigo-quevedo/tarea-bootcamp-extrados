using Microsoft.AspNetCore.Http;
using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class InvalidCredentialsException : MiExceptionBase
    {
        public InvalidCredentialsException()
        {
            this.ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidCredentialsException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidCredentialsException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
