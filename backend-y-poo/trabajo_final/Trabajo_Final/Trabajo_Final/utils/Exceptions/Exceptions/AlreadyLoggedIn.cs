using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class AlreadyLoggedInException : MiExceptionBase
    {
        public AlreadyLoggedInException()
        {
            this.ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public AlreadyLoggedInException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public AlreadyLoggedInException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
