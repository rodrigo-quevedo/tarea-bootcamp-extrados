using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class InvalidInputException : MiExceptionBase
    {
        public InvalidInputException()
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public InvalidInputException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public InvalidInputException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
