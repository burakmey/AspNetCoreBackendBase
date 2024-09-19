namespace AspNetCoreBackendBase.Application.Exceptions
{
    public class GoogleLoginInvalidAuthenticationException : Exception
    {
        public GoogleLoginInvalidAuthenticationException() : base("Invalid Google external authentication.")
        {
        }

        public GoogleLoginInvalidAuthenticationException(string? message) : base(message)
        {
        }

        public GoogleLoginInvalidAuthenticationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
