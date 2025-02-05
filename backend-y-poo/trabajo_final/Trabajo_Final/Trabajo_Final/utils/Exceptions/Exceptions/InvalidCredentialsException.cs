namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class InvalidCredentialsException : MiExceptionBase
    {
        public InvalidCredentialsException()
        {
        }

        public InvalidCredentialsException(string message)
            : base(message)
        {
        }

        public InvalidCredentialsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
