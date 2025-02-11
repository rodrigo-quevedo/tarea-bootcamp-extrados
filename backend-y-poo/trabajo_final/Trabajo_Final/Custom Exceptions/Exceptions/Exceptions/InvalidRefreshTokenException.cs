using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class InvalidRefreshTokenException : MiExceptionBase
    {
        public InvalidRefreshTokenException()
        {
            ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidRefreshTokenException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidRefreshTokenException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
