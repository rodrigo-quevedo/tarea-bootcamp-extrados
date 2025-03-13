using Microsoft.AspNetCore.Http;

namespace Custom_Exceptions.Exceptions.BaseException
{
    public class MiExceptionBase : Exception
    {
        public int ExceptionStatusCode { get; set; }


        public MiExceptionBase()
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public MiExceptionBase(string message)
            : base(message)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }

        public MiExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
            ExceptionStatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
