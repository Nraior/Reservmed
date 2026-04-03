using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Reservmed.Common.Settings;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;

namespace Reservmed.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailSettings _settings;

        public EmailSenderService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        private async Task<string> PrepareMailWithDefaultContentAsync(string name, string link, string templateName)
        {
            var safeName = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(name);

            var content = await PrepareMailAsync(templateName, (string emailBody) =>
            {
                emailBody = emailBody.Replace("{{username}}", safeName);
                emailBody = emailBody.Replace("{{link}}", link);
                return emailBody;

            });

            return content;
        }

        private async Task<string> PrepareMailAsync(string templateFile, Func<string, string> templateMapper)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", templateFile);
            var emailBody = await File.ReadAllTextAsync(templatePath);

            return templateMapper(emailBody);
        }

        private async Task SendEmailAsync(string email, string htmlMessage, string subject)
        {
            var emailMessage = new MimeMessage();

            var senderName = _settings.SenderName;
            var senderEmail = _settings.SenderEmail;
            var senderUsername = _settings.SenderUsername;
            var senderPassword = _settings.SenderPassword;
            var host = _settings.SmtpServer;
            var port = _settings.SmtpPort;

            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderUsername, senderPassword);
                await client.SendAsync(emailMessage);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public async Task PrepareAndSendRegistrationEmail(ApplicationUser user, string name, string link)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                throw new InvalidOperationException("Cannot send reset password email. User email is missing ");
            }
            var emailContent = await PrepareMailWithDefaultContentAsync(name, link, "RegistrationEmail.html");
            await SendEmailAsync(user.Email, emailContent, "Reservmed - Registration Confirmation");

        }

        public async Task PrepareAndSendResetPasswordEmail(ApplicationUser user, string link)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                throw new InvalidOperationException("Cannot send reset password email. User email is missing ");
            }
            var emailContent = await PrepareMailWithDefaultContentAsync("", link, "ResetPasswordEmail.html");
            await SendEmailAsync(user.Email, emailContent, "Reservmed - Password Reset Request");
        }

    }
}
