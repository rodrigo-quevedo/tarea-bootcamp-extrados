namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class AlreadyLoggedInException : Exception
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
