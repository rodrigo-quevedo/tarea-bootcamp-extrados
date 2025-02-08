using Microsoft.AspNetCore.Http;

namespace Trabajo_Final.utils.Exceptions.BaseException
{
    public class MiExceptionBase : Exception
    {
        public int ExceptionStatusCode { get; set; }


        public MiExceptionBase()
        {
            this.ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public MiExceptionBase(string message)
            : base(message)
        {
            this.ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public MiExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
            this.ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
