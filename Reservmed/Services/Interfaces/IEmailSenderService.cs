using Reservmed.Models.Identity;

namespace Reservmed.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task PrepareAndSendRegistrationEmail(ApplicationUser user, string name, string activationLink);

        Task PrepareAndSendResetPasswordEmail(ApplicationUser user, string resetToken);
    }

}
