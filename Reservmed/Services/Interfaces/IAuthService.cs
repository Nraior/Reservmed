using Reservmed.Common;
using Reservmed.DTOs;
using Reservmed.DTOs.Internal;
using Reservmed.Models.Identity;
namespace Reservmed.Services.Interfaces
{
    public interface IAuthService
    {

        Task<Result<ApplicationUserCreationDto>> GetOrCreateIdentityAsync(string login, string password);

        Task<Result> RollbackUserIdentityAsync(ApplicationUser user);

        Task<string?> CreateRegistrationTokenAsync(ApplicationUser user);

        Task<ApplicationUser?> GetUserIdentityAsync(string email);
        Task<string> GenerateResetPasswordTokenAsync(ApplicationUser user);
        Task<Result> ResetPasswordAsync(string token, string newPassword);
        Task<Result> LoginAsync(LoginDto login);
        Task<Result> ConfirmRegistrationAsync(string token);
        Task<Result> LogoutAsync();
        Task<string> MeAsync(string mail);
    }
}
