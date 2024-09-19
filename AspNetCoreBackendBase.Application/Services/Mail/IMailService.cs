namespace AspNetCoreBackendBase.Application.Services
{
    /// <summary>
    /// Defines methods for sending emails.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Asynchronously sends an email to a single recipient.
        /// </summary>
        /// <param name="to">A <see langword="string"/> representing the recipient's email address.</param>
        /// <param name="subject">A <see langword="string"/> representing the subject of the email.</param>
        /// <param name="body">A <see langword="string"/> representing the body of the email.</param>
        /// <param name="isBodyHtml">A <see langword="bool"/> indicating if the body is HTML. Defaults to <see langword="true"/>,
        /// meaning the email body is assumed to be HTML by default.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// Asynchronously sends an email to multiple recipients.
        /// </summary>
        /// <param name="tos">An array of <see langword="string"/> representing the recipient email addresses.</param>
        /// <param name="subject">A <see langword="string"/> representing the subject of the email.</param>
        /// <param name="body">A <see langword="string"/> representing the body of the email.</param>
        /// <param name="isBodyHtml">A <see langword="bool"/> indicating if the body is HTML. Defaults to <see langword="true"/>,
        /// meaning the email body is assumed to be HTML by default.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// Asynchronously sends a password reset email.
        /// </summary>
        /// <param name="to">A <see langword="string"/> representing the recipient's email address.</param>
        /// <param name="userId">A <see cref="Guid"/> that represents the ID of the user requesting the password reset.</param>
        /// <param name="resetToken">A <see langword="string"/> representing the token used for resetting the password.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        Task SendPasswordResetMailAsync(string to, Guid userId, string resetToken);
    }
}
