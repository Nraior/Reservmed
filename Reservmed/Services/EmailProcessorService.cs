using Reservmed.Common;
using Reservmed.DTOs.Internal;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;
using System.Threading.Channels;

namespace Reservmed.Services
{
    public class EmailProcessorService : IEmailProcessorService
    {
        private readonly Channel<EmailDataDto> _emailQueue;

        public EmailProcessorService(Channel<EmailDataDto> emailQueue)
        {
            _emailQueue = emailQueue;
        }

        private record EmailDataParameters
        {
            public string? Email { get; init; }
            public required string TemplateFile { get; init; }
            public required string EmptyMailErrorMessage { get; init; }
            public required string LinkInEmail { get; init; }
            public required string NameInEmail { get; init; }
            public required string EmailSubject { get; init; }


        };

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

        private async Task PrepareAndQueueEmailAsync(EmailDataParameters prepareMailParams)
        {

            if (string.IsNullOrEmpty(prepareMailParams.Email))
            {
                throw new InvalidOperationException(prepareMailParams.EmptyMailErrorMessage);
            }
            var emailContent = await PrepareMailWithDefaultContentAsync(prepareMailParams.NameInEmail, prepareMailParams.LinkInEmail, prepareMailParams.TemplateFile);

            EmailDataDto emailDataDto = new EmailDataDto
            {
                EmailAddress = prepareMailParams.Email,
                EmailContent = emailContent,
                Subject = prepareMailParams.EmailSubject
            };

            await _emailQueue.Writer.WriteAsync(emailDataDto);

        }

        public async Task PrepareAndQueueRegistrationEmailAsync(ApplicationUser user, string name, string link)
        {

            EmailDataParameters prepareMailParams = new EmailDataParameters
            {
                Email = user.Email,
                LinkInEmail = link,
                NameInEmail = name,
                EmptyMailErrorMessage = "Cannot send registration email. User email is missing",
                TemplateFile = "RegistrationEmail.html",
                EmailSubject = EmailSubjects.AccountCreation
            };

            await PrepareAndQueueEmailAsync(prepareMailParams);

        }

        public async Task PrepareAndQueueResetPasswordEmailAsync(ApplicationUser user, string link)
        {

            EmailDataParameters prepareMailParams = new EmailDataParameters
            {
                Email = user.Email,
                LinkInEmail = link,
                NameInEmail = "",
                EmptyMailErrorMessage = "Cannot send password reset email. User email is missing",
                TemplateFile = "ResetPasswordEmail.html",
                EmailSubject = EmailSubjects.AccountPasswordReset
            };
            await PrepareAndQueueEmailAsync(prepareMailParams);

        }
    }
}
