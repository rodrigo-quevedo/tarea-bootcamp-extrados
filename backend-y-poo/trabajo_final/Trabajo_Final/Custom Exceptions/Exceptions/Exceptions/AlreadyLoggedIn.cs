using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class AlreadyLoggedInException : MiExceptionBase
    {
        public AlreadyLoggedInException()
        {
            ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public AlreadyLoggedInException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public AlreadyLoggedInException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
