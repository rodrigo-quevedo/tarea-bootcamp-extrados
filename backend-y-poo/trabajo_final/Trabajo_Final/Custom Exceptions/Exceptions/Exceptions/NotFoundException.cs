using Microsoft.AspNetCore.Http;
using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class NotFoundException : MiExceptionBase
    {
        public NotFoundException()
        {
            this.ExceptionStatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status404NotFound;
        }
    }
}
