using Microsoft.EntityFrameworkCore;
using Reservmed.Common;
using Reservmed.Data;
using Reservmed.DTOs;
using Reservmed.Models.Domain;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;

namespace Reservmed.Services
{
    public class PatientService : IPatientService
    {
        private readonly ReservmedDBContext _dbContext;
        public PatientService(ReservmedDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> CreatePatientAccountAsync(ApplicationUser identity, PatientRegistrationDto registrationData)
        {

            Patient createdPatient = new Patient
            {
                ApplicationUserId = identity.Id,
                FirstName = registrationData.FirstName,
                LastName = registrationData.LastName,
                BirthDate = registrationData.BirthDate,
            };

            try
            {
                _dbContext.Patients.Add(createdPatient);
                await _dbContext.SaveChangesAsync();
                return Result.Success("Successfully created Patient Account");
            }
            catch (Exception ex)
            {
                return Result.Error("Failed to create Patient Account");
            }
        }

        public async Task<bool> IsPatientExistingAsync(string email)
        {
            return await _dbContext.Patients.AnyAsync((patient) => patient.User.Email == email);
        }


    }
}
