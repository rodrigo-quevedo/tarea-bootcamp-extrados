namespace tarea_API_Web_REST.Utils.Exceptions
{
    public class InvalidRefreshTokenException : Exception
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
