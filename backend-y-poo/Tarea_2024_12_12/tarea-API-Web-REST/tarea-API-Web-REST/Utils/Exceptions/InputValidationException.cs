namespace tarea_API_Web_REST.Utils.Exceptions
{
    public class InputValidationException : Exception
    {
        //basado en el ejemplo de: https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions
        public InputValidationException()
        {
        }

        public InputValidationException(string message)
            : base(message)
        {
        }

        public InputValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
