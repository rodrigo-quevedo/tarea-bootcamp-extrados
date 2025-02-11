using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class AlreadyExistsException : MiExceptionBase
    {
        public AlreadyExistsException()
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public AlreadyExistsException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public AlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
