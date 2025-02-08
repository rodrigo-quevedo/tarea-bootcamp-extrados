using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class InvalidRefreshTokenException : MiExceptionBase
    {
        public InvalidRefreshTokenException()
        {
            this.ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidRefreshTokenException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }

        public InvalidRefreshTokenException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
