namespace AspNetCoreBackendBase.Application.Exceptions
{
    public class PasswordResetFailedException : Exception
    {
        // Default constructor with a predefined error message.
        public PasswordResetFailedException() : base("Password reset failed. Please try again later.")
        {
        }

        // Constructor that accepts a custom message.
        public PasswordResetFailedException(string? message) : base(message)
        {
        }

        // Constructor that accepts a custom message and an inner exception.
        public PasswordResetFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
