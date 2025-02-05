namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class MiExceptionBase : Exception
    {
        public MiExceptionBase()
        {
        }

        public int GetStatusCode()
        {
            return StatusCodes.Status500InternalServerError;
        }

        public MiExceptionBase(string message)
            : base(message)
        {
        }

        public MiExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
