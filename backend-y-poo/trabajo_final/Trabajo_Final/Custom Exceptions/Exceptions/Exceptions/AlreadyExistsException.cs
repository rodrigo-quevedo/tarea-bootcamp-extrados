using Microsoft.AspNetCore.Http;
using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class AlreadyExistsException : MiExceptionBase
    {
        public AlreadyExistsException()
        {
            this.ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public AlreadyExistsException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public AlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
