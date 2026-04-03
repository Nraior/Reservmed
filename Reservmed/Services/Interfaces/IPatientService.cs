using Reservmed.Common;
using Reservmed.DTOs;
using Reservmed.Models.Identity;

namespace Reservmed.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<Result> CreatePatientAccountAsync(ApplicationUser identity, PatientRegistrationDto registrationData);
        public Task<bool> IsPatientExistingAsync(string email);

    }
}
