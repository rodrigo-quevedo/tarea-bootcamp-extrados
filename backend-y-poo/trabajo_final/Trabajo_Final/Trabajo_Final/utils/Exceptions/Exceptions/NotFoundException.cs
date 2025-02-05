namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class NotFoundException : MiExceptionBase
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
