namespace AspNetCoreBackendBase.Application.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base("Authentication error.")
        {
        }

        public InvalidLoginException(string? message) : base(message)
        {
        }

        public InvalidLoginException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}