using Reservmed.Common;
using Reservmed.DTOs;
using Reservmed.Models.Identity;

namespace Reservmed.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<Result> CreateDoctorAccountAsync(ApplicationUser identity, DoctorRegistrationDto registrationData);
        public Task<bool> IsDoctorExistingAsync(string email);
    }
}
