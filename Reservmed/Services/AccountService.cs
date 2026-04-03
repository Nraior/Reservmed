using Reservmed.Common;
using Reservmed.DTOs;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;

namespace Reservmed.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthService _authService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IEmailSenderService _emailSenderService;

        public AccountService(IAuthService authService, IDoctorService doctorService, IPatientService patientService, IEmailSenderService emailSenderService)
        {
            _authService = authService;
            _doctorService = doctorService;
            _patientService = patientService;
            _emailSenderService = emailSenderService;
        }

        public async Task<Result> AskForPasswordResetAsync(string email)
        {

            var user = await _authService.GetUserIdentityAsync(email);

            if (user == null)
            {
                return Result.Success("Password reset successfully requested");

                // Add log 
            }

            var resetToken = await _authService.GenerateResetPasswordTokenAsync(user);
            // prepare reset link

            if (resetToken == null)
            {
                return Result.Error("Failed to request reset password");
            }

            await _emailSenderService.PrepareAndSendResetPasswordEmail(user, resetToken);

            return Result.Success("Password reset successfully requested");
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDto passwordResetData)
        {
            var result = await _authService.ResetPasswordAsync(passwordResetData.Token, passwordResetData.NewPassword);
            return result;
        }

        private async Task<Result> CreateAccount(string email, string password, string emailName, Func<ApplicationUser, Task<Result>> domainCreationMethod)
        {
            var result = await _authService.GetOrCreateIdentityAsync(email, password);
            ApplicationUser? identity = result?.Payload?.UserIdentity;
            bool isNewUser = result?.Payload?.IsNewUser ?? false;
            if (identity == null)
            {
                return Result.Error(result?.Message ?? "Failed to retrieve User identity");
            }

            var domainUserCreationResult = await domainCreationMethod(identity);

            if (!domainUserCreationResult.IsSuccess)
            {
                if (isNewUser)
                {

                    await _authService.RollbackUserIdentityAsync(identity);
                }

                return Result.Error(domainUserCreationResult.Message ?? "Failed to create");
            }

            if (isNewUser)
            {
                var token = await _authService.CreateRegistrationTokenAsync(identity);
                if (token == null)
                {
                    await _authService.RollbackUserIdentityAsync(identity);
                    return Result.Error("Failed to generate token");
                }
                else
                {
                    await _emailSenderService.PrepareAndSendRegistrationEmail(identity, emailName, token);
                }
            }

            return Result.Success("Account Successfully Created");

        }

        public async Task<Result> RegisterDoctorAsync(DoctorRegistrationDto registrationData)
        {
            var doctorAlreadyExists = await _doctorService.IsDoctorExistingAsync(registrationData.Email);
            if (doctorAlreadyExists)
            {
                return Result.Error("Account already exists");
            }

            return await CreateAccount(
                 registrationData.Email,
                 registrationData.Password,
                 registrationData.FirstName,
                 async (ApplicationUser identity) => await _doctorService.CreateDoctorAccountAsync(identity, registrationData)
               );
        }

        public async Task<Result> RegisterPatientAsync(PatientRegistrationDto registrationData)
        {
            var patientAlreadyExists = await _patientService.IsPatientExistingAsync(registrationData.Email);
            if (patientAlreadyExists)
            {
                return Result.Error("Account already exists");
            }

            return await CreateAccount(
                registrationData.Email,
                registrationData.Password,
                registrationData.FirstName,
                async (ApplicationUser identity) => await _patientService.CreatePatientAccountAsync(identity, registrationData)
            );
        }

    }
}
