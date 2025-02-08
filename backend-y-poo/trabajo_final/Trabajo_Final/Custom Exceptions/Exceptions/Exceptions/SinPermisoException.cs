using Microsoft.AspNetCore.Http;
using Trabajo_Final.utils.Exceptions.BaseException;

namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class SinPermisoException : MiExceptionBase
    {
        public SinPermisoException()
        {
            this.ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public SinPermisoException(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }

        public SinPermisoException(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
