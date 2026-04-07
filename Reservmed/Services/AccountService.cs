using Reservmed.Common;
using Reservmed.Data;
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
        private readonly ReservmedDBContext _dbContext;

        public AccountService(IAuthService authService,
            IDoctorService doctorService,
            IPatientService patientService,
            IEmailSenderService emailSenderService,
            ReservmedDBContext dbContext
        )
        {
            _authService = authService;
            _doctorService = doctorService;
            _patientService = patientService;
            _emailSenderService = emailSenderService;
            _dbContext = dbContext;
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

        private async Task<Result> CreateAccount(string email, string password, string userName, string role, Func<ApplicationUser, Task<Result>> domainCreationMethod)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
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
                    return Result.Error(domainUserCreationResult.Message ?? "Failed to create");

                }

                var addClaimResult = await _authService.AddUserClaimsAsync(identity, role, userName);
                if (!addClaimResult.IsSuccess)
                {
                    return Result.Error(addClaimResult.Message ?? "Failed to create");
                }

                string? token = null;
                if (isNewUser)
                {
                    token = await _authService.CreateRegistrationTokenAsync(identity);
                    if (token == null)
                    {
                        return Result.Error("Failed to generate token");
                    }

                }

                await transaction.CommitAsync();


                if (isNewUser)
                {
                    var task = Task.Run(() => _emailSenderService.PrepareAndSendRegistrationEmail(identity, userName, token));
                    // Move it to background task queue in the future
                }
                return Result.Success("Account Successfully Created");

            }
            catch (Exception)
            {
                return Result.Error("Unexpected error");
            }

        }

        public async Task<Result> RegisterDoctorAsync(DoctorRegistrationDto registrationData)
        {
            var doctorAlreadyExists = await _doctorService.IsDoctorExistingAsync(registrationData.Email);
            if (doctorAlreadyExists)
            {
                return Result.Error("Account already exists");
            }

            var result = await CreateAccount(
                 registrationData.Email,
                 registrationData.Password,
                 registrationData.FirstName,
                 UserRoles.Doctor,
                 async (ApplicationUser identity) => await _doctorService.CreateDoctorAccountAsync(identity, registrationData)
               );

            return result;
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
                UserRoles.Patient,
                async (ApplicationUser identity) => await _patientService.CreatePatientAccountAsync(identity, registrationData)


            );
        }

    }
}
