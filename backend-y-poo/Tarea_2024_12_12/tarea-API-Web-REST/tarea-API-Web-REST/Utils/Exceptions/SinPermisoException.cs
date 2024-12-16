namespace tarea_API_Web_REST.Utils.Exceptions
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
