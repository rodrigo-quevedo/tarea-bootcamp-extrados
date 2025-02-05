namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class AlreadyLoggedInException : MiExceptionBase
    {
        public AlreadyLoggedInException()
        {
        }

        public AlreadyLoggedInException(string message)
            : base(message)
        {
        }

        public AlreadyLoggedInException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
