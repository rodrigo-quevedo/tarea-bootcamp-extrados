using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class DuracionInvalidaException : MiExceptionBase
    {
        public DuracionInvalidaException()
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public DuracionInvalidaException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public DuracionInvalidaException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
