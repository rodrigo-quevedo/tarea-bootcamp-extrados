using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class SinPermisoException : MiExceptionBase
    {
        public SinPermisoException()
        {
            ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public SinPermisoException(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public SinPermisoException(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
