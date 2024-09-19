namespace AspNetCoreBackendBase.Application.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException() : base("Refresh token expired, login your account again.")
        {
        }

        public RefreshTokenExpiredException(string? message) : base(message)
        {
        }

        public RefreshTokenExpiredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
