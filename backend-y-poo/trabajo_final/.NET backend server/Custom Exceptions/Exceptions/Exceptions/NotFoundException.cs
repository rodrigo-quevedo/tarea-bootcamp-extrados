using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class NotFoundException : MiExceptionBase
    {
        public NotFoundException()
        {
            ExceptionStatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status404NotFound;
        }
    }
}
