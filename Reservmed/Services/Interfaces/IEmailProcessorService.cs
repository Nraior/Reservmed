using Reservmed.Models.Identity;

namespace Reservmed.Services.Interfaces
{
    public interface IEmailProcessorService
    {
        Task PrepareAndQueueRegistrationEmailAsync(ApplicationUser user, string name, string activationLink);
        Task PrepareAndQueueEmailConfirmationEmailAsync(ApplicationUser user, string activationLink);
        Task PrepareAndQueueResetPasswordEmailAsync(ApplicationUser user, string resetToken);
    }

}
