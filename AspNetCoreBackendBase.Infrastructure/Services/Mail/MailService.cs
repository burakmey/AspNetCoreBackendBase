using System.Net.Mail;
using System.Net;
using System.Text;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Application;

namespace AspNetCoreBackendBase.Infrastructure.Services
{
    public class MailService : IMailService
    {
        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            // Create a new MailMessage object.
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;

            // Add each recipient to the mail message.
            foreach (var to in tos)
                mail.To.Add(to);

            // Set the subject and body of the email.
            mail.Subject = subject;
            mail.Body = body;

            // Set the sender's email address and display name.
            mail.From = new(Configuration.GetMailUserName, Configuration.GetMailDisplayName, Encoding.UTF8);

            // Create and configure the SmtpClient.
            SmtpClient smtp = new()
            {
                Credentials = new NetworkCredential(Configuration.GetMailUserName, Configuration.GetMailPassword), // SMTP credentials
                Port = int.Parse(Configuration.GetMailPort), // SMTP port
                EnableSsl = true, // Enable SSL
                Host = Configuration.GetMailHost // SMTP host
            };

            // Send the email asynchronously.
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, Guid userId, string resetToken)
        {
            // Retrieve the origin URL for the reset link.
            string originUrl = Configuration.GetOrigin;

            // Create the email body with a reset link.
            string emailBody = $@"
                Hello,<br>
                If you requested a new password, you can reset it using the link below.<br>
                <strong><a target=""_blank"" href=""{originUrl}/update-password/{userId}/{resetToken}"">Click here to reset your password...</a></strong><br><br>
                <span style=""font-size:12px;"">NOTE: If you did not request this, please ignore this email.</span><br>
                Best regards,<br><br>
                Mail Display Name";

            // Send the password reset email.
            await SendMailAsync(to, "Password Reset Request", emailBody);
        }
    }
}
