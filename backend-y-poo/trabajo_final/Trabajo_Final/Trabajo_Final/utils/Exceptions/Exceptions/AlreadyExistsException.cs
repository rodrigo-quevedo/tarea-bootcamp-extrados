namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class AlreadyExistsException : MiExceptionBase
    {
        public AlreadyExistsException()
        {
        }

        //public int

        public AlreadyExistsException(string message)
            : base(message)
        {
        }

        public AlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
