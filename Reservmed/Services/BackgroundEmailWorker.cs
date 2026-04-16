
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Reservmed.Common.Settings;
using Reservmed.DTOs.Internal;
using System.Threading.Channels;

namespace Reservmed.Services
{
    public class BackgroundEmailWorker : BackgroundService
    {
        private Channel<EmailDataDto> _emailQueue;
        //IEmailSenderService _emailSenderService;
        EmailSettings _emailSettings;

        public BackgroundEmailWorker(IOptions<EmailSettings> emailSettings, Channel<EmailDataDto> emailQueue)
        {
            _emailSettings = emailSettings.Value;
            _emailQueue = emailQueue;
        }

        private async Task<bool> SendEmailAsync(string email, string htmlMessage, string subject)
        {
            var emailMessage = new MimeMessage();

            var senderName = _emailSettings.SenderName;
            var senderEmail = _emailSettings.SenderEmail;
            var senderUsername = _emailSettings.SenderUsername;
            var senderPassword = _emailSettings.SenderPassword;
            var host = _emailSettings.SmtpServer;
            var port = _emailSettings.SmtpPort;

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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var data in _emailQueue.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await SendEmailAsync(data.EmailAddress, data.EmailContent, data.Subject);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during sending mail: {ex.Message}");
                }
            }
        }
    }
}
