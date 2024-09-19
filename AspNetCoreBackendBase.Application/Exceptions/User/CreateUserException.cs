namespace AspNetCoreBackendBase.Application.Exceptions
{
    public class CreateUserException : Exception
    {
        public CreateUserException() : base("Register problem.")
        {
        }

        public CreateUserException(string? message) : base(message)
        {
        }

        public CreateUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
