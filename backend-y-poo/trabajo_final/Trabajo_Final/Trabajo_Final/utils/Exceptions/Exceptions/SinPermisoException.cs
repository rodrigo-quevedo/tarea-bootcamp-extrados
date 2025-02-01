namespace Trabajo_Final.utils.Exceptions.Exceptions
{
    public class SinPermisoException : Exception
    {
        public SinPermisoException()
        {
        }

        public SinPermisoException(string message)
            : base(message)
        {
        }

        public SinPermisoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
