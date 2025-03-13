using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class InvalidCredentialsException : MiExceptionBase
    {
        public InvalidCredentialsException()
        {
            ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidCredentialsException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidCredentialsException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
