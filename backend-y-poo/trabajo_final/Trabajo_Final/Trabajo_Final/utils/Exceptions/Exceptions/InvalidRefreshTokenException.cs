namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class InvalidRefreshTokenException : MiExceptionBase
    {
        public InvalidRefreshTokenException()
        {
        }

        public InvalidRefreshTokenException(string message)
            : base(message)
        {
        }

        public InvalidRefreshTokenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
