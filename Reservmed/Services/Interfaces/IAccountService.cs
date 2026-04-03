using Reservmed.Common;
using Reservmed.DTOs;
namespace Reservmed.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<Result> RegisterPatientAsync(PatientRegistrationDto registrationData);
        public Task<Result> RegisterDoctorAsync(DoctorRegistrationDto registrationData);
        public Task<Result> AskForPasswordResetAsync(string email);
        public Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordData);


    }
}
