namespace AspNetCoreBackendBase.Application.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException() : base("An unexpected error occurred while registering.")
        {
        }

        public RegisterException(string? message) : base(message)
        {
        }

        public RegisterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
