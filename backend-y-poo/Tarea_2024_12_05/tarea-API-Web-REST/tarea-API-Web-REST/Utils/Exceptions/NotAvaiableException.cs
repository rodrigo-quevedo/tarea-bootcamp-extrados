namespace tarea_API_Web_REST.Utils.Exceptions
{
    public class NotAvaiableException : Exception
    {
        public NotAvaiableException()
        {
        }

        public NotAvaiableException(string message)
            : base(message)
        {
        }

        public NotAvaiableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
